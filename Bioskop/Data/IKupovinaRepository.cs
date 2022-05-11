using Bioskop.Models;
using System;
using System.Collections.Generic;

namespace Bioskop.Data
{
    public interface IKupovinaRepository
    {
        List<Kupovina> GetKupovinaList(bool placeno);
        Kupovina GetKupovinaById(Guid kupovinaId);
        Kupovina CreateKupovina(Kupovina kupovina);
        Kupovina UpdateKupovina(Kupovina kupovina);
        void DeleteKupovina(Guid kupovinaId);
        bool SaveChanges();
    }
}
