using System;

namespace Bioskop.Models.Dtos
{
    public class ProjekcijaDto
    {
        public Guid ProjekcijaID { get; set; }

        public DateTime Vreme { get; set; }

        public int BrojStampanihKarata { get; set; }

        public Film Film { get; set; }
    }
}
