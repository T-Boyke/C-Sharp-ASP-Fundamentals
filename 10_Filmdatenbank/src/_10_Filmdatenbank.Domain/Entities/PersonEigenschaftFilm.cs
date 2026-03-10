using System;

namespace _10_Filmdatenbank.Domain.Entities
{
    public class PersonEigenschaftFilm
    {
        public int PEFID { get; set; }
        
        public int PersonID { get; set; }
        public Person Person { get; set; } = null!;

        public int FilmID { get; set; }
        public Film Film { get; set; } = null!;

        public int EigenschaftID { get; set; }
        public Eigenschaft Eigenschaft { get; set; } = null!;
    }
}
