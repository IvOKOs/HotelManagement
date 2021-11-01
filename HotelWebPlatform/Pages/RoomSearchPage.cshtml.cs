using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelLibrary.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelApp.Web.Pages
{
    public class RoomSearchPageModel : PageModel
    {
        //SqlData data = new SqlData(); 


        [BindProperty] 
        public DateTime StartDate { get; set; }

        [BindProperty] 
        public DateTime EndDate { get; set; }



        public void OnGet()
        {
            //data.GetAvailableRoomTypes(StartDate, EndDate); 
        }

        public IActionResult OnPost()
        {
            return Page(); 
        }
    }
}
