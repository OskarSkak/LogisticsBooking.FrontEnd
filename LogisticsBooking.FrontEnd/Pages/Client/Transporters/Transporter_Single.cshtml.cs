using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace LogisticsBooking.FrontEnd.Pages.Client.Transporters
{
    public class Transporter_SingleModel : PageModel
    {
        private ITransporterDataService dataService;
        [BindProperty]
        public DataServices.Models.Transporter Transporter { get; set; }

        [TempData]
        public Guid guid { get; set; }

        public Transporter_SingleModel(ITransporterDataService _dataService)
        {
            dataService = _dataService;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var TransporterName = id;
            var Transporters = await dataService.ListTransporters(0, 0);
            List<DataServices.Models.Transporter> TransporterList = (List<DataServices.Models.Transporter>)Transporters; 
            foreach(var item in TransporterList)
            {
                if (item.Name == TransporterName)
                {
                    Transporter = item;
                    guid = item.ID;
                }    
            }
            return Page();
        }

        public async Task<IActionResult> OnPostUpdate(string id, string ViewName, string ViewEmail, int ViewTelephone, string ViewAddress)
        {
            var Transporters = await dataService.ListTransporters(0, 0);
            Transporters = (List<DataServices.Models.Transporter>)Transporters;
            foreach (var item in Transporters)
                if (item.Name == id)
                    Transporter = item;

            Transporter.Name = ViewName;
            Transporter.Email = ViewEmail;
            Transporter.Telephone = ViewTelephone;
            Transporter.Address = ViewAddress;

            var result = await dataService.UpdateTransporter(Transporter.ID, Transporter);

            if (!result.IsSuccesfull)
                return new RedirectToPageResult("Error");

            return new RedirectToPageResult("./Transporters");
        }
    }
}