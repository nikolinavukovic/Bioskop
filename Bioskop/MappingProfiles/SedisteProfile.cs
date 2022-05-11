using AutoMapper;
using Bioskop.Models;

namespace Bioskop.MappingProfiles
{
    public class SedisteProfile : Profile
    {
        public SedisteProfile()
        {
            CreateMap<Sediste, Sediste>();
        }
    }
}
