using Chaletin.Models.Enum;

namespace Chaletin.Models.Model
{
    public class FarmViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<string> Images { get; set; }
        public FarmType Type { get; set; }
        public City City { get; set; }
        public int LivingRoomCount { get; set; }
        public int BedRoomCount { get; set; }
        public int BathRoomCount { get; set; }
        public string Description { get; set; }
        public double Rate { get; set; }
        public int Price { get; set; }
        public string UserId { get; set; }
        public bool PayLater { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int TotalAmount { get; set; }
        public bool Booked { get; set; }
    }
}
