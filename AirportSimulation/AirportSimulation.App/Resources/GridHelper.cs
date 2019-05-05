using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AirportSimulation.App.Resources
{
    public class GridHelper
    {
        internal struct GridDefinitionInformation
        {
            public GridLength GridLength;
            public string SharedSizeGroup;
        }

        #region helper methods

        internal static IEnumerable<GridDefinitionInformation> Parse(string text)
        {
            if (text.Contains("#"))
            {
                var parts = text.Split(new[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
                var count = int.Parse(parts[1].Trim());
                return Enumerable.Repeat(ParseGridDefinition(parts[0]), count);
            }
            else
            {
                return new[] { ParseGridDefinition(text) };
            }
        }

        internal static GridLength ParseGridLength(string text)
        {
            text = text.Trim();
            if (text.ToLower() == "auto")
                return GridLength.Auto;
            if (text.Contains("*"))
            {
                var startCount = text.ToCharArray().Count(c => c == '*');
                var pureNumber = text.Replace("*", "");
                var ratio = string.IsNullOrWhiteSpace(pureNumber) ? 1 : double.Parse(pureNumber);
                return new GridLength(startCount * ratio, GridUnitType.Star);
            }
            var pixelsCount = double.Parse(text);
            return new GridLength(pixelsCount, GridUnitType.Pixel);
        }

        internal static GridDefinitionInformation ParseGridDefinition(string text)
        {
            text = text.Trim();
            var result = new GridDefinitionInformation();
            if (text.StartsWith("[") && text.EndsWith("]"))
            {
                result.SharedSizeGroup = text.Substring(1, text.Length - 2); // inside the []s
                result.GridLength = GridLength.Auto;
            }
            else
            {
                result.GridLength = ParseGridLength(text);
            }

            return result;
        }

        static string CalculateSharedSize(string sharedSizeGroup, string sharedSizeGroupPrefix)
        {
            if (sharedSizeGroup != null && sharedSizeGroupPrefix != null)
            {
                return sharedSizeGroupPrefix + sharedSizeGroup;
            }
            return sharedSizeGroup;
        }

        #endregion

        #region GridColumnsLayout

        public static string GetColumns(DependencyObject obj)
        {
            return (string)obj.GetValue(ColumnsProperty);
        }
        public static void SetColumns(DependencyObject obj, string value)
        {
            obj.SetValue(ColumnsProperty, value);
        }

        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.RegisterAttached("Columns", typeof(string), typeof(GridHelper),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure, Columns_PropertyChangedCallback));

        static void Columns_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = d as Grid;
            var oldValue = e.OldValue as string;
            var newValue = e.NewValue as string;
            if (grid == null || oldValue == null || newValue == null)
                return;

            var prefix = GetSharedSizeGroupPrefix(grid);

            if (oldValue != newValue)
            {
                grid.ColumnDefinitions.Clear();
                newValue
                    .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .SelectMany(Parse)
                    .Select(gridDefInfo => new ColumnDefinition
                    {
                        Width = gridDefInfo.GridLength,
                        SharedSizeGroup = CalculateSharedSize(gridDefInfo.SharedSizeGroup, prefix)
                    })
                    .ToList().ForEach(grid.ColumnDefinitions.Add);
            }
        }

        #endregion

        #region Rows

        public static string GetRows(DependencyObject obj)
        {
            return (string)obj.GetValue(RowsProperty);
        }
        public static void SetRows(DependencyObject obj, string value)
        {
            obj.SetValue(RowsProperty, value);
        }

        public static readonly DependencyProperty RowsProperty =
            DependencyProperty.RegisterAttached("Rows", typeof(string), typeof(GridHelper),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure, Rows_PropertyChangedCallback));

        static void Rows_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = d as Grid;
            var oldValue = e.OldValue as string;
            var newValue = e.NewValue as string;
            if (grid == null || oldValue == null || newValue == null)
                return;

            var prefix = GetSharedSizeGroupPrefix(grid);

            if (oldValue != newValue)
            {
                grid.RowDefinitions.Clear();
                newValue
                    .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .SelectMany(Parse)
                    .Select(gridDefInfo => new RowDefinition
                    {
                        Height = gridDefInfo.GridLength,
                        SharedSizeGroup = CalculateSharedSize(gridDefInfo.SharedSizeGroup, prefix)
                    })
                    .ToList().ForEach(grid.RowDefinitions.Add);
            }
        }

        #endregion

        #region Cell

        public static string GetCell(DependencyObject obj)
        {
            return (string)obj.GetValue(CellProperty);
        }
        public static void SetCell(DependencyObject obj, string value)
        {
            obj.SetValue(CellProperty, value);
        }

        public static readonly DependencyProperty CellProperty =
            DependencyProperty.RegisterAttached("Cell", typeof(string), typeof(GridHelper),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure, Cell_PropertyChangedCallback));

        static void Cell_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as UIElement;
            var oldValue = e.OldValue as string;
            var newValue = e.NewValue as string;
            if (element == null || oldValue == null || newValue == null)
                return;

            if (oldValue != newValue)
            {
                var rowAndColumn = newValue.Split(new[] { ' ', ';' });
                // only set row and/or column if they are specified
                // "1" or "1;" for only row, ";1" for column only
                if (!string.IsNullOrEmpty(rowAndColumn[0]))
                {
                    var row = int.Parse(rowAndColumn[0]);
                    Grid.SetRow(element, row);
                }

                if (!string.IsNullOrEmpty(rowAndColumn[1]))
                {
                    var column = int.Parse(rowAndColumn[1]);
                    Grid.SetColumn(element, column);
                }
            }
        }
        #endregion

        #region SharedSizeGroupPrefix

        public static string GetSharedSizeGroupPrefix(DependencyObject obj)
        {
            return (string)obj.GetValue(SharedSizeGroupPrefixProperty);
        }

        public static void SetSharedSizeGroupPrefix(DependencyObject obj, string value)
        {
            obj.SetValue(SharedSizeGroupPrefixProperty, value);
        }

        public static readonly DependencyProperty SharedSizeGroupPrefixProperty =
            DependencyProperty.RegisterAttached("SharedSizeGroupPrefix", typeof(string), typeof(GridHelper), new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));

        #endregion
    }
}

