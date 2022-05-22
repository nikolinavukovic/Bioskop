using AutoMapper;
using Bioskop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bioskop.Data
{
    public class SedisteProjekcijeRepository : ISedisteProjekcijeRepository
    {
        private readonly DatabaseContext Context;
        private readonly IMapper Mapper;

        public SedisteProjekcijeRepository(DatabaseContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public List<SedisteProjekcije> GetSedisteProjekcijeList()
        {
            return Context.SedisteProjekcije.Include(sp => sp.Sediste).Include(sp => sp.Projekcija).Include(sp => sp.Kupovina)
                                                                                                                    .ToList();
        }

        public SedisteProjekcije GetSedisteProjekcijeById(Guid sedisteId, Guid projekcijaId)
        {
            return Context.SedisteProjekcije.Include(sp => sp.Sediste).Include(sp => sp.Projekcija).Include(sp => sp.Kupovina)
                                                            .FirstOrDefault(e => e.SedisteID == sedisteId &&
                                                            e.ProjekcijaID == projekcijaId);
        }

        public SedisteProjekcije CreateSedisteProjekcije(SedisteProjekcije sedisteProjekcije)
        {
            Context.SedisteProjekcije.Add(sedisteProjekcije);
            Context.SaveChanges();

            return Mapper.Map<SedisteProjekcije>(sedisteProjekcije);
        }

        public SedisteProjekcije UpdateSedisteProjekcije(SedisteProjekcije sedisteProjekcije)
        {
            SedisteProjekcije sp = Context.SedisteProjekcije.FirstOrDefault(e => e.SedisteID == sedisteProjekcije.SedisteID &&
                                                                    e.ProjekcijaID == sedisteProjekcije.ProjekcijaID);

            if (sp == null)
                throw new EntryPointNotFoundException();


            sp.SedisteID = sedisteProjekcije.SedisteID;
            sp.ProjekcijaID = sedisteProjekcije.ProjekcijaID;
            sp.Cena = sedisteProjekcije.Cena;
            sp.KupovinaID = sedisteProjekcije.KupovinaID;

            Context.SaveChanges();

            return Mapper.Map<SedisteProjekcije>(sp);
        }

        public void DeleteSedisteProjekcije(Guid sedisteId, Guid projekcijaId)
        {
            SedisteProjekcije zf = Context.SedisteProjekcije.FirstOrDefault(e => e.SedisteID == sedisteId &&
                                                            e.ProjekcijaID == projekcijaId);
            if (zf == null)
                throw new EntryPointNotFoundException();

            Context.SedisteProjekcije.Remove(zf);
            Context.SaveChanges();
        }


        public bool SaveChanges()
        {
            return Context.SaveChanges() > 0;
        }

    }
}