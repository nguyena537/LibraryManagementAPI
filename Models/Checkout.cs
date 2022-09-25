using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementAPI.Models
{
    public class Checkout
    {
        [Key] public int CheckoutId { get; set; }
        public int MemberId { get; set; }
        public int BookId { get; set; }
        public DateTime CheckedOutOn { get; set; }
        public DateTime CheckedOutUntil { get; set; }
    }
}
