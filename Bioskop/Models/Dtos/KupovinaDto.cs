using System;

namespace Bioskop.Models.Dtos
{
    public class KupovinaDto
    {
        public Guid KupovinaID { get; set; }

        public float UkupanIznos { get; set; }

        public bool Placeno { get; set; }

        public DateTime VremeRezervacije { get; set; }

        public DateTime VremePlacanja { get; set; }

        public Korisnik Korisnik { get; set; }


    }
}
