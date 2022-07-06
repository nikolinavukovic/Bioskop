using AutoMapper;
using Bioskop.Models;
using System.Collections.Generic;

namespace Bioskop.MappingProfiles
{
    public class TransakcijaProfile : Profile
    {
        public TransakcijaProfile()
        {
            CreateMap<List<string>, string>();
        }

    }
}
