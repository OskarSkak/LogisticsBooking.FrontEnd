using System;
using System.Threading.Tasks;
using AutoMapper;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models.Transporter.Transporter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Client.Transporters
{
    public class Transporter_CreateModel : PageModel
    {
        private readonly ITransporterDataService _transporterDataService;
        private readonly IMapper _mapper;


        [BindProperty]
        public TransporterViewModel TransporterViewModel { get; set; }
        
        [TempData]
        public String ResponseMessage { get; set; }
        
        public Transporter_CreateModel(ITransporterDataService transporterDataService , IMapper mapper)
        {
            _transporterDataService = transporterDataService;
            _mapper = mapper;
        }

        public void OnGet()
        {
        }

        public async Task<ActionResult> OnPost(TransporterViewModel transporterViewModel)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _transporterDataService.CreateTransporter(transporterViewModel);

            if (!result.IsSuccesfull)
            {
                //
            }
            ResponseMessage = "Transportøren er oprettet";
            return new RedirectResult("Transporters");
        }
    }
}