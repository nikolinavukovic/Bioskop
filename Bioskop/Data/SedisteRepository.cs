using AutoMapper;
using Bioskop.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bioskop.Data
{
    public class SedisteRepository : ISedisteRepository
    {
        private readonly DatabaseContext Context;
        private readonly IMapper Mapper;

        public SedisteRepository(DatabaseContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public List<Sediste> GetSedisteList()
        {
            return Context.Sediste.ToList();
        }

        public Sediste GetSedisteById(Guid sedisteId)
        {
            return Context.Sediste.FirstOrDefault(e => e.SedisteID == sedisteId);
        }

        public Sediste CreateSediste(Sediste sediste)
        {
            sediste.SedisteID = Guid.NewGuid();

            Context.Sediste.Add(sediste);
            Context.SaveChanges();

            return Mapper.Map<Sediste>(sediste);
        }

        public Sediste UpdateSediste(Sediste sediste)
        {
            Sediste s = Context.Sediste.FirstOrDefault(e => e.SedisteID == sediste.SedisteID);

            if (s == null)
                throw new EntryPointNotFoundException();

            s.SedisteID = sediste.SedisteID;
            s.BrojSedista = sediste.BrojSedista;
            s.BrojReda = sediste.BrojReda;

            Context.SaveChanges();

            return Mapper.Map<Sediste>(s);
        }

        public void DeleteSediste(Guid sedisteId)
        {
            Sediste s = Context.Sediste.FirstOrDefault(e => e.SedisteID == sedisteId);

            if (s == null)
                throw new EntryPointNotFoundException();

            Context.Sediste.Remove(s);
            Context.SaveChanges();
        }


        public bool SaveChanges()
        {
            return Context.SaveChanges() > 0;
        }

    }
}
