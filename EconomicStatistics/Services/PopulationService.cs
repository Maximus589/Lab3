using EconomicStatistics.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace EconomicStatistics.Services
{
    public class PopulationService
    {
        private List<PopulationData> _data;

        public PopulationService()
        {
            LoadData();
            CalculateGrowthPercents();
        }

        private void LoadData()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "population.json");
            var json = File.ReadAllText(filePath);
            _data = JsonConvert.DeserializeObject<List<PopulationData>>(json);
        }

        private void CalculateGrowthPercents()
        {
            for (int i = 1; i < _data.Count; i++)
            {
                _data[i].GrowthPercent = ((decimal)(_data[i].TotalPopulation - _data[i - 1].TotalPopulation) / _data[i - 1].TotalPopulation) * 100;
            }
        }

        public List<PopulationData> GetData() => _data;

        public (decimal maxGrowth, decimal minGrowth) GetExtremes()
        {
            var maxGrowth = _data.Max(d => d.GrowthPercent);
            var minGrowth = _data.Min(d => d.GrowthPercent);
            return (maxGrowth, minGrowth);
        }

        public List<PopulationData> Predict(int years, int windowSize)
        {
            var predictedData = new List<PopulationData>();
            var lastData = _data.Last();
            
            for (int i = 1; i <= years; i++)
            {
                var window = _data.Skip(_data.Count - windowSize).Select(d => (decimal)d.TotalPopulation).ToList();
                var newPopulation = (long)window.Average();
                
                predictedData.Add(new PopulationData
                {
                    Year = lastData.Year + i,
                    TotalPopulation = newPopulation,
                    GrowthPercent = ((decimal)(newPopulation - lastData.TotalPopulation) / lastData.TotalPopulation) * 100
                });
                
                lastData = predictedData.Last();
            }
            
            return predictedData;
        }
    }
}