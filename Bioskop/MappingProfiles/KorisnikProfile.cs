using AutoMapper;
using Bioskop.Models;
using Bioskop.Models.Dtos;
using Bioskop.Wrappers;
using System.Collections.Generic;

namespace Bioskop.MappingProfiles
{
    public class KorisnikProfile : Profile
    {
        public KorisnikProfile()
        {
            CreateMap<Korisnik, Korisnik>();
            CreateMap<Korisnik, KorisnikDto>();
        }
    }
}
