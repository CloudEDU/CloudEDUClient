using CloudEDU.Common;
using CloudEDU.Service;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

        CloudEDUEntities ctx;

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

            ProgressBar progressBar = new ProgressBar()
            {
                
            };

            Uri uri = new Uri("http://10.0.1.39:8080/CloudEDUServer/CourseService.svc/");
            ctx = new CloudEDUEntities(uri);

            //ctx.BeginExecute<COURSE_OK>(new Uri("GetCoursesByName?name='Test Title2'", UriKind.Relative), OnComplete, null);

            //DataServiceQuery<COURSE_OK> dps = (DataServiceQuery<COURSE_OK>)(from c in ctx.COURSE_OK where c.TITLE == "Test Title2" select c);
            
            //TaskFactory<IEnumerable<COURSE_OK>> tf = new TaskFactory<IEnumerable<COURSE_OK>>();
            //IEnumerable<COURSE_OK> courses = await tf.FromAsync(dps.BeginExecute(null, null), ira => dps.EndExecute(ira));

            //foreach (COURSE_OK c in courses)
            //{
            //    System.Diagnostics.Debug.WriteLine(c.TITLE);
            //}


            //query = (DataServiceQuery<COURSE>)(from c in ctx.COURSEs select c);

            //try
            //{
            //    query.BeginExecute(OnComplete, query);
            //}
            //catch (Exception ex)
            //{
            //    System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            //}
        }

        //public void OnComplete(IAsyncResult result)
        //{
        //    var courses = ctx.EndExecute<COURSE_OK>(result);
        //    foreach (var c in courses)
        //    {
        //        System.Diagnostics.Debug.WriteLine(c.TITLE);
        //    }
        //}

        //private void OnComplete(object sender, LoadCompletedEventArgs e)
        //{
        //    System.Diagnostics.Debug.WriteLine("==============================================================================");
        //    System.Diagnostics.Debug.WriteLine(e.Cancelled);
        //    System.Diagnostics.Debug.WriteLine("==============================================================================");
        //    System.Diagnostics.Debug.WriteLine(e.Error);
        //    System.Diagnostics.Debug.WriteLine("==============================================================================");
        //    System.Diagnostics.Debug.WriteLine(sender.ToString());
        //}

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
            var courseName = ((Course)e.ClickedItem).Title;

            Frame.Navigate(typeof(CourseOverview), courseName);
        }
    }
}
