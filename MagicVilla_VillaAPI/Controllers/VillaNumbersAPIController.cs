using AutoMapper;
using AutoMapper.Features;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/v{version:apiVersion}VillaNumbersAPI")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    //[ApiVersion("2.0")] -> ritorna errore perché in program.cs ho messo di default la versione 1.0
    public class VillaNumbersAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IMapper _mapper;
        private readonly IVillaRepository _dbVilla;
        private readonly IVillaNumberRepository _dbVillaNumber;

        public VillaNumbersAPIController(IMapper mapper, IVillaNumberRepository dbVillaNumber, IVillaRepository dbVilla)
        {
            _mapper = mapper;
            _dbVillaNumber = dbVillaNumber;
            _dbVilla = dbVilla;
            this._response = new();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillaNumbers()
        {
            try
            {
                //IEnumerable<VillaNumber>, it means you have a collection of VillaNumber objects that you can enumerate through.
                IEnumerable<VillaNumber> villaNumbersList = await _dbVillaNumber.GetAllAsync(includeProperties: "Villa");
                _response.Result = _mapper.Map<List<VillaNumberDTO>>(villaNumbersList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSucces = false;
                _response.ErrorMessage = new List<string> { ex.ToString() };
            }

            return _response;
        }

        [MapToApiVersion("2.0")]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("villaNo:int", Name = "GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int villaNo)
        {
            try
            {
                if (villaNo == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var villaNumber = await _dbVillaNumber.GetAsync(V => V.VillaNo == villaNo);

                _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
                _response.IsSucces = true;
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSucces = false;
                _response.ErrorMessage = new List<string> { ex.ToString() };
            }

            return _response;
        }

        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)] //ritorna un villa model vuoto
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost]

        //API Endpoint that return APIResponse
        public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO villaNumberCreateDTO)
        {
            try
            {
                //v => v.VillaNo == villaNumberCreateDTO.VillaNo) != null -> LINQ expression.
                //It's a lambda expression that checks if any villa in your collection
                //has a VillaNo matching the one in villaNumberCreateDTO.VillaNo

                //Expression<Func<T, TResult>> is a powerful feature in C# that allows you to construct expressions
                //that can be compiled into executable code or inspected for analysis.
                //These expressions are particularly useful in scenarios like building dynamic queries or LINQ

                if (await _dbVillaNumber.GetAsync(v => v.VillaNo == villaNumberCreateDTO.VillaNo) != null)
                {
                    ModelState.AddModelError("ErrorMessage", "VillaNumber already exists!");
                    return BadRequest(ModelState);
                }

                if(await _dbVilla.GetAsync(v => v.Id==villaNumberCreateDTO.VillaID) == null)
                {
                    ModelState.AddModelError("ErrorMessage", "VillaID is invalid");
                    return BadRequest(ModelState);
                }

                if (villaNumberCreateDTO == null)
                {
                    return BadRequest(villaNumberCreateDTO);
                }

                VillaNumber villaNumber = _mapper.Map<VillaNumber>(villaNumberCreateDTO);

                await _dbVillaNumber.CreateAsync(villaNumber);

                _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetVillaNumber", new { id = villaNumber.VillaNo }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSucces = false;
                _response.ErrorMessage = new List<string> { ex.ToString() };
            }

            return _response;
        }

        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("villaNo:int", Name = "DeleteVillaNumber")]
        public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int villaNo)
        {
            try
            {
                if (villaNo == 0)
                {
                    return BadRequest();
                }

                var villaNumber = await _dbVillaNumber.GetAsync(v => v.VillaNo == villaNo);

                if (villaNumber == null)
                {
                    return NotFound();
                }


                await _dbVillaNumber.RemoveAsync(villaNumber);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSucces = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSucces = false;
                _response.ErrorMessage = new List<string> { ex.ToString() };
            }

            return _response;
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("villaNo:int", Name = "UpdateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> UpdateVilla(int villaNo, [FromBody] VillaNumberUpdateDTO villaNumberUpdateDTO)
        {
            try
            {
                if (villaNumberUpdateDTO == null || villaNo != villaNumberUpdateDTO.VillaNo)
                {
                    return BadRequest();
                }

                if (await _dbVilla.GetAsync(v => v.Id == villaNumberUpdateDTO.VillaID) == null)
                {
                    ModelState.AddModelError("ErrorMessage", "VillaID is invalid");
                    return BadRequest(ModelState);
                }

                VillaNumber villaNumber = _mapper.Map<VillaNumber>(villaNumberUpdateDTO);

                await _dbVillaNumber.UpdateAsync(villaNumber);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSucces = true;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSucces = false;
                _response.ErrorMessage = new List<string> { ex.ToString() };
            }

            return _response;
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("villaNo:int", Name = "UpdateVillaNumberPartial")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateVillaPartial(int villaNo, JsonPatchDocument<VillaNumberUpdateDTO> villaNumberPatchDTO)
        {
            if (villaNumberPatchDTO == null || villaNo == 0)
            {
                return BadRequest();
            }

            var villaNumber = await _dbVillaNumber.GetAsync(v => v.VillaNo == villaNo, tracked: false);

            VillaNumberUpdateDTO patchVillaNumberUpdateDTO = _mapper.Map<VillaNumberUpdateDTO>(villaNumber);

            if (villaNumber == null)
            {
                return BadRequest();
            }

            villaNumberPatchDTO.ApplyTo(patchVillaNumberUpdateDTO, ModelState);

            VillaNumber model = _mapper.Map<VillaNumber>(patchVillaNumberUpdateDTO);

            await _dbVillaNumber.UpdateAsync(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return NoContent();

        }
    }
}
