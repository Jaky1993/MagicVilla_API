using MagicVilla_Utility;
using MagicVilla_VillaAPI.Models.DTO;
using MagicVilla_Web.Models;
using MagicVilla_Web.services.IServices;

namespace MagicVilla_Web.services
{
    public class CustomerService : BaseService, ICustomerService
    {
        public IHttpClientFactory _clientFactory { get; set; }
        public string customerUrl { get; set; }

        public CustomerService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            customerUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
        }

        public Task<T> CreateAsync<T>(CustomerCreateDTO customerCreateDTO)
        {
            APIRequest apiRequest = new APIRequest();
            
            apiRequest.ApiType = SD.ApiType.POST;
            apiRequest.Data = customerCreateDTO;
            apiRequest.Url = customerUrl + "/api/v1/Customer";

            //CreateAsync<T> method is returning the result of calling another asynchronous method, SendAsync<T>

            return SendAsync<T>(apiRequest);
        }

        public Task<T> DeleteAsync<T>(int customerId)
        {
            APIRequest apiRequest = new APIRequest();

            apiRequest.ApiType = SD.ApiType.DELETE;
            apiRequest.Url = customerUrl + "/api/v1/Customer" + customerId;

            return SendAsync<T>(apiRequest);
        }

        public Task<T> GetAllAsync<T>()
        {
            APIRequest apiRequest = new APIRequest();

            apiRequest.ApiType = SD.ApiType.GET;
            apiRequest.Url = customerUrl + "/api/v1/Customer";

            return SendAsync<T>(apiRequest);
        }

        public Task<T> GetAsync<T>(int customerId)
        {
            APIRequest apiRequest = new APIRequest();

            apiRequest.ApiType = SD.ApiType.GET;
            apiRequest.Url = customerUrl + "/api/v1/Customer" + customerId;

            return SendAsync<T>(apiRequest);
        }

        public Task<T> UpdateAsync<T>(CustomerUpdateDTO customerUpdateDTO)
        {
            APIRequest apiRequest = new APIRequest();

            apiRequest.ApiType = SD.ApiType.PUT;
            apiRequest.Data = customerUpdateDTO;
            apiRequest.Url = customerUrl + "/api/v1/Customer";

            return SendAsync<T>(apiRequest);
        }
    }
}
