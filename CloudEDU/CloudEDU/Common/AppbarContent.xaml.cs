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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace CloudEDU.Common
{
    public sealed partial class AppbarContent : UserControl
    {
        public AppbarContent()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// AppBar Button used to navigate to Courstore Page.
        /// </summary>
        /// <param name="sender">The Courstore button clicked.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>
        private void CourstoreButton_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(CourseStore.Courstore));
        }

        /// <summary>
        /// AppBar Button used to navigate to MyCourses Page.
        /// </summary>
        /// <param name="sender">My Courses button clicked.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>
        private void MyCoursesButton_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(CourseStore.MyCourses));
        }

        /// <summary>
        /// AppBar Button used to navigate to Uploading Page.
        /// </summary>
        /// <param name="sender">The Uploading Course button clicked.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>
        private void UploadCourseButton_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(Uploading));
        }

        private void LogoutButton_Click_1(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(Login.Login));
        }
    }
}
