using CloudEDU.Common;
using CloudEDU.Service;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace CloudEDU.CourseStore
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SearchResult : GlobalPage
    {
        private StoreData storeSearchResult;
        private List<GroupInfoList<Object>> searchResults;
        private CloudEDUEntities ctx = null;
        private DataServiceQuery<COURSE_AVAIL> courseDsq = null;

        string searchString = null;

        public SearchResult()
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
            searchString = e.Parameter as string;
            loadingProgressRing.IsActive = true;

            courseDsq = (DataServiceQuery<COURSE_AVAIL>)(from course_avail in ctx.COURSE_AVAIL
                                                         where course_avail.TITLE.Contains(searchString)
                                                         select course_avail);
            courseDsq.BeginExecute(OnSearchResultComplete, null);

            UserProfileBt.DataContext = Constants.User;
        }

        private async void OnSearchResultComplete(IAsyncResult result)
        {
            storeSearchResult = new StoreData();
            try
            {
                IEnumerable<COURSE_AVAIL> courses = courseDsq.EndExecute(result);

                foreach (var c in courses)
                {
                    storeSearchResult.AddCourse(Constants.CourseAvail2Course(c));
                }
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        searchResults = storeSearchResult.GetSearchResultGroup(searchString);
                        cvs1.Source = searchResults;
                        loadingProgressRing.IsActive = false;
                    });
            }
            catch
            {
                ShowMessageDialog();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
            else
            {
                Frame.Navigate(typeof(Courstore));
            }
        }

        /// <summary>
        /// Invoked when a course within this category is clicked.
        /// </summary>
        /// <param name="sender">The GridView displaying the course clicked.</param>
        /// <param name="e">Event data that describes the course clicked.</param>
        private void Course_ItemClick(object sender, ItemClickEventArgs e)
        {
            var courseName = ((Course)e.ClickedItem).Title;

            Frame.Navigate(typeof(CourseOverview), courseName);
        }
        private void UserProfileButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Login.Profile));
        }

        /// <summary>
        /// Network Connection error MessageDialog.
        /// </summary>
        private async void ShowMessageDialog(String msg = "No network has been fooooooound ")
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                try
                {
                    var messageDialog = new MessageDialog("No Network has been found!");
                    messageDialog.Commands.Add(new UICommand("Try Again", (command) =>
                    {
                        Frame.Navigate(typeof(Courstore));
                    }));
                    messageDialog.Commands.Add(new UICommand("Close"));
                    loadingProgressRing.IsActive = false;
                    await messageDialog.ShowAsync();
                }
                catch
                {
                    ShowMessageDialog();
                }
            });
        }
    }
}
