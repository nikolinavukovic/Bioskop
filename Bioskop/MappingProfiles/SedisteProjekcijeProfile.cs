using AutoMapper;
using Bioskop.Models;
using Bioskop.Models.Dtos;
using System.Collections.Generic;

namespace Bioskop.MappingProfiles
{
    public class SedisteProjekcijeProfile : Profile
    {
        public SedisteProjekcijeProfile()
        {
            CreateMap<SedisteProjekcije, SedisteProjekcije>();
            CreateMap<SedisteProjekcije, SedisteProjekcijeDto>();
            //CreateMap<List<SedisteProjekcije>, SedisteProjekcije>();
            //CreateMap<SedisteProjekcije, List<SedisteProjekcije>>();
        }
    }
}
