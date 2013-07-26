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
    public sealed partial class Courstore : GlobalPage
    {
        private StoreData storeSampleData;
        private List<GroupInfoList<Object>> dataCategory;

        public Courstore()
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
            storeSampleData = new StoreData();
            dataCategory = storeSampleData.GetGroupsByCategory();
            cvs1.Source = dataCategory;
            (SemanticZoom.ZoomedOutView as ListViewBase).ItemsSource = cvs1.View.CollectionGroups;

            Uri uri = new Uri("http://10.0.1.16:8080/cloudeduserver/courseservice.svc");
            CloudEDU.CourseService.CloudEDUEntities ctx = new CloudEDU.CourseService.CloudEDUEntities(uri);
           //System.Diagnostics.Debug.WriteLine(ctx.CUSTOMERs.Where(c => c.ID == 1).FirstOrDefault().NAME);
            var query = ctx.CreateQuery<Category>("GetAllCategory");
            System.Diagnostics.Debug.WriteLine(query.ToString());
            try
            {
                System.Diagnostics.Debug.WriteLine(query.ToString());
                foreach (var res in query)
                {
                    System.Diagnostics.Debug.WriteLine(res.ToString());
                }
            }
            catch
            {
            }

        }

        /// <summary>
        /// Invoked when a category is clicked.
        /// </summary>
        /// <param name="sender">The Button used as a category for the selected category.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>
        private void CategoryButton_Click(object sender, RoutedEventArgs e)
        {
            var category = (sender as FrameworkElement).DataContext;
            string categoryName = ((GroupInfoList<Object>)category).Key.ToString();

            Frame.Navigate(typeof(Category), categoryName);
        }

        /// <summary>
        /// Invoked when a course within a category is clicked.
        /// </summary>
        /// <param name="sender">The GridView displaying the course clicked.</param>
        /// <param name="e">Event data that describes the course clicked.</param>
        private void Course_ItemClick(object sender, ItemClickEventArgs e)
        {
            var courseName = ((Course)e.ClickedItem).Name;

            Frame.Navigate(typeof(CourseOverview), courseName);
        }
    }
}
