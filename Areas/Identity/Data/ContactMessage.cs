namespace Chaletin.Areas.Identity.Data
{
    public class ContactMessage
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public User User { get; set; }
    }
}
