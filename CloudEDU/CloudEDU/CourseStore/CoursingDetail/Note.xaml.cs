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
        DataServiceQuery<NOTE_AVAIL> sharedNoteDsq = null;
        DataServiceQuery<NOTE_AVAIL> myNoteDsq = null;
        List<NOTE_AVAIL> sharedNotesList;
        List<NOTE_AVAIL> mySharedNotesList;
        NOTE_AVAIL changedNote = null;
        int changedLessonID;

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
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            course = e.Parameter as Course;


            sharedNoteDsq = (DataServiceQuery<NOTE_AVAIL>)(from selectNote in ctx.NOTE_AVAIL
                                                           where selectNote.COURSE_ID == course.ID.Value && selectNote.SHARE == true
                                                           orderby selectNote.DATE descending
                                                           select selectNote);
            switchButton.Content = "Mine";
            TaskFactory<IEnumerable<NOTE_AVAIL>> tf = new TaskFactory<IEnumerable<NOTE_AVAIL>>();
            IEnumerable<NOTE_AVAIL> nas = await tf.FromAsync(sharedNoteDsq.BeginExecute(null, null), iar => sharedNoteDsq.EndExecute(iar));
            sharedNotesList = nas.ToList();
            foreach (var n in sharedNotesList)
            {
                allSharedNotesStackPanel.Children.Add(GenerateSharedNoteItem(n.ID, n.TITLE, n.CONTENT, n.CUSTOMER_NAME, n.DATE, n.LESSON_NUMBER, n.CUSTOMER_ID.Value));
            }
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
                ShowMessageDialog("Network connection error27!");
            }
        }

        private async void OnMyNoteComplete(IAsyncResult result)
        {
            try
            {
                mySharedNotesList = myNoteDsq.EndExecute(result).ToList();

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
                ShowMessageDialog("Network connection error!21");
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

            Image deleteImage = new Image
                {
                    Tag = id.ToString(),
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

        public Grid GenerateMySharedNoteItem(int id, string title, string content, string user, DateTime time, int lessonNum)
        {
            TextBlock noteInfo = new TextBlock
            {
                Tag = id,
                FontSize = 45,
                Height = 50,
                Margin = new Thickness(5, 0, 0, 0),
                Foreground = new SolidColorBrush(Colors.White),
                HorizontalAlignment = HorizontalAlignment.Left,
                Text = title + " At " + time.Year.ToString() + "." + time.Month.ToString() + "." + time.Day.ToString(),
                IsTapEnabled = true
            };
            noteInfo.Tapped += noteInfo_Tapped;
            ToolTip toolTip = new ToolTip()
            {
                Content = content,
                FontSize = 30,
                MaxWidth = 200
            };
            ToolTipService.SetToolTip(noteInfo, toolTip);

            Image deleteImage = new Image
            {
                Tag = id.ToString(),
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

            ctx.BeginExecute<bool>(new Uri("/RemoveNote?id=" + Convert.ToInt32(toDelImage.Tag), UriKind.Relative), OnDeleteNoteComplete, null);

            Grid toDelGrid = toDelImage.Parent as Grid;
            myNotesStackPanel.Children.Remove(toDelGrid);
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
                    ShowMessageDialog("Delete error.22");
                }

            }
            catch
            {
                ShowMessageDialog("Network connection error.23");
            }
        }

        private async void SwitchButton_Click(object sender, RoutedEventArgs e)
        {
            Button bt = sender as Button;
            sharedNoteDsq = (DataServiceQuery<NOTE_AVAIL>)(from selectNote in ctx.NOTE_AVAIL
                                                           where selectNote.COURSE_ID == course.ID.Value && selectNote.SHARE == true
                                                           orderby selectNote.DATE descending
                                                           select selectNote);
            myNoteDsq = (DataServiceQuery<NOTE_AVAIL>)(from myNote in ctx.NOTE_AVAIL
                                                       where myNote.COURSE_ID == course.ID.Value && myNote.CUSTOMER_ID == Constants.User.ID
                                                       orderby myNote.DATE descending
                                                       select myNote);

            try
            {
                if (bt.Content.ToString() == "Mine")
                {
                    bt.Content = "Shared";
                    myNotesStackPanel.Visibility = Visibility.Visible;
                    allSharedNotesStackPanel.Visibility = Visibility.Collapsed;
                    myNotesStackPanel.Children.Clear();
                    TaskFactory<IEnumerable<NOTE_AVAIL>> tf = new TaskFactory<IEnumerable<NOTE_AVAIL>>();
                    IEnumerable<NOTE_AVAIL> nas = await tf.FromAsync(myNoteDsq.BeginExecute(null, null), iar => myNoteDsq.EndExecute(iar));
                    mySharedNotesList = nas.ToList();
                    foreach (var n in mySharedNotesList)
                    {
                        myNotesStackPanel.Children.Add(GenerateMySharedNoteItem(n.ID, n.TITLE, n.CONTENT, n.CUSTOMER_NAME, n.DATE, n.LESSON_NUMBER));
                    }
                }
                else if (bt.Content.ToString() == "Shared")
                {
                    bt.Content = "Mine";
                    myNotesStackPanel.Visibility = Visibility.Collapsed;
                    allSharedNotesStackPanel.Visibility = Visibility.Visible;
                    allSharedNotesStackPanel.Children.Clear();
                    TaskFactory<IEnumerable<NOTE_AVAIL>> tf = new TaskFactory<IEnumerable<NOTE_AVAIL>>();
                    IEnumerable<NOTE_AVAIL> nas = await tf.FromAsync(sharedNoteDsq.BeginExecute(null, null), iar => sharedNoteDsq.EndExecute(iar));
                    sharedNotesList = nas.ToList();
                    foreach (var n in sharedNotesList)
                    {
                        allSharedNotesStackPanel.Children.Add(GenerateSharedNoteItem(n.ID, n.TITLE, n.CONTENT, n.CUSTOMER_NAME, n.DATE, n.LESSON_NUMBER, n.CUSTOMER_ID.Value));
                    }
                }
            }
            catch
            {
                ShowMessageDialog("Network connection error38!");
            }
        }

        private void CancelUploadButton_Click(object sender, RoutedEventArgs e)
        {
            addNotePopup.IsOpen = false;
            noteTitle.Text = "";
            noteContent.Text = "";
            selectLessonComboBox.SelectedIndex = 0;
            sharableCheckBox.IsChecked = false;
        }

        private async void SaveNoteButton_Click(object sender, RoutedEventArgs e)
        {
            NOTE updatedNote = null;
            try
            {
                DataServiceQuery<NOTE> naDsq = (DataServiceQuery<NOTE>)(from selNote in ctx.NOTE
                                                                        where selNote.ID == changedNote.ID
                                                                        select selNote);

                TaskFactory<IEnumerable<NOTE>> changeNote = new TaskFactory<IEnumerable<NOTE>>();
                updatedNote = (await changeNote.FromAsync(naDsq.BeginExecute(null, null), iar => naDsq.EndExecute(iar))).FirstOrDefault();
            }
            catch
            {
                ShowMessageDialog("Network connection error!24");
                return;
            }

            updatedNote.TITLE = noteTitle.Text;
            updatedNote.CONTENT = noteContent.Text;
            updatedNote.LESSON_ID = changedLessonID;
            updatedNote.DATE = DateTime.Now;
            updatedNote.SHARE = sharableCheckBox.IsChecked ?? false;

            try
            {
                ctx.UpdateObject(updatedNote);
                TaskFactory<DataServiceResponse> tf = new TaskFactory<DataServiceResponse>();
                await tf.FromAsync(ctx.BeginSaveChanges(null, null), iar => ctx.EndSaveChanges(iar));
            }
            catch
            {
                ShowMessageDialog("Update error25! Please check your network.");
                addNotePopup.IsOpen = false;
                return;
            }

            ShowMessageDialog("Update successfully!");
            addNotePopup.IsOpen = false;
        }

        private async void noteInfo_Tapped(object sender, TappedRoutedEventArgs e)
        {
            TextBlock tb = sender as TextBlock;
            int noteId = (int)tb.Tag;
           
            IEnumerable<LESSON> allLessons = null;
            try
            {
                DataServiceQuery<NOTE_AVAIL> naDsq = (DataServiceQuery<NOTE_AVAIL>)(from selNote in ctx.NOTE_AVAIL
                                                                                    where selNote.ID == noteId
                                                                                    select selNote);

                TaskFactory<IEnumerable<NOTE_AVAIL>> changeNote = new TaskFactory<IEnumerable<NOTE_AVAIL>>();
                changedNote = (await changeNote.FromAsync(naDsq.BeginExecute(null, null), iar => naDsq.EndExecute(iar))).FirstOrDefault();

                DataServiceQuery<LESSON> lessonDsq = (DataServiceQuery<LESSON>)(from selLesson in ctx.LESSON
                                                                                where selLesson.COURSE_ID == course.ID
                                                                                select selLesson);
                TaskFactory<IEnumerable<LESSON>> lessonTf = new TaskFactory<IEnumerable<LESSON>>();
                allLessons = await lessonTf.FromAsync(lessonDsq.BeginExecute(null, null), res => lessonDsq.EndExecute(res));
            }
            catch
            {
                ShowMessageDialog("Seach failed or Network connection error26!");
                return;
            }
            List<string> allLessonString = new List<string>();
            foreach (var l in allLessons)
            {
                allLessonString.Add(l.TITLE);
            }

            addNotePopup.IsOpen = true;
            noteTitle.Text = changedNote.TITLE;
            noteContent.Text = changedNote.CONTENT;
            selectLessonComboBox.ItemsSource = allLessonString;
            selectLessonComboBox.SelectedIndex = changedNote.LESSON_NUMBER - 1;
            sharableCheckBox.IsChecked = changedNote.SHARE;

            changedLessonID = changedNote.LESSON_ID;
        }
    }
}
