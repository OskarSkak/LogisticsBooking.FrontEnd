using System;
using System.Linq;
using LogisticsBooking.FrontEnd.Pages.Transporter.Booking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Client.Schedule
{

    public class person
    {
        public string Name { get; set; }
    }
    
    public class Calendar : PageModel
    {
        [BindProperty] 
        public DateTime date { get; set; }
        
        public int currentMonth { get; set; }
        public int CurrentYear { get; set; }


        public Calendar()
        {
            date = DateTime.Today;
        }
      
        public void OnGet(string id)
        {


            Console.WriteLine(id);
            var v = Guid.Parse(id);
            
            
            var Subjectid = "";
            
           
            Subjectid = User.Claims.FirstOrDefault(x => x.Type == "sub").Value;

            var result = HttpContext.Session.GetObject<DateTime>(Subjectid);
            if (result == DateTime.MinValue)
            {
                result = date;
                result =  new DateTime(date.Year, date.Month, 1);
            }
            
            date = result;
            currentMonth = date.Month;
            CurrentYear = date.Year;
            Console.WriteLine(date);

        }


        [ValidateAntiForgeryToken]
        [EnableCors("MyPolicy")]
        public IActionResult OnPost([FromBody]string[] value)
        {
            Console.WriteLine("msoæpqnm3dpoqædnpaeodnpaeidfnipwedfnwepn");
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] != null)
                {
                    var a = DateTime.Parse(value[i]);
                    Console.WriteLine(a);
                    Console.WriteLine(DateTime.Today.AddMonths(7).ToString("d"));
                    Console.WriteLine(DateTime.Today.AddMonths(7).Month);
                }
            }
            return new RedirectToPageResult("Calendar");
        }

        public IActionResult OnPostForward()
        {
            var id = "";
            
           
                id = User.Claims.FirstOrDefault(x => x.Type == "sub").Value;
                var result = HttpContext.Session.GetObject<DateTime>(id);
                if (result == DateTime.MinValue)
                {
                    result =  new DateTime(date.Year, date.Month, 1);
                    
                }
                else
                {
                    result = new DateTime(result.Year, result.Month, 1);
                    
                }

                currentMonth = (result.Month + 1) % 13;

                if (currentMonth == 0)
                {
                    currentMonth++;
                    CurrentYear = result.Year;
                    CurrentYear++;
                    result = new DateTime(CurrentYear, currentMonth, 1);
                }
                else
                {
                    result = new DateTime(result.Year, currentMonth, 1);
                }
                
                
                
                HttpContext.Session.SetObject(id , result);
                
                return new RedirectToPageResult("Calendar");
        }
    

        public void getDate()
        {
            
        }

        public IActionResult OnPostBack()
        {
            var id = User.Claims.FirstOrDefault(x => x.Type == "sub").Value;
            var result = HttpContext.Session.GetObject<DateTime>(id);

            currentMonth = result.Month;
            CurrentYear = result.Year;
            
            if (result == DateTime.MinValue)
            {
                result =  new DateTime(date.Year, date.Month, 1);
                    
            }
            else
            {
                result = new DateTime(result.Year, result.Month, 1);
                    
            }
            if (currentMonth == 1)
            {
                currentMonth=12;
                CurrentYear-- ;
            }
            else
            {
                currentMonth--;
            }
            
            
            result = new DateTime(CurrentYear, currentMonth, 1);
            HttpContext.Session.SetObject( id , result);


            return new RedirectToPageResult("Calendar");
        }
        
        
    }
}