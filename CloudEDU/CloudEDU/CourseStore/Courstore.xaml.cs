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
    public sealed partial class Courstore : GlobalPage
    {
        private StoreData coursesData;
        private List<GroupInfoList<object>> dataCategory;
        private CloudEDUEntities ctx = null;
        private DataServiceQuery<COURSE_AVAIL> courseDsq = null;

        /// <summary>
        /// Constructor, initialize the components
        /// </summary>
        public Courstore()
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
            loadingProgressRing.IsActive = true;

            courseDsq = (DataServiceQuery<COURSE_AVAIL>)(from course_avail in ctx.COURSE_AVAIL select course_avail);
            courseDsq.BeginExecute(OnCourseAvailComplete, null);
            UserProfileBt.DataContext = Constants.User;


            //DataServiceQuery<COURSE> dps = (DataServiceQuery<COURSE>)(from c in DataServiceContextSingleton.SharedDataServiceContext().COURSEs where c.TITLE == "Test Title2" select c);

            

//TaskFactory<IEnumerable<COURSE>> tf = new TaskFactory<IEnumerable<COURSE>>();
            //IEnumerable<COURSE> courses = await tf.FromAsync(dps.BeginExecute(null, null), ira => dps.EndExecute(ira));
            //courseDsq = (DataServiceQuery<COURSE_AVAIL>)(from course_avail in DataServiceContextSingleton.SharedDataServiceContext().COURSE_AVAIL select course_avail);
            //DataServiceContextSingleton.SharedDataServiceContext().BeginExecute<int?>(new Uri("CreateCourse?teacher_id=3&title='HaoHaoDBL'&intro='HaoHaoYouDBL'&category_id=3&price=0&pg_id=1&icon_url='www.HaoHaoDBL.com'", UriKind.Relative), OnComplete, null);

            //ctx.BeginExecute<COURSE_OK>(new Uri("GetCoursesByName?name='Test Title2'", UriKind.Relative), OnComplete, null);



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

        /// <summary>
        /// DataServiceQuery callback method to refresh the UI.
        /// </summary>
        /// <param name="result">Async operation result.</param>
        private async void OnCourseAvailComplete(IAsyncResult result)
        {
            coursesData = new StoreData();
            try
            {
                IEnumerable<COURSE_AVAIL> courses = courseDsq.EndExecute(result);
                foreach (var c in courses)
                {
                    coursesData.AddCourse(Constants.CourseAvail2Course(c));
                }
                
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    dataCategory = coursesData.GetGroupsByCategory();
                    cvs1.Source = dataCategory;
                    (SemanticZoom.ZoomedOutView as ListViewBase).ItemsSource = cvs1.View.CollectionGroups;
                    loadingProgressRing.IsActive = false;
                });
            }
            catch
            {
                ShowMessageDialog();
                // Network Connection error.
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

        //public void OnComplete(IAsyncResult result)
        //{
        //    var courses = ctx.EndExecute<COURSE_OK>(result);
        //    foreach (var c in courses)
        //    {
        //        System.Diagnostics.Debug.WriteLine(c.TITLE);
        //    }
        //}

        /// <summary>
        /// Invoked when a category is clicked.
        /// </summary>
        /// <param name="sender">The Button used as a category for the selected category.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>
        private void CategoryButton_Click(object sender, RoutedEventArgs e)
        {
            var category = (sender as FrameworkElement).DataContext;
            string categoryName = ((GroupInfoList<object>)category).Key.ToString();

            Frame.Navigate(typeof(Category), categoryName);
        }

        /// <summary>
        /// Invoked when a course within a category is clicked.
        /// </summary>
        /// <param name="sender">The GridView displaying the course clicked.</param>
        /// <param name="e">Event data that describes the course clicked.</param>
        private void Course_ItemClick(object sender, ItemClickEventArgs e)
        {
            Course course = (Course)e.ClickedItem;

            System.Diagnostics.Debug.WriteLine(course.Rate);

            Frame.Navigate(typeof(CourseOverview), course);
        }



        private void UserProfileButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Login.Profile));
        }
    }
}
