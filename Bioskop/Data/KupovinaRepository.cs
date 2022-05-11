using AutoMapper;
using Bioskop.Models;
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

        public List<Kupovina> GetKupovinaList(bool placeno = default)
        {
            return Context.Kupovina.Where(e => (placeno == default || e.Placeno.Equals(placeno))).ToList();
        }

        public Kupovina GetKupovinaById(Guid kupovinaId)
        {
            return Context.Kupovina.FirstOrDefault(e => e.KupovinaID == kupovinaId);
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
