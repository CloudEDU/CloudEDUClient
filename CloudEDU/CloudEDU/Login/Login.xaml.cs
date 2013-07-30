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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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

        public Login()
        {
            this.InitializeComponent();

            ctx = new CloudEDUEntities(new Uri(Constants.DataServiceURI));
        }

        //private void OnCallback(IAsyncResult ar)
        //{
        //    CUSTOMER a = customerDsq.EndExecute(ar).FirstOrDefault();
        //    System.Diagnostics.Debug.WriteLine(a.NAME);
        //    a.NAME = "hhdbl";
        //    ctx.UpdateObject(a);
        //    ctx.BeginSaveChanges(OnSave, null);
        //}

        private void OnSave(IAsyncResult ar)
        {
            var d = ctx.EndExecute(ar);
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            customerDsq = (DataServiceQuery<CUSTOMER>)(from cu in ctx.CUSTOMER select cu);
            //customerDsq.BeginExecute(OnCallback, null
            
            //TaskFactory<IEnumerable<CUSTOMER>> tf = new TaskFactory<IEnumerable<CUSTOMER>>();
            //IEnumerable<CUSTOMER> courses = await tf.FromAsync(customerDsq.BeginExecute(null, null), ira => customerDsq.EndExecute(ira));
            //CUSTOMER c = courses.FirstOrDefault();
            //c.NAME = "dldbl";
            CUSTOMER c = CUSTOMER.CreateCUSTOMER(0, "siafjksla", "sfajklasfj", 213, 2.2, 4.2, DateTime.Now, true);


            ctx.AddToCUSTOMER(c);
            ctx.BeginSaveChanges(OnSave, ctx);
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
    }
}
