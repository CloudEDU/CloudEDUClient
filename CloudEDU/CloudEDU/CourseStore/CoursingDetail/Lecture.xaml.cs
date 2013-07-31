using CloudEDU.Common;
using CloudEDU.Service;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.Storage.Search;
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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace CloudEDU.CourseStore.CoursingDetail
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Lecture : Page
    {

        DBAccessAPIs dba = null;
        List<LESSON> lessons = null;
        Dictionary<string, int> resourceDic = null;
        Dictionary<Image, string> Res_URL = null;
        /// <summary>
        /// Constructor, initilize the components.
        /// </summary>
        public Lecture()
        {
            this.InitializeComponent();
            ctx = new CloudEDUEntities(new Uri(Constants.DataServiceURI));
            dba = new DBAccessAPIs();
            lessons = new List<LESSON>();
            Res_URL = new Dictionary<Image, string>();
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。Parameter
        /// 属性通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Course course = e.Parameter as Course;
            dba = new DBAccessAPIs();
            dba.GetLessonsByCourseId((int)course.ID, onGetLessonComplete);
            allLessonsStackPanel.Children.RemoveAt(0);
            allLessonsStackPanel.Children.RemoveAt(0);
            
            //allLessonsStackPanel.Children.Add(GenerateALessonBox(null));
            //allLessonsStackPanel.Children.Add(GenerateALessonBox(null));
        }


        private async void onGetLessonComplete(IAsyncResult iar)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("get lesson complete");
                IEnumerable<LESSON> lessons = dba.lessonDsq.EndExecute(iar);
                foreach (var l in lessons)
                {
                    this.lessons.Add(l);
                    //coursesData.AddCourse(Constants.CourseAvail2Course(c));

                }
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    foreach (var l in this.lessons)
                    {
                        System.Diagnostics.Debug.WriteLine(l.TITLE);
                        allLessonsStackPanel.Children.Add(GenerateALessonBox(l));
                    }
                    //dataCategory = coursesData.GetGroupsByCategory();
                    //cvs1.Source = dataCategory;
                    //(SemanticZoom.ZoomedOutView as ListViewBase).ItemsSource = cvs1.View.CollectionGroups;
                    //loadingProgressRing.IsActive = false;
                });
            }
            catch
            {
                //ShowMessageDialog();
                // Network Connection error.
            }
            try
            {
                resourceDic = new Dictionary<string, int>(Constants.ResourceType.Count);
                for (int i = 0; i <Constants.ResourceType.Count; ++i)
                {
                    DataServiceQuery<RES_TYPE> dps = (DataServiceQuery<RES_TYPE>)(from res_type in ctx.RES_TYPE
                                                                                  where res_type.DESCRIPTION.Trim() == Constants.ResourceType[i]
                                                                                  select res_type);
                    TaskFactory<IEnumerable<RES_TYPE>> tf = new TaskFactory<IEnumerable<RES_TYPE>>();
                    RES_TYPE resID = (await tf.FromAsync(dps.BeginExecute(null, null), result => dps.EndExecute(result))).FirstOrDefault();

                    resourceDic.Add(Constants.ResourceType[i], resID.ID);
                }
            }
            catch
            {
                ShowMessageDialog("Network connection error!");
                return;
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



        private Image GenerateDocImage(string url)
        {
            return GenerateImage("ms-appx:///Images/Upload/doc_white.png",url);
        }

        private Image GenerateAudioImage(string url)
        {
            return GenerateImage("ms-appx:///Images/Upload/audio_white.png",url);
        }

        private Image GenerateVideoImage(string url)
        {
            return GenerateImage("ms-appx:///Images/Upload/video_white.png",url);
        }

        private Image GenerateImage(string uri,string url)
        {
            Image image = new Image
            {
                Source = new BitmapImage(new Uri(uri)),
                Margin = new Thickness(4, 0, 4, 0),
                Width = 40,
                Height = 40,
                HorizontalAlignment = HorizontalAlignment.Right,
            };
            Res_URL.Add(image,url);

            return image;
        }

        
        private Grid GenerateALessonBox(LESSON les)
        {
            if (les == null)
            {
                return null;
            }
            TextBlock lessonName = new TextBlock
            {
                FontSize = 50,
                Height = 65,
                Margin = new Thickness(5, 0, 0, 0),
                Foreground = new SolidColorBrush(Colors.White),
                HorizontalAlignment = HorizontalAlignment.Left,
                Text = les.TITLE,
                
            };
            /*
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
            */
            StackPanel imagesStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
            };
            //imagesStackPanel.Children.Add(docImage);
            //imagesStackPanel.Children.Add(audioImage);
            //imagesStackPanel.Children.Add(videoImage);

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
                Margin = new Thickness(2, 2, 80, 2)
            };
            newLesson.Children.Add(lessonName);
            newLesson.Children.Add(imagesStackPanel);
            newLesson.Children.Add(noteImage);

            noteImage.Tapped += noteImage_Tapped;

            AddImageButton(les.ID, imagesStackPanel);
            return newLesson;
        }

        private async void AddImageButton(int lessonId, StackPanel parent)
        {
            System.Diagnostics.Debug.WriteLine("AddImageButton!");
            DataServiceQuery<RESOURCE> dps = (DataServiceQuery<RESOURCE>)(ctx.RESOURCE.Where(r => r.LESSON_ID == lessonId));
            TaskFactory<IEnumerable<RESOURCE>> tf = new TaskFactory<IEnumerable<RESOURCE>>();
            IEnumerable<RESOURCE> resources = (await tf.FromAsync(dps.BeginExecute(null, null), iar => dps.EndExecute(iar)));
            foreach (var r in resources)
            {
                System.Diagnostics.Debug.WriteLine(r.URL);
                Image image = null;
                if (r.TYPE == 2)
                {
                    image = GenerateDocImage(r.URL);
                    parent.Children.Add(image);
                }
                else if (r.TYPE == 1)
                {
                    image = GenerateAudioImage(r.URL);
                    parent.Children.Add(image);
                }
                else if (r.TYPE == 3)
                {
                    image = GenerateAudioImage(r.URL);
                    parent.Children.Add(image);
                }
                image.Tapped += ResImageTapped;
            }
        }

        //string fileToLaunch = null;
        private async void LaunchFileOpenWith(string fileName)
        {

            var file = await KnownFolders.VideosLibrary.GetFileAsync(fileName);
            // First, get the image file from the package's image directory.
            //var file = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(fileToLaunch);

            // Calculate the position for the Open With dialog.
            // An alternative to using the point is to set the rect of the UI element that triggered the launch.
            Point openWithPosition = new Point(600, 500);

            // Next, configure the Open With dialog.
            var options = new Windows.System.LauncherOptions();
            options.DisplayApplicationPicker = true;
            options.UI.InvocationPoint = openWithPosition;
            options.UI.PreferredPlacement = Windows.UI.Popups.Placement.Below;

            // Finally, launch the file.
            bool success = await Windows.System.Launcher.LaunchFileAsync(file, options);
            if (success)
            {
                ShowMessageDialog("File launched: " + file.Name);
            }
            else
            {
                ShowMessageDialog("File launch failed.");
            }
        }




        private async void ResImageTapped(object sender, TappedRoutedEventArgs e)
        {
            Image image = (Image)sender;
            string url = Res_URL[image];
            System.Diagnostics.Debug.WriteLine(url);
            Uri uri = new Uri(Constants.BaseURI+url);
            string[] fileArray = url.Split('\\');
            

            string fileName = fileArray[fileArray.Length - 1];
            System.Diagnostics.Debug.WriteLine(fileName);


            if ((await StorageFolderExtension.CheckFileExisted(KnownFolders.VideosLibrary, fileName)))
            {
                LaunchFileOpenWith(fileName);
                //System.Diagnostics.Debug.WriteLine("somepath"+KnownFolders.VideosLibrary.Path+"asdfa");

                //ShowMessageDialog("file already exist!!");
                return;
            }



            StorageFile destinationFile;
            try
            {
                destinationFile = await KnownFolders.VideosLibrary.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            }
            catch (FileNotFoundException ex)
            {
                //rootPage.NotifyUser("Error while creating file: " + ex.Message, NotifyType.ErrorMessage);
                return;
            }

            BackgroundDownloader downloader = new BackgroundDownloader();
            DownloadOperation download = downloader.CreateDownload(uri, destinationFile);

            await HandleDownloadAsync(download, true);

        }

        private CancellationTokenSource cts;


        private async Task HandleDownloadAsync(DownloadOperation download, bool start)
        {
            try
            {
                //LogStatus("Running: " + download.Guid, NotifyType.StatusMessage);

                // Store the download so we can pause/resume.
                //activeDownloads.Add(download);

                cts = new CancellationTokenSource();

                //Progress<DownloadOperation> progressCallback = new Progress<DownloadOperation>(DownloadProgress);
                if (start)
                {
                    // Start the download and attach a progress handler.
                    await download.StartAsync().AsTask(cts.Token, null);
                }
                else
                {
                    // The download was already running when the application started, re-attach the progress handler.
                    await download.AttachAsync().AsTask(cts.Token, null);
                }
                ShowMessageDialog("Download Complete!");
                ResponseInformation response = download.GetResponseInformation();
                
                //LogStatus(String.Format("Completed: {0}, Status Code: {1}", download.Guid, response.StatusCode),
                    //NotifyType.StatusMessage);
            }
            catch (TaskCanceledException e)
            {
                
                //LogStatus("Canceled: " + download.Guid, NotifyType.StatusMessage);
            }
            catch (Exception ex)
            {
                ShowMessageDialog("Execution error!");
                //if (!IsExceptionHandled("Execution error", ex, download))
                //{
                   // throw;
                //}
            }
            finally
            {
                //activeDownloads.Remove(download);
            }
            
        }




        private void noteImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.addNotePopup.IsOpen = true;
            List<string> list= new List<string>();
            for (int i = 0; i < lessons.Count; i++)
            {
                list.Add("Lesson " + (i+1));
            }
            this.selectLessonComboBox.ItemsSource = list;
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
            note.LESSON_ID = lessons[selectLessonComboBox.SelectedIndex].ID;
            note.CUSTOMER_ID = Constants.User.ID;

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
