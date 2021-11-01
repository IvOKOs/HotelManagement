using HotelLibrary.Databases;
using HotelLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelLibrary.Data
{
    public class SqlData : IDatabaseData 
    {
        private readonly ISqlDataAccess db;
        private const string connectionStringName = "SqlDb";

        public SqlData(ISqlDataAccess db) 
        {
            this.db = db;
        }
        public List<RoomTypeModel> GetAvailableRoomTypes(DateTime startDate, DateTime endDate)
        {
            return db.LoadData<RoomTypeModel, dynamic>("dbo.spRoomTypes_GetAvailableTypes",
                                                       new { startDate, endDate },
                                                       connectionStringName,
                                                       true);
        }

        public void BookGuest(DateTime startDate, DateTime endDate, string firstName, string lastName, int roomTypeId)
        {
            GuestModel guest = db.LoadData<GuestModel, dynamic>("dbo.spGuest_Insert",
                                                                new { firstName, lastName },
                                                                connectionStringName,
                                                                true).First();

            List<RoomModel> availableRooms = db.LoadData<RoomModel, dynamic>("dbo.spRooms_GetAbailableRooms",
                                                                             new { startDate, endDate, roomTypeId },
                                                                             connectionStringName,
                                                                             true);
            string sql = "select * from dbo.RoomTypes where Id = @id;";
            RoomTypeModel roomType = db.LoadData<RoomTypeModel, dynamic>(sql,
                                                                         new { id = roomTypeId },
                                                                         connectionStringName,
                                                                         false).First();

            TimeSpan timeStaying = endDate.Date.Subtract(startDate.Date);


            db.SaveData("dbo.spBookings_Insert",
                        new { roomId = availableRooms.First().Id, guestId = guest.Id, startDate, endDate, totalCost = timeStaying.Days * roomType.Price },
                        connectionStringName,
                        true); 
        }

        public List<BookingFullModel> SearchBookings(string lastName)
        {
            var bookings = db.LoadData<BookingFullModel, dynamic>("dbo.spBookings_Search",
                                                             new { lastName, startDate = DateTime.Now.Date },
                                                             connectionStringName,
                                                             true);
            return bookings;
        }

        public void CheckInGuest(int bookingId)
        {
            db.SaveData("dbo.spBookings_CheckIn", new { Id = bookingId }, connectionStringName, true);
        }

        public RoomTypeModel GetRoomTypeById(int id)
        {
            return db.LoadData<RoomTypeModel, dynamic>("dbo.spRoomTypes_GetById", new { id }, connectionStringName, true).FirstOrDefault();  
        }

    }
}
