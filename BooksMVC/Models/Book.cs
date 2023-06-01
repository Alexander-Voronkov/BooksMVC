using System.ComponentModel.DataAnnotations;

namespace BooksMVC.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public byte[]? Cover { get; set; }
        public string? Style { get; set; }
        public DateTime? PublishDate { get; set; }
        public ICollection<Author> Authors { get; set; } = new List<Author>();

    }
}
