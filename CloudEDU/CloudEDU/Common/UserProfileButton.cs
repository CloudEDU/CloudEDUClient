using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using CloudEDU.Login;

// “用户控件”项模板在 http://go.microsoft.com/fwlink/?LinkId=234235 上有介绍

namespace CloudEDU.Common
{

    public sealed class UserProfileButton : Button
    {
        public User user
        {
            get;
            set;
        }

        public string UserName
        {
            get
            {
                return (string)GetValue(UserNameProperty);
            }
            set
            {
                SetValue(UserNameProperty, value);
            }
        }

        public static readonly DependencyProperty UserNameProperty =
        DependencyProperty.Register("UserName", typeof(string), typeof(UserProfileButton),
          null);

        public UserProfileButton()
        {
            this.DefaultStyleKey = typeof(UserProfileButton);
        }
    }
}
