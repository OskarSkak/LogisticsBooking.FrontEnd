using System;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models.ApplicationUser;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Client.admin
{
    public class CreateApplicationUser : PageModel
    {
        private readonly IApplicationUserDataService _applicationUserDataService;

        [TempData]
        public string Message { get; set; }
        
        
        public bool MessageIsNull => !String.IsNullOrEmpty(Message);
        
        public CreateUserCommand CreateUserCommand { get; set; }
        
        public CreateApplicationUser(IApplicationUserDataService applicationUserDataService)
        {
            _applicationUserDataService = applicationUserDataService;
        }
        
        public void OnGet()
        {
            
        }


        public async Task<IActionResult> OnPostCreateAsync(CreateUserCommand createUserCommand)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _applicationUserDataService.CreateUser(createUserCommand);

            if (result.IsSuccesfull)
            {
                Message = "User created. Check email for confirmation link";
            }

            return Page();
        }
    }
}