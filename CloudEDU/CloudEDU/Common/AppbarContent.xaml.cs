using CloudEDU.Common;
using CloudEDU.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
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
        public static TextBox searchTextBlock = null;
     
        public AppbarContent()
        {
            this.InitializeComponent();

            searchTextBlock = searchBox;
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

        private async void LogoutButton_Click_1(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(Login.LoginSel));
            string courseUplaodUri = "/AddDBLog?opr='Logout'&msg='" + Constants.User.NAME + "'";
            //ctx.UpdateObject(c);
            CloudEDUEntities ctx = new CloudEDUEntities(new Uri(Constants.DataServiceURI));
            try
            {
                TaskFactory<IEnumerable<bool>> tf = new TaskFactory<IEnumerable<bool>>();
                IEnumerable<bool> result = await tf.FromAsync(ctx.BeginExecute<bool>(new Uri(courseUplaodUri, UriKind.Relative), null, null), iar => ctx.EndExecute<bool>(iar));
            }
            catch
            {
            }
        }

        private void Enter_Clicked(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                string searchText = searchBox.Text;
                if (searchText != null && searchText.Length != 0)
                {
                    ((Frame)Window.Current.Content).Navigate(typeof(CourseStore.SearchResult), searchText);
                }
            }
        }
    }
}
