using Bioskop.Models;
using System;
using System.Collections.Generic;

namespace Bioskop.Data
{
    public interface IZanrFilmaRepository
    {
        List<ZanrFilma> GetZanrFilmaList();
        ZanrFilma GetZanrFilmaById(Guid filmId, Guid zanrId);
        ZanrFilma CreateZanrFilma(ZanrFilma zanrFilma);
        ZanrFilma UpdateZanrFilma(ZanrFilma zanrFilma);
        void DeleteZanrFilma(Guid filmId, Guid zanrId);
        bool SaveChanges();
    }
}
