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
        public int Price { get; set; }
        public int Rate { get; set; }
    }
}
