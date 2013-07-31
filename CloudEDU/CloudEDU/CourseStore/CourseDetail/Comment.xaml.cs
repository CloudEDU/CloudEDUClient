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
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CloudEDU.CourseStore.CourseDetail
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Comment : Page
    {
        private Course course;
        private int globalRate;
        private List<COMMENT_DET> allComments;

        private CloudEDUEntities ctx = null;
        private DataServiceQuery<COMMENT_DET> commentDsq = null;
        private DataServiceQuery<ATTEND> attendDsq = null;

        /// <summary>
        /// Constructor, initialize the components.
        /// </summary>
        public Comment()
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
            course = e.Parameter as Course;
            globalRate = 0;

            attendDsq = (DataServiceQuery<ATTEND>)(from attend in ctx.ATTEND
                                                   where attend.COURSE_ID == course.ID && attend.CUSTOMER_ID == Constants.User.ID
                                                   select attend);

            try
            {
                TaskFactory<IEnumerable<ATTEND>> tf = new TaskFactory<IEnumerable<ATTEND>>();
                IEnumerable<ATTEND> attends = await tf.FromAsync(attendDsq.BeginExecute(null, null), iar => attendDsq.EndExecute(iar));
                if (attends.Count() != 0)
                {
                    enterCommentStackPanel.Visibility = Visibility.Visible;
                }
            }
            catch
            {
                ShowMessageDialog();
            }

            commentDsq = (DataServiceQuery<COMMENT_DET>)(from comment in ctx.COMMENT_DET
                                                         where comment.COURSE_ID == course.ID
                                                         orderby comment.TIME ascending
                                                         select comment);
            commentDsq.BeginExecute(OnCommentComplete, null);
        }

        private async void OnCommentComplete(IAsyncResult result)
        {
            try
            {
                IEnumerable<COMMENT_DET> coms = commentDsq.EndExecute(result);
                allComments = new List<COMMENT_DET>(coms);
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        foreach (COMMENT_DET c in allComments)
                        {
                            StackPanel newComment = GenerateACommentBox(c.USERNAME, c.TITLE, Convert.ToInt32(c.RATE), c.CONTENT);
                            commentsStackPanel.Children.Add(newComment);
                        }
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
                        Frame.Navigate(typeof(Comment));
                    }));
                    messageDialog.Commands.Add(new UICommand("Close"));
                    await messageDialog.ShowAsync();
                }
                catch
                {
                    ShowMessageDialog();
                }
            });
        }

        /// <summary>
        /// Invoked when Add Comment button clicked and add comment.
        /// </summary>
        /// <param name="sender">The Add Comment button clicked.</param>
        /// <param name="e"></param>
        private void AddCommentButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(newTitleTextBox.Text);
            if (newTitleTextBox.Text == "" || newContentTextBox.Text == "" || newTitleTextBox.Text.Trim() == "Title")
            {
                WarningTextBlock.Visibility = Visibility.Visible;
                return;
            }
            StackPanel newComment = GenerateACommentBox(Constants.User.NAME, newTitleTextBox.Text, globalRate, newContentTextBox.Text);

            COMMENT commentEntity = new COMMENT();
            commentEntity.COURSE_ID = course.ID.Value;
            commentEntity.CUSTOMER_ID = Constants.User.ID;
            commentEntity.TITLE = newTitleTextBox.Text;
            commentEntity.RATE = globalRate;
            commentEntity.CONTENT = newContentTextBox.Text;
            ctx.AddToCOMMENT(commentEntity);
            ctx.BeginSaveChanges(OnAddCommentComplete, null);

            commentsStackPanel.Children.Add(newComment);

            newTitleTextBox.Text = newContentTextBox.Text = "";
            globalRate = 0;
            SetStarTextBlock(globalRate);
            WarningTextBlock.Visibility = Visibility.Collapsed;
        }

        private void OnAddCommentComplete(IAsyncResult result)
        {
            ctx.EndSaveChanges(result);
        }

        /// <summary>
        /// Create a stackpanel representing a comment.
        /// </summary>
        /// <param name="title">Comment title.</param>
        /// <param name="rate">Comment rate.</param>
        /// <param name="content">Comment content.</param>
        /// <returns>Comment Stackpanel created.</returns>
        private StackPanel GenerateACommentBox(string username, string title, int rate, string content)
        {
            TextBlock userTextBlock = new TextBlock
            {
                Style = Application.Current.Resources["SubheaderTextStyle"] as Style,
                FontWeight = FontWeights.Bold,
                Text = username
            };
            TextBlock dateTextBlock = new TextBlock
            {
                Style = Application.Current.Resources["SubheaderTextStyle"] as Style,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(40, 0, 0, 0),
                Text = DateTime.Now.Year.ToString() + "." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString()
            };
            TextBlock timeTextBlock = new TextBlock
            {
                Style = Application.Current.Resources["SubheaderTextStyle"] as Style,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(20, 0, 0, 0),
                Text = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString()
            };
            TextBlock rateTextBlock = new TextBlock
            {
                Style = Application.Current.Resources["SubheaderTextStyle"] as Style,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(30, 0, 0, 0),
                Text = ""
            };
            for (int i = 0; i < rate; ++i)
            {
                rateTextBlock.Text += Constants.FillStar;
            }

            StackPanel insidePanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(10, 3, 3, 3)
            };
            insidePanel.Children.Add(userTextBlock);
            insidePanel.Children.Add(dateTextBlock);
            insidePanel.Children.Add(timeTextBlock);
            insidePanel.Children.Add(rateTextBlock);

            TextBlock titleTextBlock = new TextBlock
            {
                Style = Application.Current.Resources["SubheaderTextStyle"] as Style,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(10, 3, 3, 3),
                Text = title
            };

            TextBlock contentTextBlock = new TextBlock
            {
                Style = Application.Current.Resources["SubheaderTextStyle"] as Style,
                Width = 500,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(10, 0, 0, 15),
                Text = content
            };

            StackPanel outsidePanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Background = new SolidColorBrush(Colors.Azure),
                Margin = new Thickness(0, 0, 0, 5)
            };
            outsidePanel.Children.Add(insidePanel);
            outsidePanel.Children.Add(titleTextBlock);
            outsidePanel.Children.Add(contentTextBlock);

            return outsidePanel;
        }

        #region Deal with star
        private void star_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            TextBlock targetTextBlock = sender as TextBlock;
            if (targetTextBlock.Name == "star1")
            {
                SetStarTextBlock(1);
            }
            else if (targetTextBlock.Name == "star2")
            {
                SetStarTextBlock(2);
            }
            else if (targetTextBlock.Name == "star3")
            {
                SetStarTextBlock(3);
            }
            else if (targetTextBlock.Name == "star4")
            {
                SetStarTextBlock(4);
            }
            else if (targetTextBlock.Name == "star5")
            {
                SetStarTextBlock(5);
            }
        }

        private void star_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            SetStarTextBlock(globalRate);
        }

        private void star_Tapped(object sender, TappedRoutedEventArgs e)
        {
            TextBlock targetTextBlock = sender as TextBlock;
            globalRate = targetTextBlock.Name[targetTextBlock.Name.Length - 1] - '0';
            System.Diagnostics.Debug.WriteLine(globalRate);
        }

        private void SetStarTextBlock(int num)
        {
            if (num == 0)
            {
                star1.Text = Constants.BlankStar;
                star2.Text = Constants.BlankStar;
                star3.Text = Constants.BlankStar;
                star4.Text = Constants.BlankStar;
                star5.Text = Constants.BlankStar;
            }
            else if (num == 1)
            {
                star1.Text = Constants.FillStar;
                star2.Text = Constants.BlankStar;
                star3.Text = Constants.BlankStar;
                star4.Text = Constants.BlankStar;
                star5.Text = Constants.BlankStar;
            }
            else if (num == 2)
            {
                star1.Text = Constants.FillStar;
                star2.Text = Constants.FillStar;
                star3.Text = Constants.BlankStar;
                star4.Text = Constants.BlankStar;
                star5.Text = Constants.BlankStar;
            }
            else if (num == 3)
            {
                star1.Text = Constants.FillStar;
                star2.Text = Constants.FillStar;
                star3.Text = Constants.FillStar;
                star4.Text = Constants.BlankStar;
                star5.Text = Constants.BlankStar;
            }
            else if (num == 4)
            {
                star1.Text = Constants.FillStar;
                star2.Text = Constants.FillStar;
                star3.Text = Constants.FillStar;
                star4.Text = Constants.FillStar;
                star5.Text = Constants.BlankStar;
            }
            else if (num == 5)
            {
                star1.Text = Constants.FillStar;
                star2.Text = Constants.FillStar;
                star3.Text = Constants.FillStar;
                star4.Text = Constants.FillStar;
                star5.Text = Constants.FillStar;
            }
        }
        #endregion
    }
}
