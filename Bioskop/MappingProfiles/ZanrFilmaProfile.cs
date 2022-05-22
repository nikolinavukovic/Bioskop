using AutoMapper;
using Bioskop.Models;
using Bioskop.Models.Dtos;

namespace Bioskop.MappingProfiles
{
    public class ZanrFilmaProfile : Profile
    {
        public ZanrFilmaProfile()
        {
            CreateMap<ZanrFilma, ZanrFilma>();
            CreateMap<ZanrFilma, ZanrFilmaDto>();
        }
    }
}
