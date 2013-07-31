using CloudEDU.Common;
using CloudEDU.Service;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.IO;
using System.Linq;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;
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
    public sealed partial class Category : GlobalPage
    {
        private StoreData categoryCourses;
        private List<GroupInfoList<Object>> dataCategory;
        private CloudEDUEntities ctx = null;
        private DataServiceQuery<COURSE_AVAIL> courseDsq = null;

        string categoryName = null;

        public Category()
        {
            this.InitializeComponent();

            ctx = new CloudEDUEntities(new Uri(Constants.DataServiceURI));
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            categoryName = e.Parameter as string;
            Title.Text = Constants.UpperInitialChar(categoryName);
            loadingProgressRing.IsActive = true;

            courseDsq = (DataServiceQuery<COURSE_AVAIL>)(from course_avail in ctx.COURSE_AVAIL
                                                         where course_avail.CATE_NAME == categoryName
                                                         select course_avail);
            courseDsq.BeginExecute(OnCategoryCoursesComplete, null);
            UserProfileBt.DataContext = Constants.User;

        }

        /// <summary>
        /// DataServiceQuery callback method to refresh the UI.
        /// </summary>
        /// <param name="result">Async operation result.</param>
        private async void OnCategoryCoursesComplete(IAsyncResult result)
        {
            categoryCourses = new StoreData();
            try
            {
                IEnumerable<COURSE_AVAIL> courses = courseDsq.EndExecute(result);
                foreach (var c in courses)
                {
                    categoryCourses.AddCourse(Constants.CourseAvail2Course(c));
                }
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        dataCategory = categoryCourses.GetSingleGroupByCategoryTitle(categoryName);
                        cvs1.Source = dataCategory;
                        loadingProgressRing.IsActive = false;
                    });
            }
            catch
            {
                ShowMessageDialog();
            }
        }

        /// <summary>
        /// Network Connection error MessageDialog.
        /// </summary>
        private async void ShowMessageDialog()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                try
                {
                    var messageDialog = new MessageDialog("No Network has been found!");
                    messageDialog.Commands.Add(new UICommand("Try Again", (command) =>
                    {
                        Frame.Navigate(typeof(Courstore));
                    }));
                    messageDialog.Commands.Add(new UICommand("Close"));
                    loadingProgressRing.IsActive = false;
                    await messageDialog.ShowAsync();
                }
                catch
                {
                    ShowMessageDialog();
                }
            });
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
                Frame.Navigate(typeof(Courstore));
            }
        }

        /// <summary>
        /// Invoked when a course within a category is clicked.
        /// </summary>
        /// <param name="sender">The GridView displaying the course clicked.</param>
        /// <param name="e">Event data that describes the course clicked.</param>
        private void Course_ItemClick(object sender, ItemClickEventArgs e)
        {
            Course course = (Course)e.ClickedItem;

            Frame.Navigate(typeof(CourseOverview), course);
        }

        /// <summary>
        /// Invoked when a category is clicked.
        /// </summary>
        /// <param name="sender">The Button used as a category for the selected category.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>
        private void HeaderButton_Click(object sender, RoutedEventArgs e)
        {
            if (categoryName == "newest")
            {
                Frame.Navigate(typeof(CategoryForNewest));
            }
        }
        private void UserProfileButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Login.Profile));
        }
        
    }
}
