using Chaletin.Models.Enum;

namespace Chaletin.Models.Model
{
    public class FarmDtoModel
    {
        public string Title { get; set; }
        public string ImageSource { get; set; }
        public FarmType Type { get; set; }
        public City City { get; set; }
        public int LivingRoomCount { get; set; }
        public int BedRoomCount { get; set; }
        public int BathRoomCount { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
        public int Price { get; set; }
        public double Rate { get; set; }
        public string LivingRoomDescription { get; set; }
        public string SwimmingPoolDescription { get; set; }
        public string BedRoomDescription { get; set; }
        public string BathRoomDescription { get; set; }
        public string KitchenDescription { get; set; }
        public string PublicUtilityDescription { get; set; }
    }
}
