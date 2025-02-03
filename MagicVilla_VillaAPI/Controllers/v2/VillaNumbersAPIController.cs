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

namespace MagicVilla_VillaAPI.Controllers.v2
{
    [Route("api/v{version:apiVersion}/VillaNumbersAPI")]
    [ApiController]
    [ApiVersion("2.0")]
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
            _response = new();
        }

        [HttpGet("GetString")]
        public IEnumerable<string> Get()
        {
            return new string[] { "Jacopo", "Grazioli" };
        }
    }
}
