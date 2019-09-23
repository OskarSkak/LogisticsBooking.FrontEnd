using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models.Transporter.Transporter;
using LogisticsBooking.FrontEnd.PagesEntity.Transporter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace LogisticsBooking.FrontEnd.Pages.Client.Transporters
{
    public class Transporter_SingleModel : PageModel
    {
        private readonly ITransporterDataService _transporterDataService;
        private readonly IMapper _mapper;


        [BindProperty] public TransporterUpdateBuildModel TransporterUpdateBuildModel { get; set; }


        [TempData] public String ResponseMessage { get; set; }

        public Transporter_SingleModel(ITransporterDataService transporterDataService, IMapper mapper)
        {
            _transporterDataService = transporterDataService;
            _mapper = mapper;
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var transporterViewModel = await _transporterDataService.GetTransporterById(id);
            
            TransporterUpdateBuildModel = _mapper.Map<TransporterUpdateBuildModel>(transporterViewModel);

            return Page();
        }

        public async Task<IActionResult> OnPostUpdate(TransporterUpdateBuildModel transporterUpdateBuildModel)
        {
            var transporter = _mapper.Map<TransporterViewModel>(TransporterUpdateBuildModel);

            if (transporter == null)
            {
                return Page();
            }

            var result = await _transporterDataService.UpdateTransporter(transporter.TransporterId, transporter);

            if (!result.IsSuccesfull)
            {
                return new RedirectToPageResult("Error");
            }

            ResponseMessage = "Opdatering af transportør var vellykket";
            return new RedirectToPageResult("./Transporters");
        }

        public async Task<IActionResult> OnPostDelete(TransporterUpdateBuildModel transporterUpdateBuildModel)
        {
            var result = await _transporterDataService.DeleteTransporter(transporterUpdateBuildModel.TransporterId);
            ResponseMessage = "Transportøren er slettet korrekt";
            return new RedirectToPageResult("./Transporters");
        }
    }
}