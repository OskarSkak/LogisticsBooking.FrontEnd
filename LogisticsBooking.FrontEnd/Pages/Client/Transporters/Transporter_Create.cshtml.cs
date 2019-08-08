using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models.Transporter.Transporter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Client.Transporters
{
    public class Transporter_CreateModel : PageModel
    {
        private ITransporterDataService _dataService {get; set;}

        public Transporter_CreateModel(ITransporterDataService DS)
        {
            _dataService = DS;
        }

        public void OnGet()
        {
        }

        public async Task<ActionResult> OnPost(string Name, string Email, int Telephone, string Address)
        {
            var result = await _dataService.CreateTransporter(new TransporterViewModel
            {
                Email = Email,
                Telephone = Telephone,
                Address = Address,
                Name = Name
            });

            if (!result.IsSuccesfull)
            {
                return new RedirectResult("Error");
            }

            return new RedirectResult("Transporters");
        }
    }
}