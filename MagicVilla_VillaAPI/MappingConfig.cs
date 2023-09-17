using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;

namespace MagicVilla_VillaAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            //CreateMap<VillaModel, VillaDTO>();
            //CreateMap<VillaDTO, VillaModel>();

            CreateMap<VillaModel, VillaDTO>().ReverseMap();
            CreateMap<VillaModel, VillaCreateDTO>().ReverseMap();
            CreateMap<VillaModel, VillaUpdateDTO>().ReverseMap();
        }
    }
}
