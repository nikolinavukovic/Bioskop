﻿using Bioskop.Models;
using System;
using System.Collections.Generic;

namespace Bioskop.Data
{
    public interface IFilmRepository
    {
        List<Film> GetFilmList(string naziv);
        Film GetFilmById(Guid filmId);
        Film CreateFilm(Film film);
        Film UpdateFilm(Film film);
        void DeleteFilm(Guid filmId);
        bool SaveChanges();
    }
}
