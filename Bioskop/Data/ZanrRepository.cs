using AutoMapper;
using Bioskop.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bioskop.Data
{
    public class ZanrRepository : IZanrRepository
    {

        private readonly DatabaseContext Context;
        private readonly IMapper Mapper;

        public ZanrRepository(DatabaseContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public List<Zanr> GetZanrList(string naziv = default)
        {
            return Context.Zanr.Where(e => (naziv == default || e.Naziv.Equals(naziv))).ToList();
        }

        public Zanr GetZanrById(Guid zanrId)
        {
            return Context.Zanr.FirstOrDefault(e => e.ZanrID == zanrId);
        }

        public Zanr CreateZanr(Zanr zanr)
        {
            zanr.ZanrID = Guid.NewGuid();

            Context.Zanr.Add(zanr);
            Context.SaveChanges();

            return Mapper.Map<Zanr>(zanr);
        }

        public Zanr UpdateZanr(Zanr zanr)
        {
            Zanr z = Context.Zanr.FirstOrDefault(e => e.ZanrID == zanr.ZanrID);

            if (z == null)
                throw new EntryPointNotFoundException();

            z.ZanrID = zanr.ZanrID;
            z.Naziv = zanr.Naziv;

            Context.SaveChanges();

            return Mapper.Map<Zanr>(z);
        }

        public void DeleteZanr(Guid zanrId)
        {
            Zanr z = Context.Zanr.FirstOrDefault(e => e.ZanrID == zanrId);

            if (z == null)
                throw new EntryPointNotFoundException();

            Context.Zanr.Remove(z);
            Context.SaveChanges();
        }


        public bool SaveChanges()
        {
            return Context.SaveChanges() > 0;
        }

    }
}
