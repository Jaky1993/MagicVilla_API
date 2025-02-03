using Azure;
using Azure.Core;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.services.IServices;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Security.AccessControl;
using System.Security.Cryptography.Xml;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

                //Prima di tutto, viene creato un'istanza di HttpRequestMessage e configurato per eseguire una richiesta HTTP.
                HttpResponseMessage apiResponse = null;

                //Con questa configurazione, il client HTTP aggiungerà il token di autorizzazione alle richieste HTTP,
                //consentendo l'accesso alle risorse protette
                if (!string.IsNullOrEmpty(apiRequest.Token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.Token);
                }

                /*
                    HttpClient.SendAsync is a method in C# used to send HTTP requests and receive HTTP responses asynchronously 
                */
                //La richiesta viene inviata utilizzando SendAsync e la risposta viene letta come stringa.
                apiResponse = await client.SendAsync(message);

                //Reading the response content as a string -> leggo la risposta di SendAsync come stringa
                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                //deserialize the content in object of type T, T is APIResponse

                //Il blocco try-catch interno viene utilizzato per gestire eventuali errori che si verificano durante
                //la deserializzazione della risposta API in un oggetto di tipo APIResponse
                try
                {
                    //Se si verifica un'eccezione durante la deserializzazione (JsonConvert.DeserializeObject<APIResponse>(apiContent))
                    APIResponse response = JsonConvert.DeserializeObject<APIResponse>(apiContent);

                    if (response == null && apiResponse.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        response = new APIResponse();

                        response.StatusCode = HttpStatusCode.Unauthorized;
                        response.IsSucces = false;

                        var jsonResponse = JsonConvert.SerializeObject(response);
                        var returnObj = JsonConvert.DeserializeObject<T>(jsonResponse);

                        return returnObj;
                    }

                    //Se si verificano eventuali errori in apiResponse (BadRequest e NotFound)
                    if (response != null && (apiResponse.StatusCode == HttpStatusCode.BadRequest || apiResponse.StatusCode == HttpStatusCode.NotFound))
                    {
                        response.StatusCode = HttpStatusCode.BadRequest;
                        response.IsSucces = false;
                        
                        var jsonResponse = JsonConvert.SerializeObject(response);
                        var returnObj = JsonConvert.DeserializeObject<T>(jsonResponse);

                        return returnObj;
                    }
                }
                catch (Exception ex)
                {
                    /*
                    Se si verifica un'eccezione durante la deserializzazione (JsonConvert.DeserializeObject<APIResponse>(apiContent)),
                    l'eccezione viene catturata. L'eccezione catturata (ex) rappresenta l'errore che si è verificato.
                    La stringa di contenuto della risposta (apiContent) viene deserializzata in un oggetto del tipo T
                    e viene restituito l'oggetto deserializzato. 
                    */
                    var exceptionResponse = JsonConvert.DeserializeObject<T>(apiContent);
                    return exceptionResponse;
                }

                //Se non ci sono errori né eccezioni durante la deserializzazione, il contenuto della risposta API (apiContent)
                //viene deserializzato in un oggetto di tipo T e restituito come risultato finale della funzione.
                //L'idea alla base di questa logica è quella di gestire specifici errori di risposta e,
                //in caso di eccezioni generali, restituire comunque un oggetto deserializzato. Tuttavia,
                //deserializzare l'oggetto due volte può sembrare inefficiente
                var apiResponseContent = JsonConvert.DeserializeObject<T>(apiContent);

                return apiResponseContent;
            }
            catch (Exception ex)
            {
                var dto = new APIResponse
                {
                    ErrorMessage = new List<string> { Convert.ToString( ex.Message) },
                    IsSucces = false
                };

                var res = JsonConvert.SerializeObject(dto);
                var APIResponse = JsonConvert.DeserializeObject<T>(res);

                return APIResponse;
            }
        }
    }
}
