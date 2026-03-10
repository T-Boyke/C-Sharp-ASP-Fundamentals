using System;
using System.Collections.Generic;

namespace _10_Filmdatenbank.Domain.Entities
{
    public class Person
    {
        public int PersonID { get; set; }
        public string Vorname { get; set; } = string.Empty;
        public string Nachname { get; set; } = string.Empty;

        public ICollection<PersonEigenschaftFilm> PersonEigenschaftFilme { get; set; } = new List<PersonEigenschaftFilm>();
    }
}
