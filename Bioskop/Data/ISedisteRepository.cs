using Bioskop.Models;
using System;
using System.Collections.Generic;

namespace Bioskop.Data
{
    public interface ISedisteRepository
    {
        List<Sediste> GetSedisteList();
        Sediste GetSedisteById(Guid sedisteId);
        Sediste CreateSediste(Sediste sediste);
        Sediste UpdateSediste(Sediste sediste);
        void DeleteSediste(Guid sedisteId);
        bool SaveChanges();
    }
}
