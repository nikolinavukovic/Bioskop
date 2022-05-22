using AutoMapper;
using Bioskop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bioskop.Data
{
    public class ProjekcijaRepository : IProjekcijaRepository
    {
        private readonly DatabaseContext Context;
        private readonly IMapper Mapper;

        public ProjekcijaRepository(DatabaseContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public List<Projekcija> GetProjekcijaList()
        {
            return Context.Projekcija.Include(p => p.Film).ToList();
        }

        public Projekcija GetProjekcijaById(Guid projekcijaId)
        {
            return Context.Projekcija.Include(p => p.Film).FirstOrDefault(e => e.ProjekcijaID == projekcijaId);
        }

        public Projekcija CreateProjekcija(Projekcija projekcija)
        {
            projekcija.ProjekcijaID = Guid.NewGuid();

            Context.Projekcija.Add(projekcija);
            Context.SaveChanges();

            return Mapper.Map<Projekcija>(projekcija);
        }

        public Projekcija UpdateProjekcija(Projekcija projekcija)
        {
            Projekcija p = Context.Projekcija.FirstOrDefault(e => e.ProjekcijaID == projekcija.ProjekcijaID);

            if (p == null)
                throw new EntryPointNotFoundException();

            /*            p.ProjekcijaID = projekcija.ProjekcijaID;
                        p.BrojSlobodnihSedista = projekcija.BrojSlobodnihSedista;
                        p.Vreme = projekcija.Vreme;
                        p.FilmID = projekcija.FilmID;*/

            Context.SaveChanges();

            return Mapper.Map<Projekcija>(p);
        }

        public void DeleteProjekcija(Guid projekcijaId)
        {
            Projekcija p = Context.Projekcija.FirstOrDefault(e => e.ProjekcijaID == projekcijaId);

            if (p == null)
                throw new EntryPointNotFoundException();

            Context.Projekcija.Remove(p);
            Context.SaveChanges();
        }


        public bool SaveChanges()
        {
            return Context.SaveChanges() > 0;
        }

    }
}

