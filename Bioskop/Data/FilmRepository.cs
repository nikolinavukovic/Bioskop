using AutoMapper;
using Bioskop.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bioskop.Data
{
    public class FilmRepository : IFilmRepository
    {

        private readonly DatabaseContext Context;
        private readonly IMapper Mapper;

        public FilmRepository(DatabaseContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }
        public List<Film> GetFilmList(int godina = default)
        {
            return Context.Film.Where(e => (godina == default || e.Godina.Equals(godina))).ToList();
        }

        public Film GetFilmById(Guid filmId)
        {
            return Context.Film.FirstOrDefault(e => e.FilmID == filmId);
        }

        public Film CreateFilm(Film film)
        {
            film.FilmID = Guid.NewGuid();

            Context.Film.Add(film);
            Context.SaveChanges();

            return Mapper.Map<Film>(film);
        }

        public Film UpdateFilm(Film film)
        {
            Film f = Context.Film.FirstOrDefault(e => e.FilmID == film.FilmID);

            if (f == null)
                throw new EntryPointNotFoundException();

            f.FilmID = film.FilmID;
            f.Naziv = film.Naziv;
            f.Trajanje = film.Trajanje;
            f.Reziser = film.Reziser;
            f.OriginalniNaziv = film.OriginalniNaziv;
            f.Opis = film.Opis;
            f.Drzava = film.Drzava;
            f.Godina = film.Godina;


            Context.SaveChanges();

            return Mapper.Map<Film>(f);
        }

        public void DeleteFilm(Guid filmId)
        {
            Film f = Context.Film.FirstOrDefault(e => e.FilmID == filmId);

            if (f == null)
                throw new EntryPointNotFoundException();

            Context.Film.Remove(f);
            Context.SaveChanges();
        }

        public bool SaveChanges()
        {
            return Context.SaveChanges() > 0;
        }

    }
}
