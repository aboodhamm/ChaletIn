using Chaletin.Models.Enum;

namespace Chaletin.Models.Model
{
    public class BookingViewModel
    {
        public int Id { get; set; }
        public string FarmTitle { get; set; }
        public string ImageSource { get; set; }
        public FarmType Type { get; set; }
        public City City { get; set; }
        public int LivingRoomCount { get; set; }
        public int BedRoomCount { get; set; }
        public int BathRoomCount { get; set; }
        public string Description { get; set; }
        public int Rate { get; set; }
        public int Price { get; set; }
        public string UserId { get; set; }
        public bool PayLater { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int TotalAmount { get; set; }
    }
}
