namespace AirportSimulation.App.Views
{
    using Helpers;
    using Resources;
    using Common;
    using Common.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using System.Windows.Media.Imaging;
    using Core;
    using Core.Contracts;
    using Utility;

    /// <summary>
    /// Interaction logic for StatisticsView.xaml
    /// </summary>
    /// 
    public partial class StatisticsView : UserControl
    {
        public ObservableCollection<TestClass> Errors { get; private set; }

        public StatisticsView()
        {
            InitializeComponent();

            Errors = new ObservableCollection<TestClass>();
            Errors.Add(new TestClass() { Category = "Globalization", Number = 75 });
            Errors.Add(new TestClass() { Category = "Features", Number = 2 });
            Errors.Add(new TestClass() { Category = "ContentTypes", Number = 12 });
            Errors.Add(new TestClass() { Category = "Correctness", Number = 83 });
            Errors.Add(new TestClass() { Category = "Best Practices", Number = 29 });
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
    public class TestClass
    {
        public string Category { get; set; }

        public int Number { get; set; }
    }
}

