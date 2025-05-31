using EconomicStatistics.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace EconomicStatistics.Services
{
    public class GdpGnpService
    {
        private List<GdpGnpData> _data;

        public GdpGnpService()
        {
            LoadData();
            CalculateGrowthPercents();
        }

        private void LoadData()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "gdp_gnp.json");
            var json = File.ReadAllText(filePath);
            _data = JsonConvert.DeserializeObject<List<GdpGnpData>>(json);
        }

        private void CalculateGrowthPercents()
        {
            for (int i = 1; i < _data.Count; i++)
            {
                _data[i].GdpGrowthPercent = ((_data[i].Gdp - _data[i - 1].Gdp) / _data[i - 1].Gdp * 100;
                _data[i].GnpGrowthPercent = ((_data[i].Gnp - _data[i - 1].Gnp) / _data[i - 1].Gnp * 100;
            }
        }

        public List<GdpGnpData> GetData() => _data;

        public (decimal maxGdpGrowth, decimal minGdpGrowth, decimal maxGnpGrowth, decimal minGnpGrowth) GetExtremes()
        {
            var maxGdpGrowth = _data.Max(d => d.GdpGrowthPercent);
            var minGdpGrowth = _data.Min(d => d.GdpGrowthPercent);
            var maxGnpGrowth = _data.Max(d => d.GnpGrowthPercent);
            var minGnpGrowth = _data.Min(d => d.GnpGrowthPercent);

            return (maxGdpGrowth, minGdpGrowth, maxGnpGrowth, minGnpGrowth);
        }

        public List<GdpGnpData> Predict(int years, int windowSize)
        {
            var predictedData = new List<GdpGnpData>();
            var lastData = _data.Last();
            
            for (int i = 1; i <= years; i++)
            {
                var gdpWindow = _data.Skip(_data.Count - windowSize).Select(d => d.Gdp).ToList();
                var gnpWindow = _data.Skip(_data.Count - windowSize).Select(d => d.Gnp).ToList();
                
                var newYear = lastData.Year + i;
                var newGdp = gdpWindow.Average();
                var newGnp = gnpWindow.Average();
                
                predictedData.Add(new GdpGnpData
                {
                    Year = newYear,
                    Gdp = newGdp,
                    Gnp = newGnp,
                    GdpGrowthPercent = ((newGdp - lastData.Gdp) / lastData.Gdp) * 100,
                    GnpGrowthPercent = ((newGnp - lastData.Gnp) / lastData.Gnp) * 100
                });
                
                lastData = predictedData.Last();
            }
            
            return predictedData;
        }
    }
}