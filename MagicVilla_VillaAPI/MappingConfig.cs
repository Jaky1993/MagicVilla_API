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

            CreateMap<Villa, VillaCreateDTO>().ReverseMap();

            CreateMap<Villa, VillaUpdateDTO>().ReverseMap();

            //Reverse Map map from destination to the source

            CreateMap<VillaNumber, VillaNumberDTO>(); //Create mapping from VillaNumber to VillaNumberDTO
            CreateMap<VillaNumberDTO, VillaNumber>();

            CreateMap<VillaNumber, VillaNumberUpdateDTO>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberCreateDTO>().ReverseMap();

            //ReverseMap(): Automatically creates the reverse mapping from VillaCreateDTO to VillaNumber

            //CreateMap<Customer, CustomerDTO>(); //FROM customer to CustomerDTO -> source to destination
            CreateMap<Customer, CustomerDTO>().ReverseMap(); //FROM destination to the source or reverse, two way mapping

            CreateMap<CustomerCreateDTO, Customer>().ReverseMap();
            CreateMap<CustomerUpdateDTO, Customer>().ReverseMap();

            CreateMap<ApplicationUser, UserDTO>().ReverseMap();
        }
       
    }
}
