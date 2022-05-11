using AutoMapper;
using Bioskop.Models;

namespace Bioskop.MappingProfiles
{
    public class TipKorisnikaProfile : Profile
    {
        public TipKorisnikaProfile()
        {
            CreateMap<TipKorisnika, TipKorisnika>();
        }
    }
}
