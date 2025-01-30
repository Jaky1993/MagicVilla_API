using Azure.Core;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.services.IServices;
using Newtonsoft.Json;
using System.Security.AccessControl;
using System.Security.Cryptography.Xml;
using System.Text;

namespace MagicVilla_Web.services
{
    public class BaseService : IBaseService
    {
        public APIResponse responseModel { get; set; }

        public IHttpClientFactory httpClient {  get; set; }
        //IHttpClientFactory is an interface in .NET Core that provides a central location for naming
        //and configuring logical HttpClient instances.

        //HttpClient: The HttpClient class in .NET is a fundamental part of the System.Net.Http namespace.
        //It's designed to provide a flexible and efficient way to send HTTP requests and receive HTTP responses from
        //a resource identified by a URI.

        public BaseService(IHttpClientFactory httpClient)
        {
            this.responseModel = new(); //istanzio la classe APIResponse
            this.httpClient = httpClient;
        }

        //It looks like you're creating a method to send an HTTP request and deserialize the response into a specific type T
        public async Task<T> SendAsync<T>(APIRequest apiRequest)
        {
            try
            {
                var client = httpClient.CreateClient("MagicAPI"); //CreateClient("MyClient") creates a new HttpClient instance
                //with the name "MyClient". You can configure the named client in the Startup class

                //Creating an HttpRequestMessage instance is a great way to customize HTTP requests in .NET.This class allows
                //you to set various properties such as the HTTP method, request URI, headers, and content.
                HttpRequestMessage message = new HttpRequestMessage();
                
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);
                if (apiRequest.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json");
                }
                switch (apiRequest.ApiType)
                {
                    case SD.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;

                    case SD.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;

                    case SD.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;

                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                HttpResponseMessage apiResponse = null;

                /*
                    HttpClient.SendAsync is a method in C# used to send HTTP requests and receive HTTP responses asynchronously 
                */
                apiResponse = await client.SendAsync(message);

                //Reading the response content as a string
                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                //deserialize the content in object of type T, T is APIResponse
                var APIResponse = JsonConvert.DeserializeObject<T>(apiContent);
                return APIResponse;
            }
            catch (Exception ex)
            {
                var dto = new APIResponse
                {
                    ErrorMessage = new List<string> { Convert.ToString(ex.Message) },
                    IsSucces = false
                };

                var res = JsonConvert.SerializeObject(dto);
                var APIResponse = JsonConvert.DeserializeObject<T>(res);

                return APIResponse;
            }
        }
    }
}
