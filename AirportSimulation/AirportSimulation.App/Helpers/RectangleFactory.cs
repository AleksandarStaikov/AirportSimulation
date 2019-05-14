namespace AirportSimulation.App.Helpers
{
    using System.Windows.Shapes;
    using System.Windows.Media;
    using Utility;
    using System.Windows.Media.Animation;
    using System.Windows;
    using System;
    using System.Windows.Controls;
    using System.Linq;
    using System.Collections.Generic;

    internal static class RectangleFactory
    {
        public static List<Rectangle> GetDisabledRectangles(this Grid grid)
        {
            return grid
                .Children
                .OfType<Rectangle>()
                .Where(x => x.Uid == Constants.DISABLED_RECTANGLE_UID)
                .ToList();
        }

        public static Rectangle CreateBlinkingRectangle(int width = 50, int height = 50)
        {
            var blinkingRectangle = CreateRectangle(width, height, true);
            blinkingRectangle.Opacity = 0.5;
            blinkingRectangle.Uid = Constants.BLINKING_RECTANGLE_UID;
            blinkingRectangle.Fill = new SolidColorBrush(Colors.White);

            var animation = new ColorAnimation
            {
                From = Colors.White,
                To = Colors.ForestGreen,
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever,
                Duration = new Duration(TimeSpan.FromSeconds(1.5))
            };

            blinkingRectangle.Fill.BeginAnimation(SolidColorBrush.ColorProperty, animation);

            return blinkingRectangle;
        }

        public static Rectangle CreateBuildingComponentRectangle(ImageSource imageSource, int width = 50, int height = 50)
        {
            var buildingComponentRectangle = CreateRectangle(width, height, true);
            buildingComponentRectangle.Fill = new ImageBrush
            {
                Stretch = Stretch.Fill,
                ImageSource = imageSource
            };

            Grid.SetZIndex(buildingComponentRectangle, 1000);

            return buildingComponentRectangle;
        }

        public static Rectangle CreateDisabledRectangle(int width = 50, int height = 50)
        {
            var disabledRectangle = CreateRectangle(width, height, true);
            disabledRectangle.Fill = new SolidColorBrush(Colors.LightSlateGray);
            disabledRectangle.Uid = Constants.DISABLED_RECTANGLE_UID;

            return disabledRectangle;
        }

        public static void RemoveBlinkingRectangles(Grid grid, List<(int, int)> externalList)
        {
            var blinkingRectangles = grid
                    .Children
                    .OfType<Rectangle>()
                    .Where(r => r.Uid == Constants.BLINKING_RECTANGLE_UID)
                    .ToList();

            foreach (var rectangle in blinkingRectangles)
            {
                grid.Children.Remove(rectangle);
            }

			externalList.Clear();
		}

        private static Rectangle CreateRectangle(int width, int height, bool isEnabled)
        {
            return new Rectangle
            {
                Width = width,
                Height = height,
                IsEnabled = isEnabled
            };
        }
    }
}
