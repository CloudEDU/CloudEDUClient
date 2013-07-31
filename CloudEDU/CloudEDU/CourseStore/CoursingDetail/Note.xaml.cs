using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CloudEDU.CourseStore.CoursingDetail
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Note : Page
    {
        public Note()
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
            Course course = e.Parameter as Course;
            this.allNoteStackPanel.Children.Add(GenerateNoteItem());
        }


        private Grid GenerateNoteItem()
        {
            TextBlock noteInfo = new TextBlock
            {
                FontSize = 45,
                Height = 50,
                Margin = new Thickness(5, 0, 0, 0),
                Foreground = new SolidColorBrush(Colors.White),
                HorizontalAlignment = HorizontalAlignment.Left,
                Text = "notexx By max 2013.7.31 10:44"
            };

            Image deleteImage = new Image
            {
                Source = new BitmapImage(new Uri("ms-appx:///Images/Coursing/Note/delete.png")),
                Margin = new Thickness(4, 0, -45, 0),
            };
            deleteImage.Height = 40;
            deleteImage.Width = 40;
            deleteImage.HorizontalAlignment = HorizontalAlignment.Right;

            TextBlock lessonNum = new TextBlock
            {
                FontSize = 45,
                Height = 50,
                Margin = new Thickness(5, 0, 5, 0),
                Foreground = this.Resources["LessonForegroundBrush"] as SolidColorBrush,
                HorizontalAlignment = HorizontalAlignment.Right,
                Text = "Lesson 3"
            };

            Grid newNote = new Grid()
            {
                Background = this.Resources["NoteBackgroundBrush"] as SolidColorBrush,
                Margin = new Thickness(2, 2, 50, 2)
            };
            newNote.Children.Add(noteInfo);
            newNote.Children.Add(deleteImage);
            newNote.Children.Add(lessonNum);

            return newNote;
        }
    }
}
