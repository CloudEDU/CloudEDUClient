using CloudEDU.Common;
using CloudEDU.Login;
using CloudEDU.Service;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class CourseOverview : GlobalPage
    {
        Course course;
        CloudEDUEntities ctx = null;
        DataServiceQuery<COURSE_AVAIL> teachCourses;
        DataServiceQuery<ATTEND> buyCourses;

        bool isTeach;
        bool isBuy;

        DBAccessAPIs dba;
        /// <summary>
        /// Constructor, initialize the components
        /// </summary>
        public CourseOverview()
        {
            this.InitializeComponent();
            ctx = new CloudEDUEntities(new Uri(Constants.DataServiceURI));
            dba = new DBAccessAPIs();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            course = e.Parameter as Course;

            try
            {
                DataServiceQuery<COURSE_AVAIL> cDsq = (DataServiceQuery<COURSE_AVAIL>)(from kc in ctx.COURSE_AVAIL
                                                                                       where course.ID == kc.ID
                                                                                       select kc);
                TaskFactory<IEnumerable<COURSE_AVAIL>> tf = new TaskFactory<IEnumerable<COURSE_AVAIL>>();
                COURSE_AVAIL tmpCourse = (await tf.FromAsync(cDsq.BeginExecute(null, null), iar => cDsq.EndExecute(iar))).FirstOrDefault();
                course = Constants.CourseAvail2Course(tmpCourse);
            }
            catch
            {
                ShowMessageDialog("Network connection error2!");
                Frame.GoBack();
            }

            UserProfileBt.DataContext = Constants.User;
            DataContext = course;
            introGrid.DataContext = course;

            frame.Navigate(typeof(CourseDetail.Overview), course);

            isTeach = false;
            isBuy = false;

            try
            {
                teachCourses = (DataServiceQuery<COURSE_AVAIL>)(from teachC in ctx.COURSE_AVAIL
                                                                where teachC.TEACHER_NAME == Constants.User.NAME
                                                                select teachC);
                TaskFactory<IEnumerable<COURSE_AVAIL>> teachTF = new TaskFactory<IEnumerable<COURSE_AVAIL>>();
                IEnumerable<COURSE_AVAIL> tcs = await teachTF.FromAsync(teachCourses.BeginExecute(null, null), iar => teachCourses.EndExecute(iar));

                buyCourses = (DataServiceQuery<ATTEND>)(from buyC in ctx.ATTEND
                                                        where buyC.CUSTOMER_ID == Constants.User.ID
                                                        select buyC);
                TaskFactory<IEnumerable<ATTEND>> buyCF = new TaskFactory<IEnumerable<ATTEND>>();
                IEnumerable<ATTEND> bcs = await buyCF.FromAsync(buyCourses.BeginExecute(null, null), iar => buyCourses.EndExecute(iar));


                foreach (var t in tcs)
                {
                    if (t.ID == course.ID)
                    {
                        isTeach = true;
                        break;
                    }
                }

                foreach (var b in bcs)
                {
                    if (b.COURSE_ID == course.ID)
                    {
                        isBuy = true;
                        break;
                    }
                }
            }
            catch
            {
                ShowMessageDialog("Network connection error3!");
                Frame.GoBack();
            }

            if (course.Price == null || course.Price.Value == 0)
            {
                PriceTextBlock.Text = "Free";
            }
            else
            {
                PriceTextBlock.Text = "$ " + Math.Round(course.Price.Value, 2);
            }
            System.Diagnostics.Debug.WriteLine(course.Rate);
            SetStarsStackPanel(course.Rate ?? 0);

            if (isTeach)
            {
                courseButton.Content = "Teach";
            }
            else if (isBuy)
            {
                courseButton.Content = "Attend";
            }
            else
            {
                courseButton.Content = "Buy";
            }
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
        /// Invoked when overview button is clicked and navigating to the overview part.
        /// </summary>
        /// <param name="sender">The overview button clicked.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>
        private void OverviewButton_Click(object sender, RoutedEventArgs e)
        {
            var primaryStyle = Application.Current.Resources["TextPrimaryButtonStyle"] as Style;
            var secondaryStyle = Application.Current.Resources["TextSecondaryButtonStyle"] as Style;

            if (OverviewButton.Style != primaryStyle)
            {
                frame.Navigate(typeof(CourseDetail.Overview), course);

                OverviewButton.Style = primaryStyle;
                DetailsButton.Style = secondaryStyle;
                CommentsButton.Style = secondaryStyle;
            }
        }

        /// <summary>
        /// Invoked when detail button is clicked and navigating to the detail part.
        /// </summary>
        /// <param name="sender">The detail button clicked.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>
        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            var primaryStyle = Application.Current.Resources["TextPrimaryButtonStyle"] as Style;
            var secondaryStyle = Application.Current.Resources["TextSecondaryButtonStyle"] as Style;

            if (DetailsButton.Style != primaryStyle)
            {
                frame.Navigate(typeof(CourseDetail.Detail), course);

                OverviewButton.Style = secondaryStyle;
                DetailsButton.Style = primaryStyle;
                CommentsButton.Style = secondaryStyle;
            }
        }

        /// <summary>
        /// Invoked when comment button is clicked and navigating to the comment part.
        /// </summary>
        /// <param name="sender">The comment button clicked.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>
        private void CommentsButton_Click(object sender, RoutedEventArgs e)
        {
            var primaryStyle = Application.Current.Resources["TextPrimaryButtonStyle"] as Style;
            var secondaryStyle = Application.Current.Resources["TextSecondaryButtonStyle"] as Style;

            if (CommentsButton.Style != primaryStyle)
            {
                frame.Navigate(typeof(CourseDetail.Comment), course);

                OverviewButton.Style = secondaryStyle;
                DetailsButton.Style = secondaryStyle;
                CommentsButton.Style = primaryStyle;
            }
        }

        /// <summary>
        /// Set the rate star according the rate.
        /// </summary>
        /// <param name="rate">The rate of course.</param>
        private void SetStarsStackPanel(double rate)
        {
            int fillInt = (int)rate;
            int blankInt = 5 - fillInt - 1;
            double percentFill = rate - (double)fillInt;

            for (int i = 0; i < fillInt; ++i)
            {
                TextBlock fillStarTextBlock = new TextBlock
                {
                    Style = Application.Current.Resources["SubheaderTextStyle"] as Style,
                    Foreground = new SolidColorBrush(Colors.White),
                    Text = Constants.FillStar
                };
                rateStarsPanel.Children.Add(fillStarTextBlock);
            }
            if (rate == 5) return;
            double width = Constants.StarWidth * percentFill;
            TextBlock halfFillStarTextBlock = new TextBlock
            {
                Style = Application.Current.Resources["SubheaderTextStyle"] as Style,
                Foreground = new SolidColorBrush(Colors.White),
                Text = Constants.FillStar,
                Width = width
            };
            TextBlock halfBlankStarTextBlock = new TextBlock
            {
                Style = Application.Current.Resources["SubheaderTextStyle"] as Style,
                Foreground = new SolidColorBrush(Colors.White),
                Text = Constants.BlankStar,
                Margin = new Thickness(-width, 0, 0, 0)
            };
            rateStarsPanel.Children.Add(halfFillStarTextBlock);
            rateStarsPanel.Children.Add(halfBlankStarTextBlock);
            for (int i = 0; i < blankInt; ++i)
            {
                TextBlock blankStarTextBlock = new TextBlock
                {
                    Style = Application.Current.Resources["SubheaderTextStyle"] as Style,
                    Foreground = new SolidColorBrush(Colors.White),
                    Text = Constants.BlankStar,
                };
                rateStarsPanel.Children.Add(blankStarTextBlock);
            }
        }

        private void UserProfileButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Login.Profile));
        }

        private async void AttendButton_Click(object sender, RoutedEventArgs e)
        {
            Button bt = sender as Button;
            List<object> courseInfo = new List<object>();
            courseInfo.Add(course);

            if (bt.Content.ToString() == "Teach")
            {
                courseInfo.Add("teaching");
                Frame.Navigate(typeof(Coursing), courseInfo);
            }
            else if (bt.Content.ToString() == "Attend")
            {
                courseInfo.Add("attending");
                Frame.Navigate(typeof(Coursing), courseInfo);
            }
            else if (bt.Content.ToString() == "Buy")
            {
                bool isToBuy = false;
                bool isHaveBuy = false;

                var buySure = new MessageDialog("Are you sure to buy this course?", "Buy Course");
                buySure.Commands.Add(new UICommand("Yes", (command) =>
                    {
                        isToBuy = true;
                    }));
                buySure.Commands.Add(new UICommand("No", (command) =>
                    {
                        isToBuy = false;
                        return;
                    }));
                await buySure.ShowAsync();

                if (isToBuy)
                {
                    try
                    {
                        string uri = "/EnrollCourse?customer_id=" + Constants.User.ID + "&course_id=" + course.ID;
                        TaskFactory<IEnumerable<int>> tf = new TaskFactory<IEnumerable<int>>();
                        IEnumerable<int> code = await tf.FromAsync(ctx.BeginExecute<int>(new Uri(uri, UriKind.Relative), null, null), iar => ctx.EndExecute<int>(iar));
                        isHaveBuy = true;

                        if (code.FirstOrDefault() != 0)
                        {
                            isHaveBuy = false;
                            var buyError = new MessageDialog("You don't have enough money. Please contact Scott Zhao.", "Buy Failed");
                            buyError.Commands.Add(new UICommand("Close"));
                            await buyError.ShowAsync();
                            return;
                        }

                    }
                    catch
                    {
                        ShowMessageDialog("Network connection error1!");
                        return;
                    }
                }

                if (isHaveBuy)
                {
                    var buyOkMsg = new MessageDialog("Do you want to start learning?", "Buy successfully");
                    buyOkMsg.Commands.Add(new UICommand("Yes", (command) =>
                        {
                            courseInfo.Add("attending");
                            Frame.Navigate(typeof(Coursing), courseInfo);
                        }));
                    buyOkMsg.Commands.Add(new UICommand("No", (command) =>
                        {
                            bt.Content = "Attend";
                        }));
                    await buyOkMsg.ShowAsync();
                }
            }
        }

        /// <summary>
        /// Upload information error MessageDialog.
        /// </summary>
        private async void ShowMessageDialog(string msg)
        {
            var messageDialog = new MessageDialog(msg);
            messageDialog.Commands.Add(new UICommand("Close"));
            await messageDialog.ShowAsync();
        }
    }
}
