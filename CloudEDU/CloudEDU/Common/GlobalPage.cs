using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace CloudEDU.Common
{
    public class GlobalPage : Page
    {
        public static AppBar globalAppBar = null;

        public GlobalPage()
        {
            // Create the global appbar
            globalAppBar = new AppBar();
            globalAppBar.Content = new AppbarContent();
            globalAppBar.Background = Application.Current.Resources["AddBarBackgroundBrush"] as SolidColorBrush;
            globalAppBar.Opened += globalAppBar_Opened;

            this.BottomAppBar = globalAppBar;
        }

        void globalAppBar_Loaded(object sender, RoutedEventArgs e)
        {
            globalAppBar.Focus(Windows.UI.Xaml.FocusState.Programmatic);
        }

        void globalAppBar_Opened(object sender, object e)
        {
            globalAppBar.Focus(Windows.UI.Xaml.FocusState.Programmatic);
        }

        /// <summary>
        /// Invoked as an event handler to navigate backward in the page's associated
        /// <see cref="Frame"/> until it reaches the top of the navigation stack.
        /// </summary>
        /// <param name="sender">Instance that triggered the event.</param>
        /// <param name="e">Event data describing the conditions that led to the event.</param>
        protected virtual void GoHome(object sender, RoutedEventArgs e)
        {
            // Use the navigation frame to return to the topmost page
            if (this.Frame != null)
            {
                while (this.Frame.CanGoBack) this.Frame.GoBack();
            }
        }

        /// <summary>
        /// Invoked as an event handler to navigate backward in the navigation stack
        /// associated with this page's <see cref="Frame"/>.
        /// </summary>
        /// <param name="sender">Instance that triggered the event.</param>
        /// <param name="e">Event data describing the conditions that led to the
        /// event.</param>
        protected virtual void GoBack(object sender, RoutedEventArgs e)
        {
            // Use the navigation frame to return to the previous page
            if (this.Frame != null && this.Frame.CanGoBack) this.Frame.GoBack();
        }

        /// <summary>
        /// Invoked as an event handler to navigate forward in the navigation stack
        /// associated with this page's <see cref="Frame"/>.
        /// </summary>
        /// <param name="sender">Instance that triggered the event.</param>
        /// <param name="e">Event data describing the conditions that led to the
        /// event.</param>
        protected virtual void GoForward(object sender, RoutedEventArgs e)
        {
            // Use the navigation frame to move to the next page
            if (this.Frame != null && this.Frame.CanGoForward) this.Frame.GoForward();
        }
    }
}
