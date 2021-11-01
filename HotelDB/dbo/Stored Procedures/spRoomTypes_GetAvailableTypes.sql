CREATE PROCEDURE [dbo].[spRoomTypes_GetAvailableTypes]
	@startDate date, 
	@endDate date
AS
begin
	set nocount on; 

	select t.Id, t.Title, t.Description, t.Price 
	from dbo.Rooms r 
	inner join dbo.RoomTypes t on t.Id = r.RoomTypeId 
	where r.Id not in(
	select b.RoomId 
	from dbo.Bookings b 
	where(@startDate < b.StartDate and @endDate > b.EndDate)
		or(@startDate >= b.StartDate and @startDate < b.EndDate)
		or(@endDate >= b.StartDate and @endDate < b.EndDate) 
	)
	group by t.Id, t.Title, t.Description, t.Price 

	
end
