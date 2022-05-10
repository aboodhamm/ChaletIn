namespace Chaletin.Models.Model
{
    public class AdminStatistics
    {
        public double TotalPayedAmount { get; set; } 
        public double TotalBookedFarms { get; set; }
        public double TotalAvailableFarms { get; set; }
        public double TotalNotAvailableFarms { get; set; }
        public int TotalFarms { get; set; }
    }
}
