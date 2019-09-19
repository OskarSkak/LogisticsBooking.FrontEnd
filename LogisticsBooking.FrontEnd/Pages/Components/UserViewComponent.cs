using System;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsBooking.FrontEnd.Pages.Components
{
    public class UserViewComponent : ViewComponent
    {
        private readonly ITransporterDataService _transporterDataService;

        public UserViewComponent(ITransporterDataService transporterDataService)
        {
            _transporterDataService = transporterDataService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user  = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "sub").Value;

            var transporter = await _transporterDataService.GetTransporterById(Guid.Parse(user));

            return View(transporter);
        }
    }
    
    
}