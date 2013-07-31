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
using Windows.Media;
using Windows.UI.Xaml.Media.Animation;
using CloudEDU.Common;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CloudEDU.Login
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 

    public sealed partial class LoginTmp : Page
    {
        //static int WidthOfScreen = 1366;

        UserSelButtonControl LastSelectedUser;
        UserSelButtonControl SelectedUser;
        List<User> users;
        public LoginTmp()
        {
            this.InitializeComponent();
            Setup();
        }

        private void Setup()
        {
            SetUsers();
            foreach (User user in users)
            {
                UserSelButtonControl bt = new UserSelButtonControl();
                bt.Content = user.ImageSource;
                bt.user = user;
                bt.Click += Button_Click;
                bt.UserName = user.Username;
                UsersStack.Children.Insert(0,bt);
            }
            test.user = new User("Fox", "http://www.gravatar.com/avatar/3c2986ad7ac1f2230ea3596f44563328");
            test.UserName = test.user.Username;
            
        }

        private void SetUsers()
        {
            users = new List<User>();
            string imageSource = Constants.ComputeMD5("yougmark94@gmail.com");
            imageSource = "http://www.gravatar.com/avatar/" + imageSource;
            users.Add(new User("Mark", imageSource));
        }
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            LastSelectedUser = SelectedUser;
            SelectedUser = sender as UserSelButtonControl;
            if (LastSelectedUser != null && SelectedUser.Equals(LastSelectedUser))
                return;
            
            System.Diagnostics.Debug.WriteLine("tap on image in logintmp");

            SelectedUser.Margin = new Thickness(50, 0, 50, 0);

            TimeSpan span = new TimeSpan(0,0,0,0,200);
            Grid grid = SelectedUser.grid;
            DoubleAnimation scaleY = new DoubleAnimation();
            scaleY.To = 1.5;
            scaleY.Duration = new Duration(span);
            Storyboard.SetTargetProperty(scaleY, "(UIElement.RenderTransform). (CompositeTransform.ScaleY)");
            Storyboard.SetTarget(scaleY, grid);
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(scaleY);
            DoubleAnimation scaleX = new DoubleAnimation();
            scaleX.To = 1.5;
            scaleX.Duration = new Duration(span);
            Storyboard.SetTargetProperty(scaleX, "(UIElement.RenderTransform). (CompositeTransform.ScaleX)");
            Storyboard.SetTarget(scaleX, grid);
            storyboard.Children.Add(scaleX);

            if (LastSelectedUser != null)
            {
                LastSelectedUser.Margin = new Thickness(5, 0, 0, 0);
                Grid gridOld = LastSelectedUser.grid;
                DoubleAnimation scaleYOld = new DoubleAnimation();
                scaleYOld.To = 1;
                scaleYOld.Duration = new Duration(span);
                Storyboard.SetTargetProperty(scaleYOld, "(UIElement.RenderTransform). (CompositeTransform.ScaleY)");
                Storyboard.SetTarget(scaleYOld, gridOld);
                storyboard.Children.Add(scaleYOld);
                DoubleAnimation scaleXOld = new DoubleAnimation();
                scaleXOld.To = 1;
                scaleXOld.Duration = new Duration(span);
                Storyboard.SetTargetProperty(scaleXOld, "(UIElement.RenderTransform). (CompositeTransform.ScaleX)");
                Storyboard.SetTarget(scaleXOld, gridOld);
                storyboard.Children.Add(scaleXOld);
            }

            Button each;
            for (int i = 0; i < UsersStack.Children.Count; i++)
            {
                each = UsersStack.Children[i] as Button; //如果类型不一致则返回null
                if (each != null)
                {
                    // doing......
                    if (each.Equals(SelectedUser))
                    {
                        DoubleAnimation transition = new DoubleAnimation();
                        transition.From = Canvas.GetLeft(UsersStack);
                        transition.To = 553 - i * (155);
                        transition.Duration = new Duration(new TimeSpan(0, 0, 0, 1));
                        BackEase ease = new BackEase();
                        ease.Amplitude = 1;
                        ease.EasingMode = EasingMode.EaseOut;
                        transition.EasingFunction = ease;
                        Storyboard.SetTargetProperty(transition, "(Canvas.Left)");
                        Storyboard.SetTarget(transition, UsersStack);
                        storyboard.Children.Add(transition);

                        storyboard.Begin();
                    }
                }
            }

        }
        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SignUp));
        }
        private void LoginButton_Click(object sende, RoutedEventArgs e)
        {
 
        }

    }
}
