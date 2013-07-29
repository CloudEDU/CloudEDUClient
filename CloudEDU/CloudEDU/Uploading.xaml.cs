﻿using CloudEDU.Common;
using System;
using System.Collections.Generic;
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
        List<string> docsFilterTypeList = new List<string> { ".doc", ".docx", ".pdf" };
        List<string> audiosFilterTypeList = new List<string> { ".mp3", ".wmv" };
        List<string> videosFilterTypeList = new List<string> { ".mp4", ".avi", ".rm", ".rmvb" };

        IReadOnlyList<StorageFile> docs = null;
        IReadOnlyList<StorageFile> audios = null;
        IReadOnlyList<StorageFile> videos = null;

        private CancellationTokenSource cts;

        int lessonCount = 0;

        public Uploading()
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
            ResetPage();
        }

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
            List<BackgroundTransferContentPart> docParts = CreateBackgroundTransferContentPartList(docs);
            List<BackgroundTransferContentPart> audioParts = CreateBackgroundTransferContentPartList(audios);
            List<BackgroundTransferContentPart> videoParts = CreateBackgroundTransferContentPartList(videos);

            Uri docsUri = new Uri("http://10.0.1.65/Upload/Upload.aspx?username=haohao");
            Uri audiosUri = new Uri("http://audios");
            Uri videosUri = new Uri("http://videos");

            BackgroundUploader uploader = new BackgroundUploader();
            if (docParts != null)
            {
                UploadOperation docsUpload = await uploader.CreateUploadAsync(docsUri, docParts);
                // Attach progress and completion handlers.
                await HandleUploadAsync(docsUpload, true);
            }
            if (audioParts != null)
            {
                UploadOperation audiosUpload = await uploader.CreateUploadAsync(audiosUri, audioParts);
                //await HandleUploadAsync(audiosUpload, true);
            }
            if (videoParts != null)
            {
                UploadOperation videosUpload = await uploader.CreateUploadAsync(videosUri, videoParts);
                //await HandleUploadAsync(videosUpload, true);
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
            List<BackgroundTransferContentPart> parts = new List<BackgroundTransferContentPart>();

            if (files == null) return null;
            for (int i = 0; i < files.Count; ++i)
            {
                BackgroundTransferContentPart part = new BackgroundTransferContentPart("File " + i, files[i].Name);
                part.SetHeader("Username", "Lazy");
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
            System.Diagnostics.Debug.WriteLine(upload);
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
                IReadOnlyDictionary<string, string> dic = upload.GetResponseInformation().Headers;
                foreach (KeyValuePair<string, string> c in dic)
                {
                    System.Diagnostics.Debug.WriteLine(c);
                }
            }
        }

        /// <summary>
        /// Handle the upload.
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
            }
            catch (TaskCanceledException)
            {
                // Canceled: upload.Guid
            }
            catch
            {
                throw;
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
                Text = ++lessonCount + ". " + lessonName.Text,
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
            lessonName.Text = "Lesson Name";
            lessonDescription.Text = "Description...";
            docsPanel.Children.Clear();
            audiosPanel.Children.Clear();
            videosPanel.Children.Clear();
            docs = null;
            audios = null;
            videos = null;
        }

        private void ResetPage()
        {
            lessonCount = 0;
            cts = new CancellationTokenSource();
            lessonInfo.Children.Clear();
            lessonRes.Children.Clear();
            docs = null;
            audios = null;
            videos = null;
        }
        #endregion
    }
}