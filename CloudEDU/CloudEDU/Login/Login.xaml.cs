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
using Windows.UI.Core;

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

        public Login()
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
            //where user.NAME == InputUsername.Text
            //customerDsq = (DataServiceQuery<CUSTOMER>)(from user in ctx.CUSTOMER  select user );
            //customerDsq.BeginExecute(OnCustomerComplete, null);
            //System.Diagnostics.Debug.WriteLine(ComputeMD5("aaa"));
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

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            //login
            //foreach (CUSTOMER c in csl)
            //{
            //    if (c.NAME == InputUsername.Text)
            //    {
            //        if (c.PASSWORD == ComputeMD5(InputPassword.Password))
            //        {
                        // login success
                        // CUSTOMER
                        //Constants.User = c;

                        //User
                        //Constants.User = new User("Shania", "../Images/Users/ania.png");
            customerDsq = (DataServiceQuery<CUSTOMER>)(from cus in ctx.CUSTOMER select cus);
            customerDsq.BeginExecute(OnUserComplete, null);
            
            //Constants.User.NAME = "Dewen";
            //Constants.User.ID = 1;
            //            System.Diagnostics.Debug.WriteLine("login success");
            //            Frame.Navigate(typeof(CourseStore.Courstore));
                        // navigate 
            //        }
            //    }
            //}
            // login fail
        }

        private async void OnUserComplete(IAsyncResult iar)
        {
            IEnumerable<CUSTOMER> customers = customerDsq.EndExecute(iar);
            Constants.User = customers.FirstOrDefault();
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Frame.Navigate(typeof(CourseStore.Courstore));
                //dataCategory = coursesData.GetGroupsByCategory();
                //cvs1.Source = dataCategory;
                //(SemanticZoom.ZoomedOutView as ListViewBase).ItemsSource = cvs1.View.CollectionGroups;
                //loadingProgressRing.IsActive = false;
            });


            
        }

        public static string ComputeMD5(string str)
        {
            var alg = HashAlgorithmProvider.OpenAlgorithm("MD5");
            IBuffer buff = CryptographicBuffer.ConvertStringToBinary(str, BinaryStringEncoding.Utf8);
            var hashed = alg.HashData(buff);
            var res = CryptographicBuffer.EncodeToHexString(hashed);
            return res;
        }
    }
}
