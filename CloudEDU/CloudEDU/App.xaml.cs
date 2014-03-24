using CloudEDU.Common;
using CloudEDU.CourseStore;
using CloudEDU.Login;
using CloudEDU.Service;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace CloudEDU
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        //CloudEDUEntities ctx = null;


        private CloudEDUEntities ctx = null;
        private DataServiceQuery<CUSTOMER> customerDsq = null;
        private List<CUSTOMER> csl;


        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;

            ctx = new CloudEDUEntities(new Uri(Constants.DataServiceURI));

        }



        private void OnCustomerComplete(IAsyncResult result)
        {
            csl = customerDsq.EndExecute(result).ToList();
            System.Diagnostics.Debug.WriteLine(csl[0].NAME);
            Constants.User = new User(csl[0]);
        }


        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {


            customerDsq = (DataServiceQuery<CUSTOMER>)(from user in ctx.CUSTOMER select user);
            customerDsq.BeginExecute(OnCustomerComplete, null);



            // Do not repeat app initialization when already running, just ensure that
            // the window is active
            if (args.PreviousExecutionState == ApplicationExecutionState.Running)
            {
                Window.Current.Activate();
                return;
            }

            if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                //TODO: Load state from previously suspended application
            }





            // Create a Frame to act navigation context and navigate to the first page
            var rootFrame = new Frame();
            if (!rootFrame.Navigate(typeof(Login.Login)))
            {
                throw new Exception("Failed to create initial page");
            }

            // Place the frame in the current Window and ensure that it is active
            Window.Current.Content = rootFrame;
            Window.Current.Activate();
            /*
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
                System.Diagnostics.Debug.WriteLine("rootframe == null");
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter

                System.Diagnostics.Debug.WriteLine("rootframe.content == null");
                Type goalPage;

                // last user
                if (Constants.Read<string>("LastUser") == default(string))
                {
                    goalPage = typeof(Login.Login);
                    //goalPage = typeof(CourseStore.Courstore);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(Constants.Read<string>("LastUser"));
                    Constants.User = User.SelectLastUser();
                    goalPage = typeof(LoginDefault);
                    System.Diagnostics.Debug.WriteLine("LoginDefault");
                }

                // auto log
                if (Constants.Read<bool>("AutoLog") == true)
                {
                    Constants.User = User.SelectLastUser();
                    // navigate
                    System.Diagnostics.Debug.WriteLine("ID:{0}, ATTEND:{1}, TEACH:{2}",
                        Constants.User.ID, Constants.User.ATTEND_COUNT, Constants.User.TEACH_COUNT);
                    goalPage = typeof(CourseStore.Courstore);
                    System.Diagnostics.Debug.WriteLine("Courstore");
                }
                //goalPage = typeof(Login.Login);
                if (!rootFrame.Navigate(goalPage, args.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                    System.Diagnostics.Debug.WriteLine("!rootframe");
                }
                System.Diagnostics.Debug.WriteLine("Outer");
            }
            // Ensure the current window is active
            Window.Current.Activate();
            */
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

       
    }

    /// <summary>
    /// Global ItemContainerStyleSelector for GridView
    /// </summary>
    public class VariableSizedStyleSelector : StyleSelector
    {
        public Style NormalStyle { get; set; }
        public Style DoubleHeightStyle { get; set; }
        public Style DoubleWidthStyle { get; set; }
        public Style SquareStyle { get; set; }

        /// <summary>
        /// Returns a specific Style based on custom logic.
        /// </summary>
        /// <param name="item">The content</param>
        /// <param name="container">The element to which the style is applied</param>
        /// <returns>An application-specific style to apply</returns>
        protected override Style SelectStyleCore(object item, DependencyObject container)
        {
            GridViewItemContainerType containerType = ((Course)item).ItemContainerType;

            switch (containerType)
            {
                case GridViewItemContainerType.DoubleHeightGridViewItemContainerSize:
                    return DoubleHeightStyle;
                case GridViewItemContainerType.DoubleWidthGridViewItemContsinerSize:
                    return DoubleWidthStyle;
                case GridViewItemContainerType.SquareGridViewItemContainerSize:
                    return SquareStyle;
                default:
                    return NormalStyle;
            }
        }
    }
}
