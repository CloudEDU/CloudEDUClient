﻿using CloudEDU.Common;
using CloudEDU.CourseStore;
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
using Windows.Security.Credentials.UI;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CloudEDU
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Uploading : GlobalPage
    {
        private List<CATEGORY> categories;
        private List<PARENT_GUIDE> pgs;

        private CloudEDUEntities ctx = null;
        private DataServiceQuery<CATEGORY> categoryDsq = null;
        private DataServiceQuery<PARENT_GUIDE> pgDsq = null;

        List<string> imagesFilterTypeList = new List<string> { ".png", ".jpg", ".bmp" };
        List<string> docsFilterTypeList = new List<string> { ".doc", ".docx", ".pdf" };
        List<string> audiosFilterTypeList = new List<string> { ".mp3", ".wmv" };
        List<string> videosFilterTypeList = new List<string> { ".mp4", ".avi", ".rm", ".rmvb" };

        IReadOnlyList<StorageFile> images = null;
        IReadOnlyList<StorageFile> docs = null;
        IReadOnlyList<StorageFile> audios = null;
        IReadOnlyList<StorageFile> videos = null;

        private List<Lesson> allLessons;
        private Course toBeUploadCourse;
     
        //private Button addImageButton;

        private CancellationTokenSource cts;

        int lessonCount = 0;

        /// <summary>
        /// Constructor, initialize the components.
        /// </summary>
        public Uploading()
        {
            this.InitializeComponent();

            ctx = new CloudEDUEntities(new Uri(Constants.DataServiceURI));
            //addImageButton = imageAddButton;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ResetPage();
        }

        #region Query Callback methods
        /// <summary>
        /// DataServiceQuery callback method to refresh the UI.
        /// </summary>
        /// <param name="result">Async operation result.</param>
        private async void OnCategoryComplete(IAsyncResult result)
        {
            try
            {
                IEnumerable<CATEGORY> cts = categoryDsq.EndExecute(result);
                categories = new List<CATEGORY>(cts);
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    categoryComboBox.ItemsSource = categories;
                    categoryComboBox.SelectedIndex = 0;
                });
            }
            catch
            {
                ShowNetworkMessageDialog();
            }
        }

        private async void OnPGComplete(IAsyncResult result)
        {
            try
            {
                IEnumerable<PARENT_GUIDE> ps = pgDsq.EndExecute(result);
                pgs = new List<PARENT_GUIDE>(ps);
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        pgComboBox.ItemsSource = pgs;
                        pgComboBox.SelectedIndex = 0;
                    });
            }
            catch
            {
                
            }
        }
        #endregion

        #region Button Click Action
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
        /// Invoked when add button is clicked and prepare to add new lesson.
        /// </summary>
        /// <param name="sender">The add button clicked.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>
        private void AddLessonButton_Click(object sender, RoutedEventArgs e)
        {
            wholeFrame.Opacity = 0.5;
            addLessonPopup.IsOpen = true;

            ResetPopup();

            // Reset button visual state
            VisualStateManager.GoToState(UploadLessionButton, "Normal", false);
            VisualStateManager.GoToState(CancelUploadButton, "Normal", false);
        }

        /// <summary>
        /// Invoked when upload button in popup is clicked and add new lesson.
        /// </summary>
        /// <param name="sender">The upload button clicked.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>
        private async void UploadLessionButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckLessonInfomation())
            {
                ShowMessageDialog("Format error! Please check your upload infomation.");
                return;
            }
            allLessons.Add(new Lesson(++lessonCount, lessonName.Text, lessonDescription.Text));

            List<BackgroundTransferContentPart> docParts = CreateBackgroundTransferContentPartList(docs);
            List<BackgroundTransferContentPart> audioParts = CreateBackgroundTransferContentPartList(audios);
            List<BackgroundTransferContentPart> videoParts = CreateBackgroundTransferContentPartList(videos);

            List<BackgroundTransferContentPart> allParts = new List<BackgroundTransferContentPart>();
            if (docParts != null) allParts.AddRange(docParts);
            if (audioParts != null) allParts.AddRange(audioParts);
            if (videoParts != null) allParts.AddRange(videoParts);

            Uri uploadUri = new Uri("http://10.0.1.65/Upload/Upload.aspx?username=" + Constants.User.NAME.Trim());

            BackgroundUploader uploader = new BackgroundUploader();
            if (allParts.Count != 0)
            {
                UploadOperation uploadOperation = await uploader.CreateUploadAsync(uploadUri, allParts);
                // Attach progress and completion handlers.
                await HandleUploadAsync(uploadOperation, true);
            }

            AddLessonInfo();
            wholeFrame.Opacity = 1;
            addLessonPopup.IsOpen = false;
        }

        /// <summary>
        /// Invoked when cancel button in popup is clicked and cancel to upload new lesson.
        /// </summary>
        /// <param name="sender">The cancel button clicked.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>
        private void CancelUploadButton_Click(object sender, RoutedEventArgs e)
        {
            wholeFrame.Opacity = 1;
            addLessonPopup.IsOpen = false;
        }

        /// <summary>
        /// Invoked when image add button is clicked and choose the image.
        /// </summary>
        /// <param name="sender">The image add button clicked.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>
        private async void ImageUploadButton_Click(object sender, RoutedEventArgs e)
        {
            //FileOpenPicker picker = FileTypePicker(imagesFilterTypeList);
            //if (picker == null) return;

            //images = await picker.PickMultipleFilesAsync();
            //if (images == null || images.Count == 0) return;

            //Image imgImg = new Image
            //{
            //    Source = new BitmapImage(new Uri("ms-appx:///Images/Upload/image.png")),
            //    Margin = new Thickness(5, 0, 5, 0)
            //};

            //ToolTip toolTip = new ToolTip();
            //toolTip.Content = images[0].Name;
            //ToolTipService.SetToolTip(imgImg, toolTip);

            //totalImagePanel.Children.RemoveAt(totalImagePanel.Children.Count - 1);
            //imagePanel.Children.Add(imgImg);

            //List<BackgroundTransferContentPart> imageParts = CreateBackgroundTransferContentPartList(images);

            //Uri uploadUri = new Uri("http://10.0.1.65/Upload/Upload.aspx?username=" + Constants.User.NAME);

            //BackgroundUploader uploader = new BackgroundUploader();
            //if (imageParts != null)
            //{
            //    UploadOperation imagesUpload = await uploader.CreateUploadAsync(uploadUri, imageParts);
            //    await HandleUploadAsync(imagesUpload, true);
            //}
        }

        /// <summary>
        /// Invoked when documentation add button is clicked and choose the documentations.
        /// </summary>
        /// <param name="sender">The documentation add button clicked.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>
        private async void DocUploadButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker picker = FileTypePicker(docsFilterTypeList);
            if (picker == null) return;

            docs = await picker.PickMultipleFilesAsync();

            for (int i = 0; i < docs.Count; ++i)
            {
                Image docImg = new Image
                {
                    Source = new BitmapImage(new Uri("ms-appx:///Images/Upload/doc.png")),
                    Margin = new Thickness(5, 0, 5, 0)
                };

                ToolTip toolTip = new ToolTip();
                toolTip.Content = docs[i].Name;
                ToolTipService.SetToolTip(docImg, toolTip);

                docsPanel.Children.Add(docImg);
            }
        }

        /// <summary>
        /// Invoked when audio add button is clicked and choose the audios.
        /// </summary>
        /// <param name="sender">The audio add button clicked.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>
        private async void AudioUploadButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker picker = FileTypePicker(audiosFilterTypeList);
            if (picker == null) return;

            audios = await picker.PickMultipleFilesAsync();

            for (int i = 0; i < audios.Count; ++i)
            {
                Image audioImg = new Image
                {
                    Source = new BitmapImage(new Uri("ms-appx:///Images/Upload/audio.png")),
                    Margin = new Thickness(5, 0, 5, 0)
                };

                ToolTip toolTip = new ToolTip();
                toolTip.Content = audios[i].Name;
                ToolTipService.SetToolTip(audioImg, toolTip);

                audiosPanel.Children.Add(audioImg);
            }
        }

        /// <summary>
        /// Invoked when video add button is clicked and choose the videos.
        /// </summary>
        /// <param name="sender">The video add button clicked.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>
        private async void VideoUploadButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker picker = FileTypePicker(videosFilterTypeList);
            if (picker == null) return;

            videos = await picker.PickMultipleFilesAsync();

            for (int i = 0; i < videos.Count; ++i)
            {
                Image videoImg = new Image
                {
                    Source = new BitmapImage(new Uri("ms-appx:///Images/Upload/video.png")),
                    Margin = new Thickness(5, 0, 5, 0)
                };

                ToolTip toolTip = new ToolTip();
                toolTip.Content = videos[i].Name;
                ToolTipService.SetToolTip(videoImg, toolTip);

                videosPanel.Children.Add(videoImg);
            }
        }

        /// <summary>
        /// Invoked when all uplaod button is clicked.
        /// </summary>
        /// <param name="sender">The all upload button clicked.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>
        private void allUploadButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckCourseInfomation())
            {
                ShowMessageDialog("Format error! Please check your upload infomation.");
                return;
            }

            COURSE newCourse = new COURSE()
            {
                TITLE = courseNameTextBox.Text,
                PRICE = Convert.ToDecimal(priceTextBox.Text),
                TEACHER = Constants.User.ID,
                CATEGORY = (categoryComboBox.SelectionBoxItem as CATEGORY).ID,
                PG = (pgComboBox.SelectionBoxItem as PARENT_GUIDE).ID
            };
        }

        /// <summary>
        /// Invoked when reset button is clicked.
        /// </summary>
        /// <param name="sender">The reset button clicked.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>
        private void resetUploadButton_Click(object sender, RoutedEventArgs e)
        {
            ResetPage();
        }
        #endregion

        #region Background Upload
        /// <summary>
        /// Create FileOpenPicker and set file filter.
        /// </summary>
        /// <param name="typeFilterList">The filter type list.</param>
        /// <returns>The FileOpenPicker created.</returns>
        private FileOpenPicker FileTypePicker(List<string> typeFilterList)
        {
            // Verify that we are currently not snapped, or that we can unsnap to open the picker.
            if (ApplicationView.Value == ApplicationViewState.Snapped && !ApplicationView.TryUnsnap())
            {
                return null;
            }

            FileOpenPicker picker = new FileOpenPicker();
            picker.CommitButtonText = "Select Files";

            foreach (var type in typeFilterList)
            {
                picker.FileTypeFilter.Add(type);
            }

            return picker;
        }

        /// <summary>
        /// Create the BackgroundTransferContentPart list.
        /// </summary>
        /// <param name="files">The file list.</param>
        /// <returns>The BackgroundTransferContentPart list created.</returns>
        private List<BackgroundTransferContentPart> CreateBackgroundTransferContentPartList(IReadOnlyList<StorageFile> files)
        {
            if (files == null) return null;

            List<BackgroundTransferContentPart> parts = new List<BackgroundTransferContentPart>();
            for (int i = 0; i < files.Count; ++i)
            {
                BackgroundTransferContentPart part = new BackgroundTransferContentPart("File " + i, files[i].Name);
                part.SetFile(files[i]);
                parts.Add(part);
            }

            return parts;
        }

        /// <summary>
        /// Event is invoked on a background thread.
        /// </summary>
        /// <param name="upload">UploadOperation.</param>
        private void UploadProgress(UploadOperation upload)
        {
            // Progress: upload.Guid; Statues: uplaod.Progress.Status

            BackgroundUploadProgress progress = upload.Progress;

            double percentSend = 100;
            if (progress.TotalBytesToSend > 0)
            {
                percentSend = progress.BytesSent * 100 / progress.TotalBytesToSend;
            }

            // Send bytes: progress.BytesSend of progress.TotalBytesSend (percentSend%)
            // Received bytes: progress.BytesReceived of progress.TotalBytesToReceive

            if (progress.HasRestarted)
            {
                // Upload restarted
                System.Diagnostics.Debug.WriteLine("Upload restarted");
            }

            if (progress.HasResponseChanged)
            {
                // Response updated; Header count: upload.GetResponseInformation().Headers.Count
            }
        }

        /// <summary>
        /// Start uplaod and create handler.
        /// </summary>
        /// <param name="upload">UploadOperation handled.</param>
        /// <param name="start">Whether uplaod is started.</param>
        /// <returns>Represent the asynchronous operation.</returns>
        private async Task HandleUploadAsync(UploadOperation upload, bool start)
        {
            try
            {
                Progress<UploadOperation> progressCallback = new Progress<UploadOperation>(UploadProgress);
                if (start)
                {
                    // Start the upload and attach a progress handler.
                    await upload.StartAsync().AsTask(cts.Token, progressCallback);
                }
                else
                {
                    // The upload was already running when the application started, re-attach the progress handler.
                    await upload.AttachAsync().AsTask(cts.Token, progressCallback);
                }

                ResponseInformation response = upload.GetResponseInformation();
                foreach (var c in response.Headers)
                {
                    System.Diagnostics.Debug.WriteLine("{0}, {1}", c.Key, c.Value);
                }

                if (images != null && images.Count != 0)
                {
                    toBeUploadCourse.ImageUri = response.Headers[images.FirstOrDefault().Name];
                }
                SaveUploadLessonToListAsync(response);
            }
            catch (TaskCanceledException)
            {
                ShowMessageDialog("Error! Upload canceled.");
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Save lesson to allLessons list.
        /// </summary>
        /// <param name="response">The data responed by server.</param>
        private void SaveUploadLessonToListAsync(ResponseInformation response)
        {
            if (docs != null && docs.Count != 0 & allLessons[lessonCount - 1].GetDocList().Count == 0)
            {
                foreach (var doc in docs)
                {
                    allLessons[lessonCount - 1].GetDocList().Add(new Resource(doc.Name, response.Headers[doc.Name], doc.FileType));
                }
            }
            if (audios != null && audios.Count != 0 && allLessons[lessonCount - 1].GetAudioList().Count == 0)
            {
                foreach (var audio in audios)
                {
                    allLessons[lessonCount - 1].GetAudioList().Add(new Resource(audio.Name, response.Headers[audio.Name], audio.FileType));
                }
            }
            if (videos != null && videos.Count != 0 && allLessons[lessonCount - 1].GetVideoList().Count == 0)
            {
                foreach (var video in videos)
                {
                    allLessons[lessonCount - 1].GetVideoList().Add(new Resource(video.Name, response.Headers[video.Name], video.FileType));
                }
            }
        }
        #endregion

        /// <summary>
        /// Add lesson info to page after uploading button clicked.
        /// </summary>
        private void AddLessonInfo()
        {
            TextBlock newLessonName = new TextBlock
            {
                Text = lessonCount + ". " + lessonName.Text,
                FontSize = 50,
                Height = 70,
                Margin = new Thickness(5, 0, 0, 0),
                Foreground = new SolidColorBrush(Colors.White)
            };

            StackPanel newLessonRes = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right
            };

            if (docs != null)
            {
                for (int i = 0; i < docs.Count; ++i)
                {
                    Image docImg = new Image
                    {
                        Source = new BitmapImage(new Uri("ms-appx:///Images/Upload/doc.png")),
                        Height = 70,
                        Width = 35,
                        Margin = new Thickness(2, 0, 2, 0),
                        HorizontalAlignment = HorizontalAlignment.Right
                    };
                    newLessonRes.Children.Add(docImg);

                    ToolTip toolTip = new ToolTip();
                    toolTip.Content = docs[i].Name;
                    ToolTipService.SetToolTip(docImg, toolTip);
                }
            }

            if (audios != null)
            {
                for (int i = 0; i < audios.Count; ++i)
                {
                    Image audioImg = new Image
                    {
                        Source = new BitmapImage(new Uri("ms-appx:///Images/Upload/audio.png")),
                        Height = 70,
                        Width = 35,
                        Margin = new Thickness(2, 0, 2, 0),
                        HorizontalAlignment = HorizontalAlignment.Right
                    };
                    newLessonRes.Children.Add(audioImg);

                    ToolTip toolTip = new ToolTip();
                    toolTip.Content = audios[i].Name;
                    ToolTipService.SetToolTip(audioImg, toolTip);
                }
            }

            if (videos != null)
            {
                for (int i = 0; i < videos.Count; ++i)
                {
                    Image videoImg = new Image
                    {
                        Source = new BitmapImage(new Uri("ms-appx:///Images/Upload/video.png")),
                        Height = 70,
                        Width = 35,
                        Margin = new Thickness(2, 0, 2, 0),
                        HorizontalAlignment = HorizontalAlignment.Right
                    };
                    newLessonRes.Children.Add(videoImg);

                    ToolTip toolTip = new ToolTip();
                    toolTip.Content = videos[i].Name;
                    ToolTipService.SetToolTip(videoImg, toolTip);
                }
            }

            lessonInfo.Children.Add(newLessonName);
            lessonRes.Children.Add(newLessonRes);
        }

        #region Reset Page and Popup methods
        /// <summary>
        /// Reset the popup.
        /// </summary>
        private void ResetPopup()
        {
            bool inFlag = false;

            lessonName.Text = "Lesson Name";
            lessonDescription.Text = "Description...";
            //imagePanel.Children.Clear();
            docsPanel.Children.Clear();
            audiosPanel.Children.Clear();
            videosPanel.Children.Clear();
            //foreach (var c in totalImagePanel.Children)
            //{
            //    if (c == addImageButton)
            //    {
            //        inFlag = true;
            //    }
            //}
            //if (!inFlag)
            //{
            //    totalImagePanel.Children.Add(addImageButton);
            //}
            images = null;
            docs = null;
            audios = null;
            videos = null;
        }

        /// <summary>
        /// Reset the page.
        /// </summary>
        private void ResetPage()
        {
            //bool inFlag = false;

            categoryDsq = (DataServiceQuery<CATEGORY>)(from category in ctx.CATEGORY select category);
            categoryDsq.BeginExecute(OnCategoryComplete, null);

            pgDsq = (DataServiceQuery<PARENT_GUIDE>)(from pg in ctx.PARENT_GUIDE select pg);
            pgDsq.BeginExecute(OnPGComplete, null);

            lessonCount = 0;
            cts = new CancellationTokenSource();
            courseNameTextBox.Text = "";
            //priceTextBox.Text = "Price";
            lessonInfo.Children.Clear();
            lessonRes.Children.Clear();
            images = null;
            docs = null;
            audios = null;
            videos = null;
            allLessons = new List<Lesson>();
            toBeUploadCourse = new Course();

            //imagePanel.Children.Clear();
            //foreach (var c in totalImagePanel.Children)
            //{
            //    if (c == addImageButton)
            //    {
            //        inFlag = true;
            //    }
            //}
            //if (!inFlag)
            //{
            //    totalImagePanel.Children.Add(addImageButton);
            //}
        }
        #endregion

        #region Check methods
        /// <summary>
        /// Check whether lesson information is legal.
        /// </summary>
        /// <returns>If legal, then true; else false.</returns>
        private bool CheckLessonInfomation()
        {
            bool result = true;

            try
            {
                if (String.IsNullOrWhiteSpace(lessonName.Text)) result = false;
                if (lessonDescription.Text.Trim() == "Description...") result = false;
            }
            catch
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Check whether course information is legal.
        /// </summary>
        /// <returns>If legal, then true; else false.</returns>
        private bool CheckCourseInfomation()
        {
            bool result = true;

            try
            {
                if (String.IsNullOrWhiteSpace(courseNameTextBox.Text) || courseNameTextBox.Text.Trim() == "Course Name") result = false;
                if (Convert.ToDouble(priceTextBox.Text) < 0) result = false;
                if (categoryComboBox.SelectedItem == null) result = false;
                if (pgComboBox.SelectedItem == null) result = false;
                if (images == null || images.Count == 0) result = false;
            }
            catch
            {
                result = false;
            }

            return result;
        }
        #endregion

        #region Message Dialog
        /// <summary>
        /// Network Connection error MessageDialog.
        /// </summary>
        private async void ShowNetworkMessageDialog()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                try
                {
                    var messageDialog = new MessageDialog("No Network has been found! You will lose all upload data!");
                    messageDialog.Commands.Add(new UICommand("Try Again", (command) =>
                    {
                        Frame.Navigate(typeof(Uploading));
                    }));
                    messageDialog.Commands.Add(new UICommand("Close"));
                    await messageDialog.ShowAsync();
                }
                catch
                {
                    ShowNetworkMessageDialog();
                }
            });
        }

        /// <summary>
        /// Upload information error MessageDialog.
        /// </summary>
        private async void ShowMessageDialog(string msg)
        {
            var messageDialog = new MessageDialog("Format error! Please check your upload infomation.");
            messageDialog.Commands.Add(new UICommand("Close"));
            await messageDialog.ShowAsync();
        }
        #endregion
    }
}
