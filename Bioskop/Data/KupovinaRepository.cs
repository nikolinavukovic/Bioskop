using AutoMapper;
using Bioskop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bioskop.Data
{
    public class KupovinaRepository : IKupovinaRepository
    {
        private readonly DatabaseContext Context;
        private readonly IMapper Mapper;

        public KupovinaRepository(DatabaseContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public List<Kupovina> GetKupovinaList(string placeno = default, string korisnickoIme = default)
        {
            //Omoguceno izlistavanje svih kupovina za samo jednog korisnika, kao i svih sedista kupljenih pri toj kupovini
            if (placeno != default && korisnickoIme != default)
            {
                
                return Context.Kupovina.Where(a => (a.Placeno.Equals(Convert.ToBoolean(placeno)) &&
                        (korisnickoIme == default || a.Korisnik.KorisnickoIme.Equals(korisnickoIme)))).Include(r => r.SedistaProjekcije).ToList();
            }

            else if (placeno != default  && korisnickoIme == default)
            {
                return Context.Kupovina.Where(a => (a.Placeno.Equals(Convert.ToBoolean(placeno)))).Include(r => r.SedistaProjekcije)
                                                                                                   .ToList();
            }
            else if (placeno == default && korisnickoIme != default)
            {              
                return Context.Kupovina.Where(a => 
                        (korisnickoIme == default || a.Korisnik.KorisnickoIme.Equals(korisnickoIme))).Include(r => r.SedistaProjekcije)
                                                                                                      .ToList();
            }
            else
            {
                return Context.Kupovina.Include(r => r.SedistaProjekcije).ToList();
            }

        }

        public Kupovina GetKupovinaById(Guid kupovinaId)
        {
            return Context.Kupovina.Include(r => r.Korisnik).FirstOrDefault(e => e.KupovinaID == kupovinaId);
        }

        public Kupovina CreateKupovina(Kupovina kupovina)
        {
            kupovina.KupovinaID = Guid.NewGuid();


            //Podesavanje vremena rezervacije i placanja prilikom create
            kupovina.VremeRezervacije = DateTime.Now;

            if (kupovina.Placeno == true)
            {
                kupovina.VremePlacanja = DateTime.Now;
            }


            Context.Kupovina.Add(kupovina);
            Context.SaveChanges();

            return Mapper.Map<Kupovina>(kupovina);
        }

        public Kupovina UpdateKupovina(Kupovina kupovina)
        {
            Kupovina k = Context.Kupovina.FirstOrDefault(e => e.KupovinaID == kupovina.KupovinaID);

            if (k == null)
                throw new EntryPointNotFoundException();

            if (kupovina.Placeno = true && (kupovina.Placeno != k.Placeno))
            {
                k.VremePlacanja = DateTime.UtcNow;
            }

            Context.SaveChanges();

            return Mapper.Map<Kupovina>(k);
        }

        public void DeleteKupovina(Guid kupovinaId)
        {
            Kupovina k = Context.Kupovina.FirstOrDefault(e => e.KupovinaID == kupovinaId);

            if (k == null)
                throw new EntryPointNotFoundException();

            Context.Kupovina.Remove(k);
            Context.SaveChanges();
        }


        public bool SaveChanges()
        {
            return Context.SaveChanges() > 0;
        }

    }
}
