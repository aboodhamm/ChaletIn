namespace Chaletin.Areas.Identity.Data
{
    public class Booking
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public int TotalAmount { get; set; }
        public int Period { get; set; }
        public string UserId { get; set; }
        public int FarmId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public bool Payed { get; set; }
        public bool Disabled { get; set; }
        public User User { get; set; }
        public Farm Farm { get; set; }
    }
}
