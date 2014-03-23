using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using CloudEDU.Common;
using CloudEDU.Service;
using Windows.UI.Core;
using Windows.UI.Popups;
using System.Data.Services.Client;
using System.Threading.Tasks;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace CloudEDU.Login
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Profile : Page
    {
        private CloudEDUEntities ctx = null;
        private DataServiceQuery<CUSTOMER> customerDsq = null;

        private List<CUSTOMER> csl;
        private CUSTOMER changedCustomer = null;
        public Profile()
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
            //!!!!!!!!!!
            Constants.User.SetAttendTeachNumber();
            image.DataContext = Constants.User;
            biggestGrid.DataContext = Constants.User;
            customerDsq = (DataServiceQuery<CUSTOMER>)(from user in ctx.CUSTOMER select user);
            customerDsq.BeginExecute(OnCustomerComplete, null);
        }

        private void OnCustomerComplete(IAsyncResult result)
        {
            csl = customerDsq.EndExecute(result).ToList();
            System.Diagnostics.Debug.WriteLine(csl[0].NAME);
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

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            retypePasswordStackPanel.Visibility = Visibility.Visible;
        }

        private async void SaveImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (passwordBox.Password.Equals(string.Empty) || retypePasswordBox.Password.Equals(string.Empty))
            {
                var messageDialog = new MessageDialog("Please Check your input, Password can't be empty!");
                await messageDialog.ShowAsync();
                return;
            }

            if (!Constants.isEmailAvailable(email.Text))
            {
                var messageDialog = new MessageDialog("Please Check your input, EMAIL is not Available!");
                await messageDialog.ShowAsync();
                return;
            }

            if (!passwordBox.Password.Equals(retypePasswordBox.Password))
            {
                var dialog = new MessageDialog("Passwords are not same! Try again, thx!");
                await dialog.ShowAsync();
                return;
            }
            
            foreach (CUSTOMER c in csl)
            {
                if (c.NAME == Constants.User.NAME)
                {
                    
                    c.DEGREE = (string)degreeBox.SelectedItem;
                    c.PASSWORD = Constants.ComputeMD5(passwordBox.Password);
                    c.EMAIL = email.Text;
                    c.BIRTHDAY = Convert.ToDateTime(birthday.Text);
                    changedCustomer = c;
                    ctx.UpdateObject(c);
                    ctx.BeginSaveChanges(OnCustomerSaveChange, null);
                    
                }
            }
        }

        private async void OnCustomerSaveChange(IAsyncResult result)
        {
            try
            {
                ctx.EndSaveChanges(result);
                Constants.User = new User(changedCustomer);
                //Constants.User = new User(changedCustomer);
                string Uri = "/AddDBLog?opr='ChangeProfile'&msg='" + Constants.User.NAME + "'";

                try
                {
                    TaskFactory<IEnumerable<bool>> tf = new TaskFactory<IEnumerable<bool>>();
                    IEnumerable<bool> resulta = await tf.FromAsync(ctx.BeginExecute<bool>(new Uri(Uri, UriKind.Relative), null, null), iar => ctx.EndExecute<bool>(iar));
                }
                catch
                {
                }

                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    Frame.GoBack();
                });
            }
            catch
            {
                ShowMessageDialog("On customer save change");
                //Network Connection error.
            }
           
        }

        private void ResetImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            retypePasswordStackPanel.Visibility = Visibility.Collapsed;
            degreeBox.SelectedItem = Constants.User.DEGREE;
            System.Diagnostics.Debug.WriteLine(Constants.User.DEGREE);
            degreeBox.SelectedIndex = 0;
            email.Text = Constants.User.EMAIL;
            birthday.Text = Constants.User.BIRTHDAY.ToString();
        }

        private void degreeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(degreeBox.SelectedItem);
        }
        /// <summary>
        /// Network Connection error MessageDialog.
        /// </summary>
        private async void ShowMessageDialog(String msg = "No Network has been found!")
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                try
                {
                    var messageDialog = new MessageDialog(msg);
                    //messageDialog.Commands.Add(new UICommand("Try Again", (command) =>
                    //{
                    //    Frame.Navigate(typeof(Profile));
                    //}));
                    messageDialog.Commands.Add(new UICommand("Close"));
                    //loadingProgressRing.IsActive = false;
                    await messageDialog.ShowAsync();
                }
                catch
                {
                    //ShowMessageDialog();
                }
            });
        }

    }
}
