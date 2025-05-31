using EconomicStatistics.Models;
using EconomicStatistics.Services;
using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace EconomicStatistics.Views
{
    public partial class PopulationView : UserControl
    {
        private readonly PopulationService _service;

        public List<string> Years { get; set; }
        public ChartValues<long> PopulationValues { get; set; }
        public ChartValues<decimal> GrowthValues { get; set; }

        public PopulationView()
        {
            InitializeComponent();
            _service = new PopulationService();
            LoadData();
            DataContext = this;
        }

        private void LoadData()
        {
            var data = _service.GetData();
            DataGrid.ItemsSource = data;

            // Prepare data for charts
            Years = data.Select(d => d.Year.ToString()).ToList();
            PopulationValues = new ChartValues<long>(data.Select(d => d.TotalPopulation));
            GrowthValues = new ChartValues<decimal>(data.Select(d => d.GrowthPercent));

            // Get extremes
            var extremes = _service.GetExtremes();
            MaxGrowth.Text = $"{extremes.maxGrowth:N2}%";
            MinGrowth.Text = $"{extremes.minGrowth:N2}%";
        }
    }
}   