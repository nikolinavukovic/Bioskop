using System.Diagnostics.CodeAnalysis;

namespace Bioskop.Models.Dtos
{
    public class SedisteProjekcijeDto
    {
        public ProjekcijaDto Projekcija { get; set; }

        public Sediste Sediste { get; set; }

        public float Cena { get; set; }

        public Kupovina Kupovina { get; set; }

    }
}
