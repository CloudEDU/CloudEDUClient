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
using SQLite;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CloudEDU.Login
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SignUp : Page
    {
        private CloudEDUEntities ctx = null;

        public SignUp()
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
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!InputPassword.Password.Equals(ReInputPassword.Password))
            {
                var dialog = new MessageDialog("Passwords are not same! Try again, thx!");
                await dialog.ShowAsync();
                return;
            }
            //CUSTOMER c = CUSTOMER.CreateCUSTOMER(null, InputUsername.Text, InputPassword.Password, null, null, null);
            CUSTOMER c = new CUSTOMER()
            {
                NAME = InputUsername.Text,
                PASSWORD = Constants.ComputeMD5(InputPassword.Password),
            };
            ctx.AddToCUSTOMER(c);
            ctx.BeginSaveChanges(OnCustomerSaveChange, null);

        }

        private void OnCustomerSaveChange(IAsyncResult result)
        {
            try
            {
                ctx.EndSaveChanges(result);
            }
            catch
            {
                 ShowMessageDialog();
                 //Network Connection error.
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
                        Frame.Navigate(typeof(SignUp));
                    }));
                    messageDialog.Commands.Add(new UICommand("Close"));
                    //loadingProgressRing.IsActive = false;
                    await messageDialog.ShowAsync();
                }
                catch
                {
                    ShowMessageDialog();
                }
            });
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Login));
        }
    }
}
