using CloudEDU.Login;
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

namespace CloudEDU
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
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
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Login.Login));
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Login.SignUp));
        }

        private void CourseOverview_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CourseStore.CourseOverview));
        }

        private void MyCourses_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Coursing));
        }

        private void CourseStore_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CourseStore.Courstore));
        }
    }
}
