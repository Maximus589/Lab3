namespace EconomicStatistics.Models
{
    public class GdpGnpData
    {
        public int Year { get; set; }
        public decimal Gdp { get; set; } // В миллиардах рублей
        public decimal Gnp { get; set; } // В миллиардах рублей
        public decimal GdpGrowthPercent { get; set; }
        public decimal GnpGrowthPercent { get; set; }
    }
}