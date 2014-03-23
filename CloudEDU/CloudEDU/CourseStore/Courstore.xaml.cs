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
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            
            loadingProgressRing.IsActive = true;

            try
            {
                if (Constants.RecUriDic.Count == 0 && Constants.CategoryNameList.Count == 0)
                {



                    try
                    {
                        DataServiceQuery<CATEGORY> cateDsq = (DataServiceQuery<CATEGORY>)(from cate in ctx.CATEGORY
                                                                                          select cate);
                        TaskFactory<IEnumerable<CATEGORY>> tfc = new TaskFactory<IEnumerable<CATEGORY>>();
                        IEnumerable<CATEGORY> categories = await tfc.FromAsync(cateDsq.BeginExecute(null, null), iar => cateDsq.EndExecute(iar));
                        foreach (var c in categories)
                        {
                            Constants.CategoryNameList.Add(c.CATE_NAME);
                        }
                    }
                    catch
                    {
                        ShowMessageDialog("categories!");
                    }
                    try
                    {
                        DataServiceQuery<RECOMMENDATION> craDsq = (DataServiceQuery<RECOMMENDATION>)(from re in ctx.RECOMMENDATION
                                                                                                     select re);
                        TaskFactory<IEnumerable<RECOMMENDATION>> tf = new TaskFactory<IEnumerable<RECOMMENDATION>>();
                        IEnumerable<RECOMMENDATION> recommendation = await tf.FromAsync(craDsq.BeginExecute(null, null), iar => craDsq.EndExecute(iar));
                        foreach (var r in recommendation)
                        {
                            Constants.RecUriDic.Add(r.TITLE, r.ICON_URL);
                        }
                    }
                    catch
                    {
                        ShowMessageDialog("recommedations!");
                    }
                    


                    

                    
                }
            }
            catch
            {
                ShowMessageDialog("On nav to");
            }

            courseDsq = (DataServiceQuery<COURSE_AVAIL>)(from course_avail in ctx.COURSE_AVAIL select course_avail);
            courseDsq.BeginExecute(OnCourseAvailComplete, null);
            UserProfileBt.DataContext = Constants.User;
            //UserProfileBt.IsEnabled = false;
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
                DataServiceQuery<COURSE_RECO_AVAIL> craDsq = (DataServiceQuery<COURSE_RECO_AVAIL>)(from re in ctx.COURSE_RECO_AVAIL
                                                                                             select re);
                TaskFactory<IEnumerable<COURSE_RECO_AVAIL>> tf = new TaskFactory<IEnumerable<COURSE_RECO_AVAIL>>();
                IEnumerable<COURSE_RECO_AVAIL> recommendation = await tf.FromAsync(craDsq.BeginExecute(null, null), iar => craDsq.EndExecute(iar));

                foreach (var re in recommendation)
                {
                    coursesData.AddCourse(Constants.CourseRecAvail2Course(re));
                }

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
                ShowMessageDialog("on course avail complete");
                // Network Connection error.
            }
        }

        /// <summary>
        /// Network Connection error MessageDialog.
        /// </summary>
        private async void ShowMessageDialog(String msg = "No Network has been found!")
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    var messageDialog = new MessageDialog(msg);
                    messageDialog.Commands.Add(new UICommand("Try Again", (command) =>
                        {
                            Frame.Navigate(typeof(Courstore));
                        }));
                    messageDialog.Commands.Add(new UICommand("Close"));
                    loadingProgressRing.IsActive = false;
                    await messageDialog.ShowAsync();
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
