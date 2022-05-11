using AutoMapper;
using Bioskop.Models;

namespace Bioskop.MappingProfiles
{
    public class FilmProfile : Profile
    {
        public FilmProfile()
        {
            CreateMap<Film, Film>();
        }
    }
}
