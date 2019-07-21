

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.ConfigHelpers;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.Documents;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace LogisticsBooking.FrontEnd.DataServices
{
    public class ScheduleDataService : BaseDataService, IScheduleDataService
    {
        private string baseurl;
        
        public ScheduleDataService(IHttpContextAccessor httpContextAccessor, IOptions<BackendServerUrlConfiguration> config) : base(httpContextAccessor, config)
        {
            baseurl = _APIServerURL + "/api/schedules/";
        }


        public async Task<Response> CreateSchedule(Schedule schedule)
        {
            var response = await PostAsync<Schedule>(baseurl, schedule);
            
            if (response.IsSuccessStatusCode)
            {
                return new Response(true );
            }
            return new Response(false);
        }

        public async Task<List<Schedule>> GetSchedules()
        {
            var response = await GetAsync(this.baseurl);
            
            var result = await TryReadAsync<List<Schedule>>(response);

            return result;
        }

        public async Task<Schedule> GetScheduleById(Guid id)
        {
            var endpoint = baseurl + id;
            var result = await GetAsync(endpoint);
            return await TryReadAsync<Schedule>(result);
        }

        public async Task<Response> UpdateSchedule(Schedule schedule)
        {
            var endpoint = baseurl + schedule.ScheduleId;  
        
            var response = await PutAsync<Schedule>(endpoint, schedule);

            if (!response.IsSuccessStatusCode)
            {
                if (response.Content != null)
                {
                    var errorMsg = await response.Content.ReadAsStringAsync();
                    return Response.Unsuccesfull(errorMsg);
                }
                return Response.Unsuccesfull(response.ReasonPhrase);
            }
            return Response.Succes();
        }

        public async Task<Response> DeleteSchedule(Guid id)
        {
            var endpoint = baseurl + id;
            var response = await DeleteAsync(endpoint);
            if (!response.IsSuccessStatusCode)
            {
                if (response.Content != null)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return Response.Unsuccesfull(errorMessage);
                }
                return Response.Unsuccesfull(response.ReasonPhrase);
            }
            return Response.Succes();
        }

        public async Task<Response> CreateManySchedule(ManySchedules schedule)
        {
            baseurl = baseurl + "list";
            var response = await PostAsync(baseurl, schedule);
            
            if (response.IsSuccessStatusCode)
            {
                return new Response(true );
            }
            return new Response(false);
        
        }

        public async Task<Schedule> GetScheduleBydate(DateTime date)
        {
            baseurl = baseurl + "date?date=" + date.ToString("yyyy-MM-dd");
            var response = await GetAsync(baseurl);

            return await TryReadAsync<Schedule>(response);
        }
    }
}