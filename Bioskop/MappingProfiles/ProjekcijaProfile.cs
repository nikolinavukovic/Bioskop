using AutoMapper;
using Bioskop.Models;

namespace Bioskop.MappingProfiles
{
    public class ProjekcijaProfile : Profile
    {
        public ProjekcijaProfile()
        {
            CreateMap<Projekcija, Projekcija>();
        }
    }
}
