﻿namespace AirportSimulation.App.Views
{
    using System.Windows.Controls;
    using System.Collections.ObjectModel;
    using De.TorstenMandelkow.MetroChart;
    using Core.Services;
    using Helpers;

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

            this.DataContext = new StatisticsViewModel();

            //ChartSeries series = new ChartSeries();
            //series.DisplayMember = "Category";
            //series.ValueMember = "Number";
            //series.ItemsSource = null;
            //pieChart.Series.Add(series);

            //ObservableCollection<ChartData> myData = new ObservableCollection<ChartData>();
            //series.ItemsSource = myData;

            //myData.Add(new ChartData() { Category = "PSC Failed Bags", Number = 6 });
            //myData.Add(new ChartData() { Category = "PSC Successfull Bags", Number = 4 });



            //ChartSeries series1 = new ChartSeries();
            //series1.DisplayMember = "Category";
            //series1.ValueMember = "Number";
            //series1.ItemsSource = null;
            //columnChart.Series.Add(series1);

            //ObservableCollection<ChartData> myData1 = new ObservableCollection<ChartData>();
            //series1.ItemsSource = myData1;

            //myData1.Add(new ChartData() { Category = "test", Number = 6 });

            //myData1.Add(new ChartData() { Category = "Ptest", Number = 4 });

            //myData1.Add(new ChartData() { Category = "test12", Number = 11});

            //myData1.Add(new ChartData() { Category = "Ptesttest", Number = 22});

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



    //// class which represent a data point in the chart
    //public class ChartData
    //{
    //    public string Category { get; set; }

    //    public int Number { get; set; }

    //    public double Percentage { get; set; }
    //}

    //public class BarChartData
    //{

    //    public string Category { get; set; }

    //    public int Number { get; set; }

    //    public double Percentage { get; set; }

    //}
}

