using System;
using System.Collections.Generic;

namespace _10_Filmdatenbank.Domain.Entities
{
    public class Eigenschaft
    {
        public int EigenschaftID { get; set; }
        public string Bezeichnung { get; set; } = string.Empty;

        public ICollection<PersonEigenschaftFilm> PersonEigenschaftFilme { get; set; } = new List<PersonEigenschaftFilm>();
    }
}
