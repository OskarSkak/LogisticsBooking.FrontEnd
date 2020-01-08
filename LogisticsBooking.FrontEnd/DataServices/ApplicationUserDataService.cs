using System.Collections.Generic;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.ConfigHelpers;
using LogisticsBooking.FrontEnd.DataServices.Models.ApplicationUser;
using LogisticsBooking.FrontEnd.Documents;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace LogisticsBooking.FrontEnd.DataServices
{
    public class ApplicationUserDataService : BaseDataService , IApplicationUserDataService
    {
        private readonly IOptions<IdentityServerConfiguration> _identityserverConfig;
        private string URL;
        private string URlCreate;

        public ApplicationUserDataService(IHttpContextAccessor httpContextAccessor, IOptions<BackendServerUrlConfiguration> config , IOptions<IdentityServerConfiguration> identityserverConfig) : base(httpContextAccessor, config)
        {
            _identityserverConfig = identityserverConfig;
            URL = _identityserverConfig.Value.IdentityServerUrl + "/ApplicationUsers";
            URlCreate = _identityserverConfig.Value.IdentityServerUrl + "/CreateApplicationUser";
        }

        public async Task<ListApplicationUserViewModels> GetAllUsers()
        {
            var response = await GetAsync(URL);
            var result = await TryReadAsync<ListApplicationUserViewModels>(response);
            return result;
        }

        public async Task<Response> CreateUser(CreateUserCommand createUserCommand)
        {
            var response = await PostAsync<CreateUserCommand>(URlCreate, createUserCommand);
            if (response.IsSuccessStatusCode)
            {
                return new Response(true );
            }
            return Response.Unsuccesfull();
        }
    }
}