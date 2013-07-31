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
using Windows.UI.Core;
using Windows.UI.Popups;
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
        Course course;
        CloudEDUEntities ctx = null;
        DataServiceQuery<NOTE_SHAREABLE_AVAIL> sharedNoteDsq = null;
        DataServiceQuery<NOTE_AVAIL> mySharedNoteDsq = null;
        List<NOTE_SHAREABLE_AVAIL> sharedNotesList;
        List<NOTE_AVAIL> mySharedNotesList;

        public Note()
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
            course = e.Parameter as Course;

            
            sharedNoteDsq = (DataServiceQuery<NOTE_SHAREABLE_AVAIL>)(from selectNote in ctx.NOTE_SHAREABLE_AVAIL
                                                                     where selectNote.COURSE_ID == course.ID.Value
                                                                     orderby selectNote.DATE descending
                                                                     select selectNote);
            mySharedNoteDsq = (DataServiceQuery<NOTE_AVAIL>)(from myNote in ctx.NOTE_AVAIL
                                                              where myNote.COURSE_ID == course.ID.Value && myNote.CUSTOMER_ID == Constants.User.ID
                                                              orderby myNote.DATE descending
                                                              select myNote);
            switchButton.Content = "Mine";
            sharedNoteDsq.BeginExecute(OnSharedNoteComplete, null);
        }

        private async void OnSharedNoteComplete(IAsyncResult result)
        {
            try
            {
                sharedNotesList = sharedNoteDsq.EndExecute(result).ToList();

                foreach (var n in sharedNotesList)
                {
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            allSharedNotesStackPanel.Children.Add(GenerateSharedNoteItem(n.ID, n.TITLE, n.CONTENT, n.CUSTOMER_NAME, n.DATE, n.LESSON_NUMBER, n.CUSTOMER_ID.Value));
                        });
                }
            }
            catch
            {
                ShowMessageDialog("Network connection error!");
            }
        }

        private async void OnMySharedNoteComplete(IAsyncResult result)
        {
            try
            {
                mySharedNotesList = mySharedNoteDsq.EndExecute(result).ToList();

                foreach (var n in mySharedNotesList)
                {
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        myNotesStackPanel.Children.Add(GenerateMySharedNoteItem(n.ID, n.TITLE, n.CONTENT, n.CUSTOMER_NAME, n.DATE, n.LESSON_NUMBER));
                    });
                }
            }
            catch
            {
                ShowMessageDialog("Network connection error!");
            }
        }

        private async void ShowMessageDialog(string msg)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                 {
                     var messageDialog = new MessageDialog(msg);
                     messageDialog.Commands.Add(new UICommand("Close"));
                     await messageDialog.ShowAsync();
                 });
        }

        private Grid GenerateSharedNoteItem(int id, string title, string content, string user, DateTime time, int lessonNum, int customerID)
        {
            TextBlock noteInfo = new TextBlock
            {
                FontSize = 45,
                Height = 50,
                Margin = new Thickness(5, 0, 0, 0),
                Foreground = new SolidColorBrush(Colors.White),
                HorizontalAlignment = HorizontalAlignment.Left,
                Text = title + " By " + user + " At " + time.Year.ToString() + "." + time.Month.ToString() + "." + time.Day.ToString()
            };
            ToolTip toolTip = new ToolTip()
            {
                Content = content,
                FontSize = 30,
                MaxWidth = 200
            };
            ToolTipService.SetToolTip(noteInfo, toolTip);

            Image deleteImage = new Image();
            if (customerID != Constants.User.ID)
            {
                deleteImage = new Image
                {
                    Name = id.ToString(),
                    Source = new BitmapImage(new Uri("ms-appx:///Images/Coursing/Note/delete.png")),
                    Margin = new Thickness(4, 0, -45, 0),
                    Height = 40,
                    Width = 40,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    IsTapEnabled = true
                };
                deleteImage.Tapped += deleteImage_Tapped;
            }

            TextBlock lesson = new TextBlock
            {
                FontSize = 45,
                Height = 50,
                Margin = new Thickness(5, 0, 5, 0),
                Foreground = this.Resources["LessonForegroundBrush"] as SolidColorBrush,
                HorizontalAlignment = HorizontalAlignment.Right,
                Text = "L " + lessonNum
            };

            Grid newNote = new Grid()
            {
                Background = this.Resources["NoteBackgroundBrush"] as SolidColorBrush,
                Margin = new Thickness(2, 2, 50, 2)
            };
            newNote.Children.Add(noteInfo);
            newNote.Children.Add(deleteImage);
            newNote.Children.Add(lesson);

            return newNote;
        }

        public Grid GenerateMySharedNoteItem(int id, string title, string content, string user, DateTime time, int lessonNum)
        {
            TextBlock noteInfo = new TextBlock
            {
                FontSize = 45,
                Height = 50,
                Margin = new Thickness(5, 0, 0, 0),
                Foreground = new SolidColorBrush(Colors.White),
                HorizontalAlignment = HorizontalAlignment.Left,
                Text = title + " At " + time.Year.ToString() + "." + time.Month.ToString() + "." + time.Day.ToString()
            };
            ToolTip toolTip = new ToolTip()
            {
                Content = content,
                FontSize = 30,
                MaxWidth = 200
            };
            ToolTipService.SetToolTip(noteInfo, toolTip);

            Image deleteImage = new Image
            {
                Name = id.ToString(),
                Source = new BitmapImage(new Uri("ms-appx:///Images/Coursing/Note/delete.png")),
                Margin = new Thickness(4, 0, -45, 0),
                Height = 40,
                Width = 40,
                HorizontalAlignment = HorizontalAlignment.Right,
                IsTapEnabled = true
            };
            deleteImage.Tapped += deleteImage_Tapped;

            TextBlock lesson = new TextBlock
            {
                FontSize = 45,
                Height = 50,
                Margin = new Thickness(5, 0, 5, 0),
                Foreground = this.Resources["LessonForegroundBrush"] as SolidColorBrush,
                HorizontalAlignment = HorizontalAlignment.Right,
                Text = "L " + lessonNum
            };

            Grid newNote = new Grid()
            {
                Background = this.Resources["NoteBackgroundBrush"] as SolidColorBrush,
                Margin = new Thickness(2, 2, 50, 2)
            };
            newNote.Children.Add(noteInfo);
            newNote.Children.Add(deleteImage);
            newNote.Children.Add(lesson);

            return newNote;
        }

        void deleteImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Image toDelImage = sender as Image;

            ctx.BeginExecute<bool>(new Uri("/RemoveNote?id=" + Convert.ToInt32(toDelImage.Name), UriKind.Relative), OnDeleteNoteComplete, null);

            Grid toDelGrid = toDelImage.Parent as Grid;
            allSharedNotesStackPanel.Children.Remove(toDelGrid);
        }

        private void OnDeleteNoteComplete(IAsyncResult result)
        {
            try
            {
                bool res = ctx.EndExecute<bool>(result).FirstOrDefault();

                if (res)
                {
                    ShowMessageDialog("Delete sucessfully!");
                }
                else
                {
                    ShowMessageDialog("Delete error.");
                }

            }
            catch
            {
                ShowMessageDialog("Network connection error.");
            }
        }

        private void SwitchButton_Click(object sender, RoutedEventArgs e)
        {
            Button bt = sender as Button;
            if (bt.Content.ToString() == "Mine")
            {
                bt.Content = "Shared";
                myNotesStackPanel.Visibility = Visibility.Visible;
                allSharedNotesStackPanel.Visibility = Visibility.Collapsed;
                allSharedNotesStackPanel.Children.Clear();
                mySharedNoteDsq.BeginExecute(OnMySharedNoteComplete, null);
            }
            else if (bt.Content.ToString() == "Shared")
            {
                bt.Content = "Mine";
                myNotesStackPanel.Visibility = Visibility.Collapsed;
                allSharedNotesStackPanel.Visibility = Visibility.Visible;
                myNotesStackPanel.Children.Clear();
                sharedNoteDsq.BeginExecute(OnSharedNoteComplete, null);
            }
        }
    }
}
