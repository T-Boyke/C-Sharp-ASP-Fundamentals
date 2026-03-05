namespace NewsApplication2.Models {
    public class Author {
        public int Id { get; set; }
        public required string Firstname { get; set; }
        public required string Lastname { get; set; }
        public virtual ICollection<Article> Articles { get; set; } = [];

        public string Fullname => $"{Firstname} {Lastname}".Trim();
    }
}
