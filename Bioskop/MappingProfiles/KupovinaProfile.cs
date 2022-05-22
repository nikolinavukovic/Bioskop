using AutoMapper;
using Bioskop.Models;
using Bioskop.Models.Dtos;

namespace Bioskop.MappingProfiles
{
    public class KupovinaProfile : Profile
    {
        public KupovinaProfile()
        {
            CreateMap<Kupovina, Kupovina>();
            CreateMap<Kupovina, KupovinaDto>();
        }
    }
}
