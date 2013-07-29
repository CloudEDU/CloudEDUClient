using CloudEDU.Common;
using System;
using System.Collections.Generic;
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

namespace CloudEDU
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Coursing : GlobalPage
    {
        SolidColorBrush pageRed;
        SolidColorBrush pageBlue;
        SolidColorBrush pageGreen;
        SolidColorBrush pageWhite;
        SolidColorBrush pageBlack;

        public Coursing()
        {
            this.InitializeComponent();

            pageRed = this.Resources["PageRed"] as SolidColorBrush;
            pageBlue = this.Resources["PageBlue"] as SolidColorBrush;
            pageGreen = this.Resources["PageGreen"] as SolidColorBrush;
            pageWhite = new SolidColorBrush(Colors.White);
            pageBlack = new SolidColorBrush(Colors.Black);
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            List<string> courseInfo = e.Parameter as List<string>;
            CourseTitle.Text = Constants.UpperInitialChar(courseInfo[0]);
            NavigateText.Text = courseInfo[1];

            HomeBorder.Background = pageRed;
            LecturesBorder.Background = pageWhite;
            NotesBorder.Background = pageWhite;

            HomeText.Foreground = pageWhite;
            LecturesText.Foreground = pageBlack;
            NotesText.Foreground = pageBlack;
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
        /// Invoked when home text is tapped and navigating to the home fame
        /// </summary>
        /// <param name="sender">The home text tapped.</param>
        /// <param name="e">Event data that describes how the tap was initiated.</param>
        private void HomeText_Tapped(object sender, TappedRoutedEventArgs e)
        {
            HomeBorder.Background = pageRed;
            LecturesBorder.Background = pageWhite;
            NotesBorder.Background = pageWhite;

            HomeText.Foreground = pageWhite;
            LecturesText.Foreground = pageBlack;
            NotesText.Foreground = pageBlack;

            ContentBackgroundRect.Fill = pageRed;
        }

        /// <summary>
        /// Invoked when lecture text is tapped and navigating to the lecture fame
        /// </summary>
        /// <param name="sender">The lecture text tapped.</param>
        /// <param name="e">Event data that describes how the tap was initiated.</param>
        private void LecturesText_Tapped(object sender, TappedRoutedEventArgs e)
        {
            HomeBorder.Background = pageWhite;
            LecturesBorder.Background = pageBlue;
            NotesBorder.Background = pageWhite;

            HomeText.Foreground = pageBlack;
            LecturesText.Foreground = pageWhite;
            NotesText.Foreground = pageBlack;

            ContentBackgroundRect.Fill = pageBlue;
        }

        /// <summary>
        /// Invoked when note text is tapped and navigating to the note fame
        /// </summary>
        /// <param name="sender">The note text tapped.</param>
        /// <param name="e">Event data that describes how the tap was initiated.</param>
        private void NotesText_Tapped(object sender, TappedRoutedEventArgs e)
        {
            HomeBorder.Background = pageWhite;
            LecturesBorder.Background = pageWhite;
            NotesBorder.Background = pageGreen;

            HomeText.Foreground = pageBlack;
            LecturesText.Foreground = pageBlack;
            NotesText.Foreground = pageWhite;

            ContentBackgroundRect.Fill = pageGreen;
        }
    }
}
