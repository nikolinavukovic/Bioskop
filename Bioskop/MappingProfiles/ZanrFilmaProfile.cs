using AutoMapper;
using Bioskop.Models;

namespace Bioskop.MappingProfiles
{
    public class ZanrFilmaProfile : Profile
    {
        public ZanrFilmaProfile()
        {
            CreateMap<ZanrFilma, ZanrFilma>();
        }
    }
}
