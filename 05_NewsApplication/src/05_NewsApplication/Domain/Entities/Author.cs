using System.Collections.Generic;

namespace _05_NewsApplication.Domain.Entities {
    public class Author {
        public int Id { get; set; }
        public required string Firstname { get; set; }
        public required string Lastname { get; set; }
        
        // Use a backing field to handle nulls gracefully if needed, or stick to auto properties
        public virtual ICollection<Article> Articles { get; set; } = new List<Article>();
        
        public string Fullname => $"{Firstname} {Lastname}".Trim();
    }
}
