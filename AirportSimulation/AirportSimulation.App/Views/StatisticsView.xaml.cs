namespace AirportSimulation.App.Views
{
    using System.Windows.Controls;
    using System.Collections.ObjectModel;
    using De.TorstenMandelkow.MetroChart;
    using Core;
    using Core.Services;
    using System.Collections.Generic;
    using System;
    using AirportSimulation.Common.Models;

    /// <summary>
    /// Interaction logic for StatisticsView.xaml
    /// </summary>
    /// 
    public partial class StatisticsView : UserControl
    {
        public StatisticsData statisticsData = new StatisticsData();

        public StatisticsView()
        {
            InitializeComponent();

            ChartSeries series = new ChartSeries();
            series.DisplayMember = "Category";
            series.ValueMember = "Number";
            series.ItemsSource = null;
            pieChart.Series.Add(series);
            ObservableCollection<ChartData> myData = new ObservableCollection<ChartData>();
            series.ItemsSource = myData;

            myData.Add(new ChartData() { Category = "PSC Failed Bags", Number = 5});
            myData.Add(new ChartData() { Category = "PSC Successfull Bags", Number = 4 });
        }



        private object selectedItem = null;
        public object SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                // selected item has changed
                selectedItem = value;
            }
        }
    }



    // class which represent a data point in the chart
    public class ChartData
    {
        public string Category { get; set; }

        public int Number { get; set; }
    }
}

