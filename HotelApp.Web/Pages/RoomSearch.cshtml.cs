using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HotelLibrary.Data;
using HotelLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelApp.Web.Pages
{
    public class RoomSearchModel : PageModel
    {
        private readonly IDatabaseData db;
        

        [BindProperty(SupportsGet = true)]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } = DateTime.Now.Date;

        [BindProperty(SupportsGet = true)]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(1); 

        [BindProperty(SupportsGet = true)] 
        public bool SearchEnabled { get; set; } = false;

        public List<RoomTypeModel> AvailableRoomTypes { get; set; }

        public RoomSearchModel(IDatabaseData db) 
        {
            this.db = db;
            
        }

        public void OnGet()
        {
            if(SearchEnabled == true)
            {
                AvailableRoomTypes = db.GetAvailableRoomTypes(StartDate, EndDate); 
            }
        }

        public IActionResult OnPost()
        {
            return RedirectToPage(new 
            { 
                SearchEnabled = true, 
                StartDate = StartDate.ToString("yyyy-MM-dd"),  
                EndDate = EndDate.ToString("yyyy-MM-dd") 
            }); 
        }
    }
}
