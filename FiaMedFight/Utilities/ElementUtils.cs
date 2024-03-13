using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;
using Windows.Gaming.UI;
using Windows.UI.Xaml.Controls;

namespace FiaMedFight.Utilities
{
    /// <summary>
    /// Contains methods for UIelements and storyboards.
    /// </summary>
    internal static class ElementUtils
    {
        /// <summary>
        /// Retrieves the row and column indices of a specified element by its name within the game board.
        /// </summary>
        /// <param name="grid">The name of the child element within the game board grid.</param>
        /// <param name="childElementName">The name of the child element within the game board grid.</param>
        /// <returns>A tuple containing the row and column indices of the element.</returns>
        public static (int, int) GetElementRowAndColumn(Grid grid, string childElementName)
        {
            var targetElement = grid.FindName(childElementName) as FrameworkElement;
            int newColumn = Grid.GetColumn(targetElement);
            int newRow = Grid.GetRow(targetElement);
            return (newRow, newColumn);
        }
        /// <summary>
        /// Applies a DoubleAnimation to an Element or Transform instance, changing the value of a chosen property to a specified value over a set duration.
        /// </summary>
        /// <param name="element">The object to transform. A UIElement, FrameworkElement, Transform or any other inherited type from DependencyObject.</param>
        /// <param name="property">The name of the DependencyProperty of 'element' to transform.</param>
        /// <param name="value">The value to set the property to.</param>
        /// <param name="milliseconds">The duration of the animation in milliseconds.</param>
        internal static Task TransformDoubleProperty(DependencyObject element, string property, double value, int milliseconds)
        {
            Storyboard doubleStoryboard = new Storyboard();
            DoubleAnimation doubleAnimation = new DoubleAnimation()
            {
                To = value,
                Duration = TimeSpan.FromMilliseconds(milliseconds)
            };
            Storyboard.SetTarget(doubleAnimation, element);
            Storyboard.SetTargetProperty(doubleAnimation, property);

            doubleStoryboard.Children.Add(doubleAnimation);

            var tcs = new TaskCompletionSource<bool>();

            doubleAnimation.Completed += (s, e) =>
            {
                tcs.SetResult(true);
            };

            doubleStoryboard.Begin();
            return tcs.Task;
        }

    }
}
