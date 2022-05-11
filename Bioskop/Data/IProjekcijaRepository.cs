using Bioskop.Models;
using System;
using System.Collections.Generic;

namespace Bioskop.Data
{
    public interface IProjekcijaRepository
    {
        List<Projekcija> GetProjekcijaList();
        Projekcija GetProjekcijaById(Guid projekcijaId);
        Projekcija CreateProjekcija(Projekcija projekcija);
        Projekcija UpdateProjekcija(Projekcija projekcija);
        void DeleteProjekcija(Guid projekcijaId);
        bool SaveChanges();
    }
}
