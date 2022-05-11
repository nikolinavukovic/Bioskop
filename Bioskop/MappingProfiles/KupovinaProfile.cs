using AutoMapper;
using Bioskop.Models;

namespace Bioskop.MappingProfiles
{
    public class KupovinaProfile : Profile
    {
        public KupovinaProfile()
        {
            CreateMap<Kupovina, Kupovina>();
        }
    }
}
