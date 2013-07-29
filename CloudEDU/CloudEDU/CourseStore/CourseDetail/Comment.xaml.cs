using CloudEDU.Common;
using CloudEDU.Service;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
        private int globalRate;

        private DataServiceQuery<COMMENT> dps;

        public Comment()
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
            string courseName = e.Parameter as string;
            globalRate = 0;

            //var query = from comment in DataServiceContextSingleton.SharedDataServiceContext().COMMENTs select comment;

            //COMMENT comment1 = new COMMENT();
            //comment1.CONTENT = "sadfasdf";
            //comment1.COURSE_ID = 1;
            //comment1.CUSTOMER_ID = 1;

            DataServiceContextSingleton.SharedDataServiceContext().BeginExecute<COMMENT>(new Uri("C", UriKind.Relative), OnComplete, null);


            //DataServiceContextSingleton.SharedDataServiceContext().AddToCOMMENTs(comment1);
            //DataServiceContextSingleton.SharedDataServiceContext().BeginSaveChanges(OnComplete, null);
            //dps = (DataServiceQuery<COMMENT>)(query);
            //dps.BeginExecute(OnComplete, query);
        }

        private void OnComplete(IAsyncResult result)
        {
            //foreach (var c in dps.EndExecute(result))
            //{
            //    System.Diagnostics.Debug.WriteLine(c.CONTENT);
            //}
            System.Diagnostics.Debug.WriteLine(result.AsyncState);
            System.Diagnostics.Debug.WriteLine(result.CompletedSynchronously);
            System.Diagnostics.Debug.WriteLine(result.IsCompleted);
            System.Diagnostics.Debug.WriteLine(result.GetType());
        }

        /// <summary>
        /// Invoked when Add Comment button clicked and add comment.
        /// </summary>
        /// <param name="sender">The Add Comment button clicked.</param>
        /// <param name="e"></param>
        private void AddCommentButton_Click(object sender, RoutedEventArgs e)
        {
            StackPanel newComment = GenerateACommentBox(newTitleTextBox.Text, globalRate, newContentTextBox.Text);
            commentsStackPanel.Children.Add(newComment);

            newTitleTextBox.Text = newContentTextBox.Text = "";
            globalRate = 0;
            SetStarTextBlock(globalRate);
        }

        private StackPanel GenerateACommentBox(string title, int rate, string content)
        {
            TextBlock userTextBlock = new TextBlock
            {
                Style = Application.Current.Resources["SubheaderTextStyle"] as Style,
                FontWeight = FontWeights.Bold,
                Text = "Boyi"
            };
            TextBlock dateTextBlock = new TextBlock
            {
                Style = Application.Current.Resources["SubheaderTextStyle"] as Style,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(40, 0, 0, 0),
                Text = "2013.7.03"
            };
            TextBlock timeTextBlock = new TextBlock
            {
                Style = Application.Current.Resources["SubheaderTextStyle"] as Style,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(20, 0, 0, 0),
                Text = "14:23"
            };
            TextBlock rateTextBlock = new TextBlock
            {
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(30, 0, 0, 0),
                Text = ""
            };
            for (int i = 0; i < rate; ++i)
            {
                rateTextBlock.Text += "$#x2605;";
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
                Background = new SolidColorBrush(Colors.Bisque),
                Margin = new Thickness(0, 0, 0 ,5)
            };
            outsidePanel.Children.Add(insidePanel);
            outsidePanel.Children.Add(titleTextBlock);
            outsidePanel.Children.Add(contentTextBlock);

            return outsidePanel;
        }

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
                star1.Text = "$#x2606;";
                star2.Text = "$#x2606;";
                star3.Text = "$#x2606;";
                star4.Text = "$#x2606;";
                star5.Text = "$#x2606;";
            }
            else if (num == 1)
            {
                star1.Text = "$#x2605;";
                star2.Text = "$#x2606;";
                star3.Text = "$#x2606;";
                star4.Text = "$#x2606;";
                star5.Text = "$#x2606;";
            }
            else if (num == 2)
            {
                star1.Text = "$#x2605;";
                star2.Text = "$#x2605;";
                star3.Text = "$#x2606;";
                star4.Text = "$#x2606;";
                star5.Text = "$#x2606;";
            }
            else if (num == 3)
            {
                star1.Text = "$#x2605;";
                star2.Text = "$#x2605;";
                star3.Text = "$#x2605;";
                star4.Text = "$#x2606;";
                star5.Text = "$#x2606;";
            }
            else if (num == 4)
            {
                star1.Text = "$#x2605;";
                star2.Text = "$#x2605;";
                star3.Text = "$#x2605;";
                star4.Text = "$#x2605;";
                star5.Text = "$#x2606;";
            }
            else if (num == 5)
            {
                star1.Text = "$#x2605;";
                star2.Text = "$#x2605;";
                star3.Text = "$#x2605;";
                star4.Text = "$#x2605;";
                star5.Text = "$#x2605;";
            }
        }
    }
}
