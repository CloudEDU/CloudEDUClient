using CloudEDU.Common;
using CloudEDU.Service;
using CloudEDU.CourseStore;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CloudEDU.Login
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Login : Page
    {
        private CloudEDUEntities ctx = null;
        private DataServiceQuery<CUSTOMER> customerDsq = null;
        private List<CUSTOMER> csl;
        private string emptyUsername;
        private bool firstTimeForUsername = true;

        public Login()
        {
            this.InitializeComponent();

            //emptyUsername = InputUsername.Text;
            ctx = new CloudEDUEntities(new Uri(Constants.DataServiceURI));
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            // auto log
            if (Constants.Read<bool>("AutoLog") == true)
            {
                Constants.User = User.SelectLastUser();
                // navigate
                Frame.Navigate(typeof(CourseStore.Courstore));
            }

            // last user
            if (Constants.Read<string>("LastUser") == default(string))
            {

            }
            else
            {
                System.Diagnostics.Debug.WriteLine(Constants.Read<string>("LastUser"));
                if (Frame.Navigate(typeof(SignUp)))
                    System.Diagnostics.Debug.WriteLine("suc navigate");
                else
                    System.Diagnostics.Debug.WriteLine("fail navigate");
            }
        }

        private void OnCustomerComplete(IAsyncResult result)
        {
            IEnumerable<CUSTOMER> cs = customerDsq.EndExecute(result);
            csl = new List<CUSTOMER>(cs);
            System.Diagnostics.Debug.WriteLine(csl[0].NAME);
        }

        /// <summary>
        /// Invoked when back button is clicked and navigate to sign up page.
        /// </summary>
        /// <param name="sender">The sign up button clicked.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>
        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SignUp));
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            //login

            if (InputUsername.Text.Equals(emptyUsername) || InputPassword.Password.Equals(string.Empty))
            {
                var messageDialog = new MessageDialog("Check your input!");
                await messageDialog.ShowAsync();
                return;
            }

            TaskFactory<IEnumerable<CUSTOMER>> tf = new TaskFactory<IEnumerable<CUSTOMER>>();
            customerDsq = (DataServiceQuery<CUSTOMER>)(from user in ctx.CUSTOMER where user.NAME.Equals(InputUsername.Text) select user);
            IEnumerable<CUSTOMER> cs = await tf.FromAsync(customerDsq.BeginExecute(null, null), iar => customerDsq.EndExecute(iar));
            csl = new List<CUSTOMER>(cs);
            bool isLogined = false;
            foreach (CUSTOMER c in csl)
            {
                if (c.NAME == InputUsername.Text)
                {
                    if (c.PASSWORD == Constants.ComputeMD5(InputPassword.Password))
                    {
                         //login success
                         //CUSTOMER
                        //Constants.User = c;

                        //User
                        Constants.Save<bool>("AutoLog", (bool)CheckAutoLogin.IsChecked);
                        Constants.User = new User(c);
                        isLogined = true;
                        System.Diagnostics.Debug.WriteLine("login success");
                        Frame.Navigate(typeof(Courstore));
                        // navigate 
                    }
                }
            }
            // login fail
            if (isLogined) return;
            var msgDialog = new MessageDialog("Username Or Password is wrong");
            await msgDialog.ShowAsync();
        }

        private void InputUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!firstTimeForUsername)
                return;
            emptyUsername = InputUsername.Text;
            firstTimeForUsername = false;
        }
    }
}
