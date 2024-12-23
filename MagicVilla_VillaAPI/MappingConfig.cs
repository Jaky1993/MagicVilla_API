using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;

namespace MagicVilla_VillaAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Villa, VillaDTO>().ReverseMap(); //Crete MAP from Villa to VillaDTO
            CreateMap<VillaDTO, Villa>(); //Create MAP from VillaDTO to Villa

            CreateMap<Villa, VillaCreateDTO>().ReverseMap(); //The .ReverseMap() method creates mappings in both directions,
            //meaning it can map from Villa to VillaCreateDTO and from VillaCreateDTO to Villa.
            //.ReverseMap(): Automatically creates the reverse mapping from VillaCreateDTO to Villa.

            CreateMap<Villa, VillaUpdateDTO>().ReverseMap();

            //Reverse Map map from destination to the source
        }
       
    }
}
