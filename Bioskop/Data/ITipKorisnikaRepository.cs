using Bioskop.Models;
using System;
using System.Collections.Generic;

namespace Bioskop.Data
{
    public interface ITipKorisnikaRepository
    {
        List<TipKorisnika> GetTipKorisnikaList();
        TipKorisnika GetTipKorisnikaById(Guid tipKorisnikaId);
        TipKorisnika CreateTipKorisnika(TipKorisnika tipKorisnika);
        TipKorisnika UpdateTipKorisnika(TipKorisnika tipKorisnika);
        void DeleteTipKorisnika(Guid tipKorisnikaId);
        bool SaveChanges();

    }
}
