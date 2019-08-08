using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LogisticsBooking.FrontEnd.DataServices.Models.Transporter.Transporter;

namespace LogisticsBooking.FrontEnd.Pages.Client.Transporters
{
    public class Transporter_DeleteModel : PageModel
    {
        private ITransporterDataService _dataService {get; set;}
        
        [BindProperty]
        public TransporterViewModel transporter { get; set; }

        public Transporter_DeleteModel(ITransporterDataService DS)
        {
            _dataService = DS;
        }

        public async Task<IActionResult> OnGet(string id)
        {
            this.transporter = await _dataService.GetTransporterByName(id);
            return Page();
        }

        public async Task<ActionResult> OnPost(string id)
        {

            var tran = await _dataService.GetTransporterByName(id);
            var result = await _dataService.DeleteTransporter(tran.TransporterId);
            
            if(result.IsSuccesfull) return new RedirectToPageResult("./Transporters");
            
            return new RedirectToPageResult("Error");
        }
    }
}