namespace AirportSimulation.App.Helpers
{
    using AirportSimulation.Common.Models;
    using AirportSimulation.Utility;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public static class GridHelper
    {
        internal struct GridDefinitionInformation
        {
            public GridLength GridLength;
            public string SharedSizeGroup;
        }

        public static int CalculateIndexFromCoordinates((int row, int col) cell)
        {
            return cell.row * (SimulationGridOptions.GRID_MAX_COLUMNS) + cell.col;
        }

        public static bool IsGridCellUsedOrDisabled(this Grid grid, (int x, int y) cell)
            => grid.IsGridCellUsed(cell) || grid.IsGridCellDisabled(cell);

        public static bool IsGridCellUsed(this Grid grid, (int x, int y) cell)
            => grid.GetGridCellOnCondition(cell, x => x.Uid != Constants.BLINKING_RECTANGLE_UID) != null;

        public static bool CanPlaceBlinkingRectangle(this Grid grid, (int x, int y) cell)
            => grid.GetGridCellOnCondition(cell, x => x.Uid == Constants.COMPONENT_RECTANGLE_UID) == null;

        public static bool IsGridCellDisabled(this Grid grid, (int x, int y) cell)
            => grid.GetGridCellOnCondition(cell, x => x.Uid == Constants.DISABLED_RECTANGLE_UID) != null;

        public static bool CanDisable(this Grid grid, (int x, int y) cell)
            => grid.GetGridCellOnCondition(cell, 
                    x => x.Uid == Constants.COMPONENT_RECTANGLE_UID || 
                        x.Uid == Constants.BLINKING_RECTANGLE_UID) == null;   

        public static void RemoveDisabledGridCellsIfPossible(this Grid grid)
        {
            var uIElements = grid.GetGridUIElements()
                .Where(x => x.Uid != Constants.DISABLED_RECTANGLE_UID)
                .ToList();

            foreach (var element in uIElements)
            {
                var x = Grid.GetRow(element);
                var y = Grid.GetColumn(element);

                if (IsGridCellDisabled(grid, (x, y)))
                {
                    grid.Children.Remove(uIElements.GetUIElementInCell((x, y)));
                }
            }
        }

        private static UIElement GetGridCellOnCondition(this Grid grid, (int x, int y) cell, Func<UIElement, bool> condition = null)
        {
            if (condition == null)
            {
                return grid.GetGridUIElements().GetUIElementInCell(cell);
            }

            return grid.GetGridUIElements()
                .Where(condition)
                .GetUIElementInCell(cell);
        }

        public static UIElement GetUIElementInCell(this IEnumerable<UIElement> uIElements, (int x, int y) cell) 
            => uIElements
                .FirstOrDefault(x =>
                        Grid.GetRow(x) == cell.x &&
                        Grid.GetColumn(x) == cell.y);

        private static IEnumerable<UIElement> GetGridUIElements(this Grid grid) => grid.Children.Cast<UIElement>();

        public static (int, int) GetCurrentlySelectedGridCell(Grid grid, MouseButtonEventArgs e)
        {
            var selectedColumnIndex = -1;
            var selectedRowIndex = -1;

            var pos = e.GetPosition(grid);
            var temp = pos.X;

            for (var i = 0; i < grid.ColumnDefinitions.Count; i++)
            {
                var colDef = grid.ColumnDefinitions[i];
                temp -= colDef.ActualWidth;

                if (temp <= -1)
                {
                    selectedColumnIndex = i;
                    break;
                }
            }

            temp = pos.Y;

            for (var i = 0; i < grid.RowDefinitions.Count; i++)
            {
                var rowDef = grid.RowDefinitions[i];
                temp -= rowDef.ActualHeight;

                if (temp <= -1)
                {
                    selectedRowIndex = i;
                    break;
                }
            }

            return (selectedRowIndex, selectedColumnIndex);
        }

        // External implementations below

        #region Helper methods

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
            {
                return GridLength.Auto;
            }

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

        private static string CalculateSharedSize(string sharedSizeGroup, string sharedSizeGroupPrefix)
        {
            if (sharedSizeGroup != null && sharedSizeGroupPrefix != null)
            {
                return sharedSizeGroupPrefix + sharedSizeGroup;
            }

            return sharedSizeGroup;
        }

        #endregion

        #region GridColumnsLayout

        public static string GetColumns(DependencyObject obj) => (string)obj.GetValue(ColumnsProperty);

        public static void SetColumns(DependencyObject obj, string value) => obj.SetValue(ColumnsProperty, value);

        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.RegisterAttached("Columns", typeof(string), typeof(GridHelper),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure, Columns_PropertyChangedCallback));

        private static void Columns_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = d as Grid;
            var oldValue = e.OldValue as string;
            var newValue = e.NewValue as string;
            if (grid == null || oldValue == null || newValue == null)
            {
                return;
            }

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

        private static void Rows_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = d as Grid;
            var oldValue = e.OldValue as string;
            var newValue = e.NewValue as string;
            if (grid == null || oldValue == null || newValue == null)
            {
                return;
            }

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

        public static string GetCell(DependencyObject obj) => (string)obj.GetValue(CellProperty);

        public static void SetCell(DependencyObject obj, string value) => obj.SetValue(CellProperty, value);

        public static readonly DependencyProperty CellProperty =
            DependencyProperty.RegisterAttached("Cell", typeof(string), typeof(GridHelper),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure, Cell_PropertyChangedCallback));

        private static void Cell_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as UIElement;
            var oldValue = e.OldValue as string;
            var newValue = e.NewValue as string;
            if (element == null || oldValue == null || newValue == null)
            {
                return;
            }

            if (oldValue != newValue)
            {
                var rowAndColumn = newValue.Split(new[] { ' ', ';' });

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

        public static string GetSharedSizeGroupPrefix(DependencyObject obj) => (string)obj.GetValue(SharedSizeGroupPrefixProperty);

        public static void SetSharedSizeGroupPrefix(DependencyObject obj, string value) => obj.SetValue(SharedSizeGroupPrefixProperty, value);

        public static readonly DependencyProperty SharedSizeGroupPrefixProperty =
            DependencyProperty.RegisterAttached("SharedSizeGroupPrefix", typeof(string), typeof(GridHelper), new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));

        #endregion
    }
}

