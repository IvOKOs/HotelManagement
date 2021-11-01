using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelLibrary.Data;
using HotelLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelApp.Web.Pages
{
    public class RoomBookingModel : PageModel
    {
        private readonly IDatabaseData db; 

        [BindProperty(SupportsGet = true)]
        public string FirstName { get; set; } 


        [BindProperty(SupportsGet = true)] 
        public string LastName { get; set; }


        [BindProperty(SupportsGet = true)]
        public DateTime StartDate { get; set; }


        [BindProperty(SupportsGet = true)]
        public DateTime EndDate { get; set; }


        [BindProperty(SupportsGet = true)] 
        public int RoomTypeId { get; set; }

        public RoomTypeModel RoomType { get; set; }




        public RoomBookingModel(IDatabaseData db)
        {
            this.db = db;
        }

        public void OnGet()
        {
            if(RoomTypeId > 0)
            {
                RoomType = db.GetRoomTypeById(RoomTypeId); 
            }
        }

        public IActionResult OnPost()
        {
            db.BookGuest(StartDate, EndDate, FirstName, LastName, RoomTypeId);
            return RedirectToPage("/Index"); 
        }
    }
}
