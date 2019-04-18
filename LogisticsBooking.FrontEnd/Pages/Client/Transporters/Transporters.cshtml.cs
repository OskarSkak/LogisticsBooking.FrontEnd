using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Client
{
    public class TransportersModel : PageModel
    {
        private ITransporterDataService transporterDataService;
        [BindProperty] public List<DataServices.Models.Transporter> Transporters { get; set; }

        public TransportersModel(ITransporterDataService _transporterDataService)
        {
            transporterDataService = _transporterDataService;
            Transporters = new List<DataServices.Models.Transporter>();
            Transporters = PopulateList(transporterDataService, Transporters).Result;
        }
        public async void OnGet()
        {
        }

        private static async Task<List<DataServices.Models.Transporter>> PopulateList(ITransporterDataService transporterDataService, List<DataServices.Models.Transporter> Transporters)
        {
            var TransportersEnumerable = await transporterDataService.ListTransporters(0, 0);
            Transporters = (List<DataServices.Models.Transporter>)TransportersEnumerable;
            return Transporters;
        }
    }
}