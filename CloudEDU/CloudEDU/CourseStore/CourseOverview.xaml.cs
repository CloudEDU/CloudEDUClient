using CloudEDU.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CloudEDU.CourseStore
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CourseOverview : GlobalPage
    {
        string courseName;

        public CourseOverview()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            courseName = e.Parameter as string;

            frame.Navigate(typeof(CourseDetail.Overview), courseName);
        }

        /// <summary>
        /// Invoked when back button is clicked and return the last page.
        /// </summary>
        /// <param name="sender">The back button clicked.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
            else
            {
                Frame.Navigate(typeof(CourseStore.Courstore));
            }
        }

        /// <summary>
        /// Invoked when overview button is clicked and navigating to the overview part.
        /// </summary>
        /// <param name="sender">The overview button clicked.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>
        private void OverviewButton_Click(object sender, RoutedEventArgs e)
        {
            var primaryStyle = Application.Current.Resources["TextPrimaryButtonStyle"] as Style;
            var secondaryStyle = Application.Current.Resources["TextSecondaryButtonStyle"] as Style;

            if (OverviewButton.Style != primaryStyle)
            {
                frame.Navigate(typeof(CourseDetail.Overview), courseName);

                OverviewButton.Style = primaryStyle;
                DetailsButton.Style = secondaryStyle;
                CommentsButton.Style = secondaryStyle;
            }
        }

        /// <summary>
        /// Invoked when detail button is clicked and navigating to the detail part.
        /// </summary>
        /// <param name="sender">The detail button clicked.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>
        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            var primaryStyle = Application.Current.Resources["TextPrimaryButtonStyle"] as Style;
            var secondaryStyle = Application.Current.Resources["TextSecondaryButtonStyle"] as Style;

            if (DetailsButton.Style != primaryStyle)
            {
                frame.Navigate(typeof(CourseDetail.Detail), courseName);

                OverviewButton.Style = secondaryStyle;
                DetailsButton.Style = primaryStyle;
                CommentsButton.Style = secondaryStyle;
            }
        }

        /// <summary>
        /// Invoked when comment button is clicked and navigating to the comment part.
        /// </summary>
        /// <param name="sender">The comment button clicked.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>
        private void CommentsButton_Click(object sender, RoutedEventArgs e)
        {
            var primaryStyle = Application.Current.Resources["TextPrimaryButtonStyle"] as Style;
            var secondaryStyle = Application.Current.Resources["TextSecondaryButtonStyle"] as Style;

            if (CommentsButton.Style != primaryStyle)
            {
                frame.Navigate(typeof(CourseDetail.Comment), courseName);

                OverviewButton.Style = secondaryStyle;
                DetailsButton.Style = secondaryStyle;
                CommentsButton.Style = primaryStyle;
            }
        }
    }
}
