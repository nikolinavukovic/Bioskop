using Bioskop.Models;
using System;
using System.Collections.Generic;

namespace Bioskop.Data
{
    public interface IZanrFilmaRepository
    {
        List<ZanrFilma> GetZanrFilmaList();
        ZanrFilma GetZanrFilmaById(Guid zanrId, Guid filmId);
        ZanrFilma CreateZanrFilma(ZanrFilma zanrFilma);
        ZanrFilma UpdateZanrFilma(ZanrFilma zanrFilma);
        void DeleteZanrFilma(Guid zanrId, Guid filmId);
        bool SaveChanges();
    }
}
