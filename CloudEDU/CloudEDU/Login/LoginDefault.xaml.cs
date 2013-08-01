using CloudEDU.Common;
using CloudEDU.CourseStore;
using CloudEDU.Service;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;



// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CloudEDU.Login
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 

    public sealed partial class LoginDefault : Page
    {

        private List<CUSTOMER> csl;
        private CloudEDUEntities ctx = null;
        private DataServiceQuery<CUSTOMER> customerDsq = null;
        //static int WidthOfScreen = 1366;

        //UserSelButtonControl LastSelectedUser;
        //UserSelButtonControl SelectedUser;
        User user;
        public LoginDefault()
        {
            this.InitializeComponent();
            Setup();
        }

        private void Setup()
        {
            ctx = new CloudEDUEntities(new Uri(Constants.DataServiceURI));
            SetUser();
            UserSelButtonControl bt = new UserSelButtonControl();
            bt.user = Constants.User;
            bt.Click += Button_Click;
            UsersStack.Children.Insert(0, bt);
            //test.user = new User("Fox", "http://www.gravatar.com/avatar/3c2986ad7ac1f2230ea3596f44563328");
            //test.UserName = test.user.NAME;
        }

        private void SetUser()
        {
            //string imageSource = Constants.ComputeMD5("yougmark94@gmail.com");
            //imageSource = "http://www.gravatar.com/avatar/" + imageSource;
            //users.Add(new User("Mark", imageSource));
            //Constants.User = User.SelectLastUser();
            user = Constants.User;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            customerDsq = (DataServiceQuery<CUSTOMER>)(from user in ctx.CUSTOMER select user);
            customerDsq.BeginExecute(OnCustomerComplete, null);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginSel));
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SignUp));
        }

        private void OnCustomerComplete(IAsyncResult result)
        {
            IEnumerable<CUSTOMER> cs = customerDsq.EndExecute(result);
            csl = new List<CUSTOMER>(cs);
            System.Diagnostics.Debug.WriteLine(csl[0].NAME);
        }

        private async void LoginButton_Click(object sende, RoutedEventArgs e)
        {
            if (InputPassword.Password.Equals(string.Empty))
            {
                var messageDialog = new MessageDialog("Please Check your input!");
                await messageDialog.ShowAsync();
                return;
            }
            try
            {
                TaskFactory<IEnumerable<CUSTOMER>> tf = new TaskFactory<IEnumerable<CUSTOMER>>();
                customerDsq = (DataServiceQuery<CUSTOMER>)(from user in ctx.CUSTOMER where user.NAME.Equals(Constants.User.NAME) select user);
                IEnumerable<CUSTOMER> cs = await tf.FromAsync(customerDsq.BeginExecute(null, null), iar => customerDsq.EndExecute(iar));
                csl = new List<CUSTOMER>(cs);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                ShowMessageDialog();
            }
            bool isLogined = false;
            try
            {
                foreach (CUSTOMER c in csl)
                {
                    if (c.NAME.Equals(Constants.User.NAME))
                    {
                        if (c.PASSWORD == Constants.ComputeMD5(InputPassword.Password))
                        {
                            //login success
                            Constants.User = new User(c);
                            Constants.Save<bool>("AutoLog", (bool)CheckAutoLogin.IsChecked);

                            System.Diagnostics.Debug.WriteLine("login success");
                            string courseUplaodUri = "/AddDBLog?opr='Login'&msg='" + Constants.User.NAME + "'";

                            try
                            {
                                TaskFactory<IEnumerable<bool>> tf = new TaskFactory<IEnumerable<bool>>();
                                IEnumerable<bool> result = await tf.FromAsync(ctx.BeginExecute<bool>(new Uri(courseUplaodUri, UriKind.Relative), null, null), iar => ctx.EndExecute<bool>(iar));

                            }
                            catch
                            {
                            }
                            isLogined = true;
                            Frame.Navigate(typeof(CourseStore.Courstore));
                            // navigate 
                        }
                    }
                }
                
            }
            catch (Exception exp)
            {
                System.Diagnostics.Debug.WriteLine("Msg: {0}\nInnerExp:{1}\nStackTrace: {2} ",
                    exp.Message, exp.InnerException, exp.StackTrace);
                ShowMessageDialog();
            }

            if (!isLogined)
            {
                var msgDialog = new MessageDialog("Username Or Password is wrong");
                await msgDialog.ShowAsync();
            }
        }
        /// <summary>
        /// Network Connection error MessageDialog.
        /// </summary>
        private async void ShowMessageDialog()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                try
                {
                    var messageDialog = new MessageDialog("No Network has been found!");
                    messageDialog.Commands.Add(new UICommand("Try Again", (command) =>
                    {
                        Frame.Navigate(typeof(LoginDefault));
                    }));
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
