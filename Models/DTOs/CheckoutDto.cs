namespace LibraryManagementAPI.Models.DTOs
{
    public class CheckoutDto
    {
        public int CheckoutId { get; set; }
        public int MemberId { get; set; }
        public int BookId { get; set; }
        public DateTime CheckedOutOn { get; set; }
        public DateTime CheckedOutUntil { get; set; }
    }
}
