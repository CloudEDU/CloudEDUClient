using CloudEDU.Common;
using CloudEDU.Login;
using CloudEDU.Service;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
        Course course;
        CloudEDUEntities ctx = null;

        DBAccessAPIs dba;
        /// <summary>
        /// Constructor, initialize the components
        /// </summary>
        public CourseOverview()
        {
            this.InitializeComponent();
            ctx = new CloudEDUEntities(new Uri(Constants.DataServiceURI));
            dba = new DBAccessAPIs();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            course = e.Parameter as Course;
            UserProfileBt.DataContext = Constants.User;
            DataContext = course;
            frame.Navigate(typeof(CourseDetail.Overview), course);

            if (course.Price == null || course.Price.Value == 0)
            {
                PriceTextBlock.Text = "Free";
            }
            else
            {
                PriceTextBlock.Text = "$ " + Math.Round(course.Price.Value, 2);
            }
            SetStarsStackPanel(course.Rate ?? 0);
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
                frame.Navigate(typeof(CourseDetail.Overview), course);

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
                frame.Navigate(typeof(CourseDetail.Detail), course);

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
                frame.Navigate(typeof(CourseDetail.Comment), course);

                OverviewButton.Style = secondaryStyle;
                DetailsButton.Style = secondaryStyle;
                CommentsButton.Style = primaryStyle;
            }
        }

        /// <summary>
        /// Set the rate star according the rate.
        /// </summary>
        /// <param name="rate">The rate of course.</param>
        private void SetStarsStackPanel(double rate)
        {
            int fillInt = (int)rate;
            int blankInt = 5 - fillInt - 1;
            double percentFill = rate - (double)fillInt;

            for (int i = 0; i < fillInt; ++i)
            {
                TextBlock fillStarTextBlock = new TextBlock
                {
                    Style = Application.Current.Resources["SubheaderTextStyle"] as Style,
                    Foreground = new SolidColorBrush(Colors.White),
                    Text = Constants.FillStar
                };
                rateStarsPanel.Children.Add(fillStarTextBlock);
            }
            if (rate == 5) return;
            double width = Constants.StarWidth * percentFill;
            TextBlock halfFillStarTextBlock = new TextBlock
            {
                Style = Application.Current.Resources["SubheaderTextStyle"] as Style,
                Foreground = new SolidColorBrush(Colors.White),
                Text = Constants.FillStar,
                Width = width
            };
            TextBlock halfBlankStarTextBlock = new TextBlock
            {
                Style = Application.Current.Resources["SubheaderTextStyle"] as Style,
                Foreground = new SolidColorBrush(Colors.White),
                Text = Constants.BlankStar,
                Margin = new Thickness(-width, 0, 0, 0)
            };
            rateStarsPanel.Children.Add(halfFillStarTextBlock);
            rateStarsPanel.Children.Add(halfBlankStarTextBlock);
            for (int i = 0; i < blankInt; ++i)
            {
                TextBlock blankStarTextBlock = new TextBlock
                {
                    Style = Application.Current.Resources["SubheaderTextStyle"] as Style,
                    Foreground = new SolidColorBrush(Colors.White),
                    Text = Constants.BlankStar,
                };
                rateStarsPanel.Children.Add(blankStarTextBlock);
            }
        }

        private void UserProfileButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Login.Profile));
        }

        private void AttendButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
