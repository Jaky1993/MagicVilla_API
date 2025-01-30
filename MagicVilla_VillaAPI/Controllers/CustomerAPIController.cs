using AutoMapper;
using Azure;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;
using System;
using MagicVilla_VillaAPI.Models.DTO;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/Customer")]
    [ApiController]
    public class CustomerAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerAPIController(ICustomerRepository customerRepository, IMapper mapper)
        {
            this._customerRepository = customerRepository;
            this._mapper = mapper;
            this._response = new();
        }

        [HttpGet]

        //Task represent an asyncronus operation you're working with tasks to perform operations
        //without blocking the main thread. It allows your application to remain responsive.

        //ActionResult: This is a base class for action results in ASP.NET Core.
        //It can represent various HTTP responses like Ok, NotFound, BadRequest, etc.
        //T: This is a generic type parameter that represents the data being returned.
        //It can be any type, such as a model or a collection of models.

        public async Task<ActionResult<APIResponse>> GetCustomerList()
        {
            try
            {
                IEnumerable<Customer> customerList = await _customerRepository.GetAllAsync();
                _response.Result = _mapper.Map<List<CustomerDTO>>(customerList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSucces = false;
                _response.ErrorMessage = new List<string> { ex.Message };
            }

            return _response;
        }

        [HttpGet("customerId:int", Name = "GetCustomer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetCustomer(int customerId)
        {
            try
            {
                if (customerId == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                Customer customer = await _customerRepository.GetAsync(c => c.Id == customerId);

                if (customer == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<CustomerDTO>(customer);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSucces = false;
                _response.ErrorMessage = new List<string> { ex.Message };
            }

            return _response;
        }

        [HttpPost]
        //[FromBody] -> The CustomerDTO object in your CreateCustomer method will be populated with the values from the JSON
        //[FromBody] is an attribute in ASP.NET Core that tells the framework to bind the incoming request's body to the parameter
        //in your action method. In this case, it binds the JSON (or other format) request body to the customerDTO parameter.
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        public async Task<ActionResult<APIResponse>> CreateCustomer([FromBody]CustomerCreateDTO customerCreateDTO)
        {
            try
            {
                //Since GetAsync is an asynchronous method, it needs to be awaited to get the actual result.
                //Without awaiting it, you're comparing the task itself to null, not the result of the task.
                //Here's how you can properly check if the name already exists in the database

                if (await _customerRepository.GetAsync(c => c.Name.ToLower() == customerCreateDTO.Name.ToLower()) != null)
                {
                    /*
                        ModelState.AddModelError is a method used in ASP.NET Core to manually add an error to the model state.
                        This is useful when you want to provide custom validation error messages.
                    */
                    /*
                        ModelState is a property in ASP.NET Core that represents the state of model binding to a controller or a Razor page.
                        It's essentially a container that holds the results of model binding and validation. When data is submitted
                        to your application, ModelState is used to capture and hold all the incoming values, any errors that might occur during
                        the binding process, and the results of validation checks.
                        Here's what ModelState does in a nutshell:
                        Stores the values: It keeps track of the values submitted from a form or JSON request.
                        Records errors: It logs any errors that occur during model binding, such as data type
                        mismatches or missing required fields.
                        Performs validation: It runs any validation attributes you've applied to your model properties
                        and stores the results.
                    */
                    ModelState.AddModelError("CustomCustomerError1","Customer already exists");
                    return BadRequest(ModelState);
                }

                if (customerCreateDTO == null)
                {
                    return BadRequest(customerCreateDTO);
                }

                customerCreateDTO.CreateDate = DateTime.Now;

                Customer customer = _mapper.Map<Customer>(customerCreateDTO);

                await _customerRepository.CreateAsync(customer);

                _response.Result = _mapper.Map<CustomerDTO>(customer);
                _response.StatusCode = HttpStatusCode.Created;

                /*
                The CreatedAtRoute method is commonly used in ASP.NET Core to generate a response for a successful
                resource creation, typically a HTTP 201 status code. This method is particularly useful in RESTful
                APIs when you want to indicate that a new resource has been created and provide a link
                to the newly created resource.

                "GetCustomer": This is the name of the route that will be used to generate the URL for the newly created customer

                new { id = customer.Id }: This is an anonymous object used to supply route parameters.
                In this case, it provides the id of the newly created customer to the route.

                _response: This is the content that will be included in the body of the response. 
                _response typically contains information about the newly created resource, like the customer details.

                The CreatedAtRoute method will generate a response that includes a Location header with the URL to the
                GetCustomer method, using the newly created customer's ID. This allows the client to easily access the
                newly created customer resource.
                */
                return CreatedAtRoute("GetCustomer", new { id = customer.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSucces = false;
                _response.ErrorMessage = new List<string> { ex.Message };
            }

            return _response;
        }

        [HttpPut("customerId:int", Name = "UpdateCustomer")]
        public async Task<ActionResult<APIResponse>> UpdateCustomer(int customerId, [FromBody]CustomerUpdateDTO customerUpdateDTO)
        {
            try
            {
                if (customerUpdateDTO == null || customerId != customerUpdateDTO.Id)
                {
                    return BadRequest();
                }

                customerUpdateDTO.UpdateDate = DateTime.Now;

                Customer customer = _mapper.Map<Customer>(customerUpdateDTO);

                await _customerRepository.UpdateCustomer(customer);

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSucces = true;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSucces = false;
                _response.ErrorMessage = new List<string> { ex.Message };
            }

            return _response;
        }

        [HttpDelete("customerId:int", Name = "DeleteCustomer")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteCustomer(int customerId)
        {
            try
            {
                if (customerId == null)
                {
                    return BadRequest();
                }

                Customer customer = await _customerRepository.GetAsync(c => c.Id == customerId);

                if (customer == null)
                {
                    return NotFound();
                }

                await _customerRepository.RemoveAsync(customer);
                _response.IsSucces = true;
                _response.StatusCode = HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSucces = false;
                _response.ErrorMessage = new List<string> { ex.Message };
            }

            return _response;
        }
    }
}
