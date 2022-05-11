using AutoMapper;
using Bioskop.Models;

namespace Bioskop.MappingProfiles
{
    public class SedisteProjekcijeProfile : Profile
    {
        public SedisteProjekcijeProfile()
        {
            CreateMap<SedisteProjekcije, SedisteProjekcije>();
        }
    }
}
