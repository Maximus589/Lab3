using EconomicStatistics.Models;
using EconomicStatistics.Services;
using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace EconomicStatistics.Views
{
    public partial class GdpGnpView : UserControl
    {
        private readonly GdpGnpService _service;

        public SeriesCollection GdpGnpSeries { get; set; }
        public SeriesCollection GrowthSeries { get; set; }
        public List<string> Years { get; set; }
        public ChartValues<decimal> GdpValues { get; set; }
        public ChartValues<decimal> GnpValues { get; set; }
        public ChartValues<decimal> GdpGrowthValues { get; set; }
        public ChartValues<decimal> GnpGrowthValues { get; set; }

        public GdpGnpView()
        {
            InitializeComponent();
            _service = new GdpGnpService();
            LoadData();
            DataContext = this;
        }

        private void LoadData()
        {
            var data = _service.GetData();
            DataGrid.ItemsSource = data;

            // Prepare data for charts
            Years = data.Select(d => d.Year.ToString()).ToList();
            GdpValues = new ChartValues<decimal>(data.Select(d => d.Gdp));
            GnpValues = new ChartValues<decimal>(data.Select(d => d.Gnp));
            GdpGrowthValues = new ChartValues<decimal>(data.Select(d => d.GdpGrowthPercent));
            GnpGrowthValues = new ChartValues<decimal>(data.Select(d => d.GnpGrowthPercent));

            // Get extremes
            var extremes = _service.GetExtremes();
            MaxGdpGrowth.Text = $"{extremes.maxGdpGrowth:N2}%";
            MinGdpGrowth.Text = $"{extremes.minGdpGrowth:N2}%";
            MaxGnpGrowth.Text = $"{extremes.maxGnpGrowth:N2}%";
            MinGnpGrowth.Text = $"{extremes.minGnpGrowth:N2}%";
        }
    }
}