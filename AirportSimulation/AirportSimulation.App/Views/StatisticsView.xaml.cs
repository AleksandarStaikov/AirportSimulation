namespace AirportSimulation.App.Views
{
    using System.Windows.Controls;
    using System.Collections.ObjectModel;
    using De.TorstenMandelkow.MetroChart;
    using Core.Services;
    using Helpers;
    using System.Windows.Threading;
    using System;

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

