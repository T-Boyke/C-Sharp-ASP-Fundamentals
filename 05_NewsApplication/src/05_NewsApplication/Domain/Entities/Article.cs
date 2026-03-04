using System;
using _05_NewsApplication.Domain.ValueObjects;

namespace _05_NewsApplication.Domain.Entities {
    public class Article {
        public int Id { get; set; }
        public required Headline Headline { get; set; }
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int AuthorId { get; set; }
        public Author? Author { get; set; }
    }
}
