using AutoMapper;
using Bioskop.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bioskop.Data
{
    public class TipKorisnikaRepository : ITipKorisnikaRepository
    {
        private readonly DatabaseContext Context;
        private readonly IMapper Mapper;

        public TipKorisnikaRepository(DatabaseContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }
        public List<TipKorisnika> GetTipKorisnikaList()
        {
            return Context.TipKorisnika.ToList();
        }

        public TipKorisnika GetTipKorisnikaById(Guid tipKorisnikaId)
        {
            return Context.TipKorisnika.FirstOrDefault(e => e.TipKorisnikaID == tipKorisnikaId);
        }

        public TipKorisnika CreateTipKorisnika(TipKorisnika tipKorisnika)
        {
            tipKorisnika.TipKorisnikaID = Guid.NewGuid();

            Context.TipKorisnika.Add(tipKorisnika);
            Context.SaveChanges();

            return Mapper.Map<TipKorisnika>(tipKorisnika);

        }

        public TipKorisnika UpdateTipKorisnika(TipKorisnika tipKorisnika)
        {
            TipKorisnika tk = Context.TipKorisnika.FirstOrDefault(e => e.TipKorisnikaID == tipKorisnika.TipKorisnikaID);

            if (tk == null)
            {
                throw new EntryPointNotFoundException();
            }

            tk.TipKorisnikaID = tipKorisnika.TipKorisnikaID;
            tk.Naziv = tipKorisnika.Naziv;

            Context.SaveChanges();

            return Mapper.Map<TipKorisnika>(tk);
        }

        public void DeleteTipKorisnika(Guid tipKorisnikaId)
        {
            TipKorisnika tipKorisnika = GetTipKorisnikaById(tipKorisnikaId);

            if (tipKorisnika == null)
            {
                throw new ArgumentNullException(nameof(tipKorisnikaId));
            }

            Context.TipKorisnika.Remove(tipKorisnika);
            Context.SaveChanges();

            //obrisan return sa mapiranjem
        }

        public bool SaveChanges()
        {
            return Context.SaveChanges() > 0;
        }
    }
}
