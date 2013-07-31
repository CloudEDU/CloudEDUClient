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
    public sealed partial class MyCourses : GlobalPage
    {
        private StoreData courseData;
        private List<GroupInfoList<Object>> dataCategory;

        private CloudEDUEntities ctx = null;
        private DataServiceQuery<COURSE_AVAIL> attendDsq = null;
        private DataServiceQuery<COURSE_AVAIL> teachDsq = null;

        /// <summary>
        /// Constructor, initialized the components.
        /// </summary>
        public MyCourses()
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
            attendDsq = (DataServiceQuery<COURSE_AVAIL>)(from attend in ctx.ATTEND
                                                         join course in ctx.COURSE_AVAIL
                                                         on attend.COURSE_ID equals course.ID
                                                         where attend.CUSTOMER_ID == Constants.User.ID
                                                         select course);
            teachDsq = (DataServiceQuery<COURSE_AVAIL>)(from course in ctx.COURSE_AVAIL
                                                        where course.TEACHER_NAME == Constants.User.NAME
                                                        select course);

            TaskFactory<IEnumerable<COURSE_AVAIL>> tf = new TaskFactory<IEnumerable<COURSE_AVAIL>>();
            IEnumerable<COURSE_AVAIL> attends = await tf.FromAsync(attendDsq.BeginExecute(null, null), iar => attendDsq.EndExecute(iar));
            IEnumerable<COURSE_AVAIL> teaches = await tf.FromAsync(teachDsq.BeginExecute(null, null), iar => teachDsq.EndExecute(iar));

            courseData = new StoreData();
            foreach (var c in attends)
            {
                Course tmpCourse = Constants.CourseAvail2Course(c);
                tmpCourse.IsBuy = true;
                tmpCourse.IsTeach = false;
                courseData.AddCourse(tmpCourse);
            }
            foreach (var c in teaches)
            {
                Course tmpCourse = Constants.CourseAvail2Course(c);
                tmpCourse.IsTeach = true;
                tmpCourse.IsBuy = false;
                courseData.AddCourse(tmpCourse);
            }
            
            dataCategory = courseData.GetGroupsByAttendingOrTeaching();
            cvs1.Source = dataCategory;
            UserProfileBt.DataContext = Constants.User;
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
        /// Invoked when a course within attending or teaching column is clicked.
        /// </summary>
        /// <param name="sender">The GridView displaying the course clicked.</param>
        /// <param name="e">Event data that describes the course clicked.</param>
        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Course course = (Course)e.ClickedItem;
            List<object> courseInfo = new List<object>();
            courseInfo.Add(course);

            if (course.IsTeach)
            {
                courseInfo.Add("teaching");
            }
            else
            {
                courseInfo.Add("attending");
            }

            Frame.Navigate(typeof(Coursing), courseInfo);
        }

        private void UserProfileButton_Click(object sender, RoutedEventArgs e)
        {
            //Frame.Navigate(typeof());
        }
    }
}
