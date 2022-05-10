using Chaletin.Areas.Identity.Data;
using Chaletin.Models.Enum;

namespace Chaletin.Models.Model
{
    public class FarmDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageSource { get; set; }
        public FarmType Type { get; set; }
        public City City { get; set; }
        public int LivingRoomCount { get; set; }
        public int BedRoomCount { get; set; }
        public int BathRoomCount { get; set; }
        public string Description { get; set; }
        public double Rate { get; set; }
        public double Price { get; set; }
        public string UserId { get; set; }
        public bool PayLater { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int TotalAmount { get; set; }
        public List<Comments> Comments { get; set; }
        public string LivingRoomDescription { get; set; }
        public string SwimmingPoolDescription { get; set; }
        public string BedRoomDescription { get; set; }
        public string BathRoomDescription { get; set; }
        public string KitchenDescription { get; set; }
        public string PublicUtilityDescription { get; set; }
        public bool Booked { get; set; }
    }
}
