using AutoMapper;
using Bioskop.Models;
using Bioskop.Models.Dtos;

namespace Bioskop.MappingProfiles
{
    public class ProjekcijaProfile : Profile
    {
        public ProjekcijaProfile()
        {
            CreateMap<Projekcija, Projekcija>();
            CreateMap<Projekcija, ProjekcijaDto>();
        }
    }
}
