using Bioskop.Models;
using System;
using System.Collections.Generic;

namespace Bioskop.Data
{
    public interface ISedisteProjekcijeRepository
    {
        List<SedisteProjekcije> GetSedisteProjekcijeList();
        SedisteProjekcije GetSedisteProjekcijeById(Guid sedisteId, Guid projekcijaId);
        SedisteProjekcije CreateSedisteProjekcije(SedisteProjekcije sedisteProjekcije);
        SedisteProjekcije UpdateSedisteProjekcije(SedisteProjekcije sedisteProjekcije);
        void DeleteSedisteProjekcije(Guid sedisteId, Guid projekcijaId);
        bool SaveChanges();
    }
}
