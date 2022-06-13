using AutoMapper;
using Bioskop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bioskop.Data
{
    public class ZanrFilmaRepository : IZanrFilmaRepository
    {
        private readonly DatabaseContext Context;
        private readonly IMapper Mapper;

        public ZanrFilmaRepository(DatabaseContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public List<ZanrFilma> GetZanrFilmaList()
        {
            return Context.ZanrFilma.Include(zf => zf.Film).Include(zf => zf.Zanr).ToList();
        }

        public ZanrFilma GetZanrFilmaById(Guid filmId, Guid zanrId)
        {
            return Context.ZanrFilma.Include(zf => zf.Film).Include(zf => zf.Zanr).FirstOrDefault(e => e.ZanrID == zanrId &&
                                                            e.FilmID == filmId);
        }

        public ZanrFilma CreateZanrFilma(ZanrFilma zanrFilma)
        {

            Context.ZanrFilma.Add(zanrFilma);
            Context.SaveChanges();

            return Mapper.Map<ZanrFilma>(zanrFilma);
        }

        public ZanrFilma UpdateZanrFilma(ZanrFilma zanrFilma)
        {
            ZanrFilma zf = Context.ZanrFilma.FirstOrDefault(e => e.ZanrID == zanrFilma.ZanrID &&
                                                                    e.FilmID == zanrFilma.FilmID);

            if (zf == null)
                throw new EntryPointNotFoundException();


            zf.ZanrID = zanrFilma.ZanrID;
            zf.FilmID = zanrFilma.FilmID;

            Context.SaveChanges();

            return Mapper.Map<ZanrFilma>(zf);
        }

        public void DeleteZanrFilma(Guid filmId, Guid zanrId)
        {
            ZanrFilma zf = Context.ZanrFilma.FirstOrDefault(e => e.ZanrID == zanrId &&
                                                            e.FilmID == filmId);
            if (zf == null)
                throw new EntryPointNotFoundException();

            Context.ZanrFilma.Remove(zf);
            Context.SaveChanges();
        }


        public bool SaveChanges()
        {
            return Context.SaveChanges() > 0;
        }

    }
}
