using LogisticsBooking.FrontEnd.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace LogisticsBooking.FrontEnd.DataServices
{
    public class BaseDataService
    {
        protected  IHttpContextAccessor _httpContextAccessor;

        public BaseDataService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        

        /// <summary>
        /// Post with body
        /// </summary>
        /// <typeparam name="T">Entity or value</typeparam>
        /// <param name="baseurl">Backend server end-point</param>
        /// <param name="Entity">Entity (what is to be posted)</param>
        /// <returns>HttpResponse message (Either succesfull or null)</returns>
        protected async Task<HttpResponseMessage> PostAsync<T>(string baseurl, T Entity)
        {
            HttpClient Client = new HttpClient();

            //Set the client to accept json in body
            Client.DefaultRequestHeaders
                .Accept
                .Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var Content = JsonConvert.SerializeObject(Entity, Formatting.Indented);
            var HttpContent = new StringContent(JsonConvert.SerializeObject(Entity, Formatting.Indented), Encoding.UTF8, "application/json");

            HttpResponseMessage HttpResponse = null;

            try
            {
                HttpResponse = await Client.PostAsync(baseurl, HttpContent);
            }
            catch (WebException ex)
            {
                //Logs the exception (Hopefully) in the txt doc in app data
                ExceptionUtility.LogException(ex, "Base DataServices, PostAsync<T>");
            }

            return HttpResponse;
        }

        /// <summary>
        /// PostAsync without any entity or value
        /// </summary>
        /// <param name="baseurl">Backend Server endpoint</param>
        /// <returns>Either succesfull/unsuccessfull HttpResponse or null</returns>
        protected async Task<HttpResponseMessage> PostAsync(string baseurl)
        {
            HttpClient client;
            client = new HttpClient();
            //Set the client to accept json in body
            client.DefaultRequestHeaders
                .Accept
                .Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var httpContent = new StringContent(JsonConvert.SerializeObject(null, Formatting.Indented), System.Text.Encoding.UTF8, "application/json");

            return await client.PostAsync(baseurl, httpContent);
        }

        protected async Task<HttpResponseMessage> PostManyAsync<T>(string baseurl, T entity)
        {
            HttpClient client;
            client = new HttpClient();
            
            //Set the client to accept json in body
            client.DefaultRequestHeaders
                .Accept
                .Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            
            
            var cont = JsonConvert.SerializeObject(entity, Formatting.Indented);
            var httpContent = new StringContent(JsonConvert.SerializeObject(entity, Formatting.Indented), System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage httpResponse = null;

            try
            {
                httpResponse = await client.PostAsync(baseurl, httpContent);
            }
            catch (WebException ex)
            {
                //Logs the exception (Hopefully) in the txt doc in app data
                ExceptionUtility.LogException(ex, "Base DataServices, PostAsync<T>");
            }
            return httpResponse;
        }

        protected async Task<HttpResponseMessage> PutAsync<T>(string baseurl, T entity)
        {
            HttpClient client;
            client = new HttpClient();
            var httpContent = new StringContent(JsonConvert.SerializeObject(entity, Formatting.Indented), System.Text.Encoding.UTF8, "application/json");

            return await client.PutAsync(baseurl, httpContent);
        }

        protected async Task<HttpResponseMessage> DeleteAsync(string baseurl)
        {
            HttpClient client;

            client = new HttpClient();

            return await client.DeleteAsync(baseurl);
        }

        protected async Task<HttpResponseMessage> GetAsync(string baseurl)
        {
            HttpClient client;

            client = new HttpClient();
            var token = string.Empty;

            var currentContext = _httpContextAccessor.HttpContext;

            token = await currentContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer" , token);

            HttpResponseMessage result = null;
            try
            {
                result = await client.GetAsync(baseurl);
            }
            catch (WebException ex)
            {
                //Logs the exception (Hopefully) in the txt doc in app data
                ExceptionUtility.LogException(ex, "Base DataServices, PostAsync<T>");
            }

            return result;
        }

        protected async Task<T> TryReadAsync<T>(HttpResponseMessage response) where T : class
        {
            if (response.Content == null || !response.IsSuccessStatusCode)
            {
                return null; // <-- Dont eat the error here!
            }

            var content = await response.Content.ReadAsStringAsync();

            Console.WriteLine(content);
            return JsonConvert.DeserializeObject<T>(content);
        }

        protected async Task<IEnumerable<T>> GetListAsync<T>(HttpResponseMessage responseMessage) where T : class
        {
            if (responseMessage.Content == null)
            {
                return null; // <-- Dont eat the error here!
            }

            var content = await responseMessage.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IEnumerable<T>>(content);
        }

    }


}
