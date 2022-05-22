using AutoMapper;
using Bioskop.Models;
using Bioskop.Models.Dtos;

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
