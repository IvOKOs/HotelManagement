using HotelLibrary.Databases;
using HotelLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelLibrary.Data
{
    public class SqliteData : IDatabaseData 
    {

        private readonly string connectionStringName = "SqliteDb"; 
        private readonly ISqliteDataAccess db; 

        public SqliteData(ISqliteDataAccess db)
        {
            this.db = db;
        }

        public void BookGuest(DateTime startDate, DateTime endDate, string firstName, string lastName, int roomTypeId)
        {
            string sql = @"select 1 from Guests where FirstName = @firstName and LastName = @lastName;";
            int result = db.LoadData<dynamic, dynamic>(sql, new { firstName, lastName }, connectionStringName).Count(); 

            if(result == 0)
            {
                sql = @"insert into Guests(FirstName, LastName) values(@firstName, @lastName);";
                db.SaveData(sql, new { firstName, lastName }, connectionStringName); 
            }

            sql = @"select [Id], [FirstName], [LastName]
                    from Guests
                    where FirstName = @firstName and LastName = @lastName LIMIT 1;"; 
            GuestModel guest = db.LoadData<GuestModel, dynamic>(sql,
                                                                new { firstName, lastName },
                                                                connectionStringName).First();

            sql = @"select r.* 
	            from Rooms r 
	            inner join RoomTypes t on t.Id = r.RoomTypeId 
	            where r.RoomTypeId = @roomTypeId
	            and r.Id not in(
	            select b.RoomId 
	            from Bookings b 
	            where(@startDate < b.StartDate and @endDate > b.EndDate)
		            or(@startDate >= b.StartDate and @startDate < b.EndDate)
		            or(@endDate >= b.StartDate and @endDate < b.EndDate) 
	            )"; 
            List<RoomModel> availableRooms = db.LoadData<RoomModel, dynamic>(sql,
                                                                             new { startDate, endDate, roomTypeId },
                                                                             connectionStringName); 
            sql = "select * from RoomTypes where Id = @id;";
            RoomTypeModel roomType = db.LoadData<RoomTypeModel, dynamic>(sql,
                                                                         new { id = roomTypeId },
                                                                         connectionStringName).First();

            TimeSpan timeStaying = endDate.Date.Subtract(startDate.Date);

            sql = @"insert into Bookings(RoomId, GuestId, StartDate, EndDate, TotalCost)
	                values(@roomId, @guestId, @startDate, @endDate, @totalCost)";

            db.SaveData(sql,
                        new 
                        { 
                            roomId = availableRooms.First().Id, 
                            guestId = guest.Id, 
                            startDate, 
                            endDate, 
                            totalCost = timeStaying.Days * roomType.Price 
                        },
                        connectionStringName); 
        }

        public void CheckInGuest(int bookingId)
        {
            string sql = @"update Bookings 
	                        set CheckedIn = 1 
	                        where Id = @Id;";  

            db.SaveData(sql, new { Id = bookingId }, connectionStringName); 
        }

        public List<RoomTypeModel> GetAvailableRoomTypes(DateTime startDate, DateTime endDate)
        {
            string sql = @"select t.Id, t.Title, t.Description, t.Price 
	                    from Rooms r 
	                    inner join RoomTypes t on t.Id = r.RoomTypeId 
	                    where r.Id not in(
	                    select b.RoomId 
	                    from Bookings b 
	                    where(@startDate < b.StartDate and @endDate > b.EndDate)
		                    or(@startDate >= b.StartDate and @startDate < b.EndDate)
		                    or(@endDate >= b.StartDate and @endDate < b.EndDate) 
	                    )
	                    group by t.Id, t.Title, t.Description, t.Price; ";

            var output = db.LoadData<RoomTypeModel, dynamic>(sql,
                                                       new { startDate, endDate },
                                                       connectionStringName);

            output.ForEach(x => x.Price = x.Price / 100);

            return output; 
        }

        public RoomTypeModel GetRoomTypeById(int id)
        {
            string sql = @"select [Id], [Title], [Description], [Price] 
	                        from RoomTypes 
	                        where Id = @id;"; 
            return db.LoadData<RoomTypeModel, dynamic>(sql, new { id }, connectionStringName).FirstOrDefault(); 
        }

        public List<BookingFullModel> SearchBookings(string lastName)
        {
            string sql = @"select [b].[Id], [b].[RoomId], [b].[GuestId], [b].[StartDate], [b].[EndDate], [b].[CheckedIn], [b].[TotalCost],
		                    [g].[FirstName], [g].[LastName], [r].[RoomNumber], [r].[RoomTypeId], [t].[Title], [t].[Description], [t].[Price] 
	                        from Bookings b
	                        inner join Guests g on b.GuestId = g.Id
	                        inner join Rooms r on b.RoomId = r.Id 
	                        inner join RoomTypes t on r.RoomTypeId = t.Id 
	                        where b.StartDate = @startDate and g.LastName = @lastName;"; 

            var bookings = db.LoadData<BookingFullModel, dynamic>(sql,
                                                             new { lastName, startDate = DateTime.Now.Date },
                                                             connectionStringName);
            bookings.ForEach(x =>
            {
                x.Price = x.Price / 100;
                x.TotalCost = x.TotalCost / 100;
            }); 

            return bookings;
        }
    }
}
