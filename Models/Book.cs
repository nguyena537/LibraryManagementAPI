using System.ComponentModel.DataAnnotations;

namespace LibraryManagementAPI.Models
{
    public class Book
    {
        [Key] public int BookId { get; set; }
        [Required] public string Title { get; set; } = string.Empty;
        public string AuthorFirstName { get; set; } = string.Empty;
        [Required] public string AuthorLastName { get; set; } = string.Empty;
        [Required] public string ISBN { get; set; } = string.Empty;
        public int PublicationYear { get; set; }
        public string Publisher { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public int numAvailable { get; set; } = 0;
        public List<Member>? borrowers { get; set; } = new List<Member>();
    }
}
