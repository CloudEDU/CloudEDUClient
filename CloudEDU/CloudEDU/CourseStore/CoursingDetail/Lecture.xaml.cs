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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace CloudEDU.CourseStore.CoursingDetail
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Lecture : Page
    {
        public Lecture()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。Parameter
        /// 属性通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
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

            Grid newLesson = new Grid()
            {
                Background = this.Resources["LessonBackgroundBrush"] as SolidColorBrush,
                Margin = new Thickness(2)
            };
            newLesson.Children.Add(lessonName);
            newLesson.Children.Add(imagesStackPanel);

            return newLesson;
        }
    }
}
