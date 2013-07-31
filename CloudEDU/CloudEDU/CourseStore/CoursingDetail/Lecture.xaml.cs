using CloudEDU.Common;
using CloudEDU.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace CloudEDU.CourseStore.CoursingDetail
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Lecture : Page
    {
        /// <summary>
        /// Constructor, initilize the components.
        /// </summary>
        public Lecture()
        {
            this.InitializeComponent();
            ctx = new CloudEDUEntities(new Uri(Constants.DataServiceURI));
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。Parameter
        /// 属性通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Course course = e.Parameter as Course;

            allLessonsStackPanel.Children.Add(GenerateALessonBox());
            allLessonsStackPanel.Children.Add(GenerateALessonBox());
        }

        private Grid GenerateALessonBox()
        {
            TextBlock lessonName = new TextBlock
            {
                FontSize = 50,
                Height = 65,
                Margin = new Thickness(5, 0, 0, 0),
                Foreground = new SolidColorBrush(Colors.White),
                HorizontalAlignment = HorizontalAlignment.Left,
                Text = "Lesson 1 xxxxx"
            };

            Image docImage = new Image
            {
                Source = new BitmapImage(new Uri("ms-appx:///Images/Upload/doc_white.png")),
                Margin = new Thickness(4, 0, 4, 0),
            };
            Image audioImage = new Image
            {
                Source = new BitmapImage(new Uri("ms-appx:///Images/Upload/audio_white.png")),
                Margin = new Thickness(4, 0, 4, 0)
            };
            Image videoImage = new Image
            {
                Source = new BitmapImage(new Uri("ms-appx:///Images/Upload/video_white.png")),
                Margin = new Thickness(8, 0, 8, 0)
            };

            StackPanel imagesStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right
            };
            imagesStackPanel.Children.Add(docImage);
            imagesStackPanel.Children.Add(audioImage);
            imagesStackPanel.Children.Add(videoImage);

            Image noteImage = new Image
            {
                Source = new BitmapImage(new Uri("ms-appx:///Images/Coursing/Lectures/newnote.png")),
                Margin = new Thickness(4, 0, -40, 0)
            };
            noteImage.Height = 30;
            noteImage.Width = 30;
            noteImage.HorizontalAlignment = HorizontalAlignment.Right;
            Grid newLesson = new Grid()
            {
                Background = this.Resources["LessonBackgroundBrush"] as SolidColorBrush,
                Margin = new Thickness(2, 2, 50, 2)
            };
            newLesson.Children.Add(lessonName);
            newLesson.Children.Add(imagesStackPanel);
            newLesson.Children.Add(noteImage);

            noteImage.Tapped += noteImage_Tapped;

            return newLesson;
        }

        private void noteImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.addNotePopup.IsOpen = true;
            //throw new NotImplementedException();
        }

        private void CancelUploadButton_Click(object sender, RoutedEventArgs e)
        {
            this.addNotePopup.IsOpen = false;
        }

        CloudEDUEntities ctx = null;

        private async void SaveNoteButton_Click(object sender, RoutedEventArgs e)
        {
            NOTE note = new NOTE();
            note.TITLE = this.noteTitle.Text;
            note.CONTENT = this.noteContent.Text;
            note.LESSON_ID = 1;
            note.CUSTOMER_ID = 1;

            if (note == null)
            {
                System.Diagnostics.Debug.WriteLine("note is null!");
            }

            ctx.AddToNOTE(note);
            ctx.BeginSaveChanges(onNoteSaved, null);

            this.addNotePopup.IsOpen = false;
            ClearNote();
            MessageDialog md = new MessageDialog("Note Saved", "Your note have been saved!");
            await md.ShowAsync();
            //md.Content = "Your note have been saved!";



        }
        private void onNoteSaved(IAsyncResult iar)
        {
            ctx.EndSaveChanges(iar);
        }
        private void ClearNote()
        {
            this.noteTitle.Text = "Title";
            this.noteContent.Text = "Note Content...";
            this.selectLessonComboBox.SelectedIndex = 0;

        }




        private void noteContent_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.noteContent.Text.Equals("Note Content..."))
            {
                this.noteContent.Text = "";
            }
            //System.Diagnostics.Debug.WriteLine("noteContent tapped");

        }

        private void noteTitle_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.noteTitle.Text.Equals("Title"))
            {
                this.noteTitle.Text = "";
            }
            //System.Diagnostics.Debug.WriteLine("notetitle tapped");

        }
    }
}
