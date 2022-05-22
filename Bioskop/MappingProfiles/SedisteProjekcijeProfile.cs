using AutoMapper;
using Bioskop.Models;
using Bioskop.Models.Dtos;

namespace Bioskop.MappingProfiles
{
    public class SedisteProjekcijeProfile : Profile
    {
        public SedisteProjekcijeProfile()
        {
            CreateMap<SedisteProjekcije, SedisteProjekcije>();
            CreateMap<SedisteProjekcije, SedisteProjekcijeDto>();
        }
    }
}
