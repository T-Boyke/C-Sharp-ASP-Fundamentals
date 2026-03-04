using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using _05_NewsApplication.Domain.Entities;
using _05_NewsApplication.Domain.ValueObjects;

namespace _05_NewsApplication.Models {
    public class HomeIndexViewModel {
        public IEnumerable<Article> Articles { get; set; } = new List<Article>();
    }

    public class ArticleCreateViewModel {
        // Validation handled partly by ValueObject or data annotations
        public string Headline { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int AuthorId { get; set; }
        public SelectList? AuthorsList { get; set; }
    }
}
