using AutoMapper;
using Bioskop.Models;

namespace Bioskop.MappingProfiles
{
    public class KorisnikProfile : Profile
    {
        public KorisnikProfile()
        {
            CreateMap<Korisnik, Korisnik>();
        }
    }
}
