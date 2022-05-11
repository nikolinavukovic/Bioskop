using Bioskop.Models;
using System;
using System.Collections.Generic;

namespace Bioskop.Data
{
    public interface IZanrRepository
    {
        List<Zanr> GetZanrList(string naziv);
        Zanr GetZanrById(Guid zanrId);
        Zanr CreateZanr(Zanr zanr);
        Zanr UpdateZanr(Zanr zanr);
        void DeleteZanr(Guid zanrId);
        bool SaveChanges();
    }
}
