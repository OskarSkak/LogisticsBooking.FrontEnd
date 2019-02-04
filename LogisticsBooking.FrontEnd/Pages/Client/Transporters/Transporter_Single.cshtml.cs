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
        public Guid guid { get; set; }

        public Transporter_SingleModel(ITransporterDataService _dataService)
        {
            dataService = _dataService;
            dataService.UpdateTransporter(Guid.Parse("f0b5c672-a390-47c7-97dc-1623d5c86df4"), new DataServices.Models.Transporter
            {
                Email = "PLEEEEEEEEASE",
                Address = "COMEOOOOOOOON",
                Name = "LETSGOOOOOO",
                Telephone = 20234203
            });


            var la = ""; 
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

        public async Task<IActionResult> OnPostUpdate()
        {
            var result = await dataService.UpdateTransporter(Transporter.ID, Transporter);

            if (!result.IsSuccesfull)
                return new RedirectResult("Error");

            return new RedirectResult("./Transporters");
        }
    }
}