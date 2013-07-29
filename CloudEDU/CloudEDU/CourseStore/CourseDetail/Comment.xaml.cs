using System;
using System.Collections.Generic;
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
        }

        private void AddCommentButton_Click(object sender, RoutedEventArgs e)
        {
            StackPanel newComment = GenerateACommentBox();
            commentsStackPanel.Children.Add(newComment);
        }

        private StackPanel GenerateACommentBox()
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
                Text = "&#x2605;&#x2605;&#x2605;&#x2605;"
            };

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
                Text = "It's awesome!"
            };

            TextBlock contentTextBlock = new TextBlock
            {
                Style = Application.Current.Resources["SubheaderTextStyle"] as Style,
                Width = 500,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(10, 0, 0, 15),
                Text = "klasfjklasjfwqpojfojpqojpfwojp qwojpf jowqf jpof ojpwqopfj wqjopfw ojpfwq ojw fqojwf qpfow pqjofpw qjo pqfw"
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
    }
}
