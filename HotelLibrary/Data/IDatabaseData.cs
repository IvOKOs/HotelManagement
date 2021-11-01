using HotelLibrary.Models;
using System;
using System.Collections.Generic;

namespace HotelLibrary.Data
{
    public interface IDatabaseData
    {
        void BookGuest(DateTime startDate, DateTime endDate, string firstName, string lastName, int roomTypeId);
        void CheckInGuest(int bookingId);
        List<RoomTypeModel> GetAvailableRoomTypes(DateTime startDate, DateTime endDate);
        RoomTypeModel GetRoomTypeById(int id);
        List<BookingFullModel> SearchBookings(string lastName);
    }
}