using System;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.ConfigHelpers;
using LogisticsBooking.FrontEnd.DataServices.Models.MasterSchedule.Commands;
using LogisticsBooking.FrontEnd.DataServices.Models.MasterSchedule.ViewModels;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailsList;
using LogisticsBooking.FrontEnd.Documents;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace LogisticsBooking.FrontEnd.DataServices
{
    public class MasterShceduleDataService : BaseDataService, IMasterScheduleDataService
    {
        
        private string baseurl;
        
        
        public MasterShceduleDataService(IHttpContextAccessor httpContextAccessor, IOptions<BackendServerUrlConfiguration> config) : base(httpContextAccessor, config)
        {
            baseurl = _APIServerURL + "/api/masterScheduleStandard/";
        }

        public async Task<Response> CreateNewMasterSchedule(CreateNewMasterScheduleStandardCommand createMasterScheduleCommand)
        {
            var response = await PostAsync<CreateNewMasterScheduleStandardCommand>(baseurl, createMasterScheduleCommand);

            
            if (response.IsSuccessStatusCode)
            {
                return new Response(true );
            }
            return Response.Unsuccesfull();
        }

        public async Task<MasterSchedulesStandardViewModel> GetAllSchedules()
        {
            var response = await GetAsync(baseurl);
            var result = await TryReadAsync<MasterSchedulesStandardViewModel>(response);

            return result;
        }

        public async Task<Response> SetMasterScheduleActive(SetMasterScheduleStandardActiveCommand masterScheduleStandardActive)
        {
            var response = await PutAsync<SetMasterScheduleStandardActiveCommand>(baseurl , masterScheduleStandardActive);

            
            if (response.IsSuccessStatusCode)
            {
                return new Response(true );
            }
            return Response.Unsuccesfull();
        }

        public async Task<MasterScheduleStandardViewModel> GetActiveMasterSchedule()
        {
            var endpoint = baseurl + "active";
                
            var response = await GetAsync(endpoint);
            var result = await TryReadAsync<MasterScheduleStandardViewModel>(response);

            return result;
        }
    }
}