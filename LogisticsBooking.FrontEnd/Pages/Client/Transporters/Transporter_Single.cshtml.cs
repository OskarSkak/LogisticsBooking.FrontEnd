using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models.Transporter.Transporter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace LogisticsBooking.FrontEnd.Pages.Client.Transporters
{
    public class Transporter_SingleModel : PageModel
    {
        private ITransporterDataService dataService;
        [BindProperty]
        public TransporterViewModel transporter { get; set; }

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
             
            foreach(var item in Transporters.Transporters)
            {
                if (item.Name == TransporterName)
                {
                    transporter = item;
                    guid = item.TransporterId;
                }    
            }
            return Page();
        }

        public async Task<IActionResult> OnPostUpdate(string id, string ViewName, string ViewEmail, int ViewTelephone, string ViewAddress)
        {
            var Transporters = await dataService.ListTransporters(0, 0);
            foreach (var item in Transporters.Transporters)
                if (item.Name == id)
                    transporter = item;

            transporter.Name = ViewName;
            transporter.Email = ViewEmail;
            transporter.Telephone = ViewTelephone;
            transporter.Address = ViewAddress;

            var result = await dataService.UpdateTransporter(transporter.TransporterId, transporter);

            if (!result.IsSuccesfull)
                return new RedirectToPageResult("Error");

            return new RedirectToPageResult("./Transporters");
        }
        
        public async Task<IActionResult> OnPostDelete(string id)
        {
            var transporter = await dataService.GetTransporterByName(id);
            
            var result = await dataService.DeleteTransporter(transporter.TransporterId);
            
            return new RedirectToPageResult("./Transporters");
        }

    }
}