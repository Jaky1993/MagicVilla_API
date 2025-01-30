using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_Web.Models.DTO;
using MagicVilla_Web.services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers
{
    public class CustomerController : Controller
    {
        public readonly ICustomerService _customerService;
        public readonly IMapper _mapper;

        public CustomerController(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        public async Task<IActionResult> CustomerIndex()
        {
            List<CustomerDTO> customerList = new();

            APIResponse response = await _customerService.GetAllAsync<APIResponse>();

            if (response != null && response.IsSucces)
            {
                customerList = JsonConvert.DeserializeObject<List<CustomerDTO>>(Convert.ToString(response.Result));
            }

            return View(customerList);
        }
    }
}
