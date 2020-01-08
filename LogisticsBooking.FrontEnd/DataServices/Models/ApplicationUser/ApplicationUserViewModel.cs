using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace LogisticsBooking.FrontEnd.DataServices.Models.ApplicationUser
{
    public class ApplicationUserViewModel
    {
        public string ApplicationUserId { get; set; }
        public IdentityRole Role { get; set; } = new IdentityRole();
        public string Email { get; set; }
        public bool Active { get; set; }
    }

    public class ListApplicationUserViewModels
    {
        public List<ApplicationUserViewModel> ApplicationUserViewModels { get; set; } = new List<ApplicationUserViewModel>();
    }
}