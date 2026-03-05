using System.ComponentModel.DataAnnotations;

namespace NewsApplication2.Models {
    public class Article {
        public int Id { get; set; }
        [MaxLength(100, ErrorMessage = "Nur 100 Zeichen erlaubt.")]
        public required string Headline { get; set; }
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        [Display(Name = "The author")]
        public int AuthorId { get; set; }
        public Author? Author { get; set; }

        public string? ContentPreview => string.Concat(Content
            .Take(100)
            .Reverse()
            .SkipWhile(c => c != ' ')
            .Reverse()) + "...";
            //Content.Length < 100 ? Content : Content.Substring(0, 100);
    }
}
