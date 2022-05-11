using AutoMapper;
using Bioskop.Models;

namespace Bioskop.MappingProfiles
{
    public class ZanrProfile : Profile
    {
        public ZanrProfile()
        {
            CreateMap<Zanr, Zanr>();
        }
    }
}
