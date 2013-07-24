using JulMar.Windows.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace CloudEDU.Login
{
    /// <summary>
    /// Simple behavior for the PasswordBox to provide a watermark text element
    /// </summary>
    public static class PasswordBoxBehavior
    {
        private const string WatermarkId = "_pboxWatermark";

        /// <summary>
        /// Backing storage key for the text property.
        /// </summary>
        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.RegisterAttached("Watermark", typeof(string), typeof(PasswordBoxBehavior),
            new PropertyMetadata("", OnWatermarkChanged));

        /// <summary>
        /// Gets the watermark text
        /// </summary>
        /// <param name="pbox">The target PasswordBox.</param>
        /// <returns>The watermark text.</returns>
        public static string GetWatermark(PasswordBox pbox)
        {
            return (string)pbox.GetValue(WatermarkProperty);
        }

        /// <summary>
        /// Sets the watermark text.
        /// </summary>
        /// <param name="pbox">The target PasswordBox.</param>
        /// <param name="text">The watermark text to be set.</param>
        public static void SetWatermark(PasswordBox pbox, string text)
        {
            pbox.SetValue(WatermarkProperty, text);
        }

        /// <summary>
        /// Called when the watermark is changed.
        /// </summary>
        /// <param name="dpo"></param>
        /// <param name="e"></param>
        private static void OnWatermarkChanged(DependencyObject dpo, DependencyPropertyChangedEventArgs e)
        {
            var pbox = dpo as PasswordBox;
            if (pbox == null)
            {
                return;
            }

            pbox.PasswordChanged += PboxOnPasswordChanged;
            pbox.GotFocus += PboxOnGotFocus;
            pbox.LostFocus += PboxOnLostFocus;
            pbox.Loaded += PboxOnLoaded;

            string text = (e.NewValue ?? "").ToString();
            if (String.IsNullOrEmpty(text))
            {
                RemoveWatermarkElement(pbox);
            }
            else
            {
                AddWatermarkElement(pbox, text);
            }
        }

        /// <summary>
        /// Called when the PasswordBox is loaded. This adds the watermark if one is present.
        /// </summary>
        /// <param name="sender">The target PasswordBox.</param>
        /// <param name="routedEventArgs"></param>
        private static void PboxOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var pbox = sender as PasswordBox;
            string text = GetWatermark(pbox);

            if (String.IsNullOrEmpty(text))
            {
                RemoveWatermarkElement(pbox);
            }
            else
            {
                AddWatermarkElement(pbox, text);
            }
        }

        /// <summary>
        /// Called when the PasswordBox loses focus - this adds the watermark if necessary.
        /// </summary>
        /// <param name="sender">The target PasswordBox.</param>
        /// <param name="routedEventArgs"></param>
        private static void PboxOnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            var pbox = sender as PasswordBox;

            if (pbox.Password.Length == 0)
            {
                AddWatermarkElement(pbox, GetWatermark(pbox));
            }
        }

        /// <summary>
        /// Called when the PasswordBox gets focus - this removes any watermark.
        /// </summary>
        /// <param name="sender">The target PasswordBox.</param>
        /// <param name="routedEventArgs"></param>
        private static void PboxOnGotFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            var pbox = sender as PasswordBox;

            RemoveWatermarkElement(pbox);
        }

        /// <summary>
        /// This is called when the password is changed in the PasswordBox and removes the watermark.
        /// </summary>
        /// <param name="sender">The target PasswordBox.</param>
        /// <param name="routedEventArgs"></param>
        private static void PboxOnPasswordChanged(object sender, RoutedEventArgs routedEventArgs)
        {
            var pbox = sender as PasswordBox;

            if (pbox.Password.Length > 0)
            {
                RemoveWatermarkElement(pbox);
            }
        }

        /// <summary>
        /// Simple method to add a new TextBlock into the visual tree of the PasswordBox which will present the watermark.
        /// </summary>
        /// <param name="pbox">The target PasswordBox.</param>
        /// <param name="text">The watermark text.</param>
        private static void AddWatermarkElement(PasswordBox pbox, string text)
        {
            var watermarkTextBlock = pbox.FindVisualChildByName<TextBlock>(WatermarkId);

            if (watermarkTextBlock == null)
            {
                var fe = pbox.FindVisualChildByName<ScrollViewer>("ContentElement");
                if (fe != null)
                {
                    var panelOwner = fe.FindVisualParent<Panel>();
                    if (panelOwner != null)
                    {
                        // Add the TextBlock
                        var textBlock = new TextBlock
                        {
                            Name = WatermarkId,
                            Text = text,
                            TextAlignment = TextAlignment.Center,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            Margin = new Thickness(3, 0, 0, 0),
                            Foreground = new SolidColorBrush(Colors.Gray)
                        };
                        int index = panelOwner.Children.IndexOf(fe);
                        panelOwner.Children.Insert(index + 1, textBlock);
                    }
                }
            }
        }

        /// <summary>
        /// Simple method to remove the TextBlock from the PasswordBox visual tree.
        /// </summary>
        /// <param name="pbox">The target PasswordBox.</param>
        private static void RemoveWatermarkElement(PasswordBox pbox)
        {
            var watermarkTextBlock = pbox.FindVisualChildByName<TextBlock>(WatermarkId);
            if (watermarkTextBlock != null)
            {
                var panelOwner = watermarkTextBlock.FindVisualParent<Panel>();
                if (panelOwner != null)
                {
                    panelOwner.Children.Remove(watermarkTextBlock);
                }
            }
        }
    }
}
