
namespace Chaletin.Areas.Identity.Data
{
    public class Farm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Type { get; set; }
        public int City { get; set; }
        public int LivingRoomCount { get; set; }
        public int BedRoomCount { get; set; }
        public int BathRoomCount { get; set; }
        public string Description { get; set; }
        public double Rate { get; set; }
        public int Price { get; set; }
        public string UserId { get; set; }
        public int Capacity { get; set; }
        public string LivingRoomDescription { get; set; }
        public string SwimmingPoolDescription { get; set; }
        public string BedRoomDescription { get; set; }
        public string BathRoomDescription { get; set; }
        public string KitchenDescription { get; set; }
        public string  PublicUtilityDescription { get; set; }
        public User User { get; set; }
        public List<Comments> Comments { get; set; }
    }
}
