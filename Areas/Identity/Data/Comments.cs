namespace Chaletin.Areas.Identity.Data
{
    public class Comments
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public int FarmId { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Farm Farm { get; set; }
        public User User { get; set; }
    }
}
