using CloudEDU.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.BackgroundTransfer;
using Windows.Security.Credentials.UI;
using Windows.Storage;
using Windows.Storage.Pickers;
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
        }

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

            // Reset button visual state
            VisualStateManager.GoToState(UploadLessionButton, "Normal", false);
            VisualStateManager.GoToState(CancelUploadButton, "Normal", false);
        }

        /// <summary>
        /// Invoked when upload button in popup is clicked and add new lesson.
        /// </summary>
        /// <param name="sender">The upload button clicked.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>
        private void UploadLessionButton_Click(object sender, RoutedEventArgs e)
        {

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

                videosPanel.Children.Add(videoImg);
            }
        }

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

            for (int i = 0; i < files.Count; ++i)
            {
                BackgroundTransferContentPart part = new BackgroundTransferContentPart("File " + i, files[i].Name);
                part.SetFile(files[i]);
                parts.Add(part);
            }

            return parts;
        }
    }
}
