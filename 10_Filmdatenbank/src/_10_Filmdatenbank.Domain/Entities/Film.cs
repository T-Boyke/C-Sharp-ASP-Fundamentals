using System;
using System.Collections.Generic;

namespace _10_Filmdatenbank.Domain.Entities
{
    public class Film
    {
        public int FilmID { get; set; }
        public string Titel { get; set; } = string.Empty;
        public int Erscheinungsjahr { get; set; }
        public int Spieldauer { get; set; }
        public decimal Preis { get; set; }

        public ICollection<PersonEigenschaftFilm> PersonEigenschaftFilme { get; set; } = new List<PersonEigenschaftFilm>();
    }
}
