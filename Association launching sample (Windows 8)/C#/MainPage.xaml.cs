// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.System;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Documents;
using AssociationLaunching;

namespace AssociationLaunching
{
    partial class rootPage : Page
    {
        #region Properties

        private Frame _scenariosFrame;

        public Frame ScenariosFrame
        {
            get { return _scenariosFrame; }
            set { _scenariosFrame = value; }
        }

        private Frame _inputFrame;

        public Frame InputFrame
        {
            get { return _inputFrame; }
            set { _inputFrame = value; }
        }

        private Frame _outputFrame;

        public Frame OutputFrame
        {
            get { return _outputFrame; }
            set { _outputFrame = value; }
        }

        private string _rootNamespace;

        public string RootNamespace
        {
            get { return _rootNamespace; }
            set { _rootNamespace = value; }
        }

        #endregion

        #region Events

        public event System.EventHandler InputFrameLoaded;
        public event System.EventHandler OutputFrameLoaded;

        #endregion

        private FileActivatedEventArgs _fileEventArgs = null;
        public FileActivatedEventArgs FileEvent
        {
            get { return _fileEventArgs; }
            set { _fileEventArgs = value; }
        }

        private ProtocolActivatedEventArgs _protocolEventArgs = null;
        public ProtocolActivatedEventArgs ProtocolEvent
        {
            get { return _protocolEventArgs; }
            set { _protocolEventArgs = value; }
        }

        public rootPage()
        {
            InitializeComponent();

            _scenariosFrame = ScenarioList;
            _inputFrame = ScenarioInput;
            _outputFrame = ScenarioOutput;

            SetFeatureName(FEATURE_NAME);

            Loaded += new RoutedEventHandler(MainPage_Loaded);
            Window.Current.SizeChanged += new WindowSizeChangedEventHandler(MainPage_SizeChanged);
            DisplayProperties.LogicalDpiChanged += new DisplayPropertiesEventHandler(DisplayProperties_LogicalDpiChanged);

            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Required;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Figure out what resolution and orientation we are in and respond appropriately
            CheckResolutionAndViewState();

            // Load the ScenarioList page into the proper frame
            ScenarioList.Navigate(Type.GetType(_rootNamespace + ".ScenarioList"), this);
        }

        #region Resolution and orientation code

        void DisplayProperties_LogicalDpiChanged(object sender)
        {
            CheckResolutionAndViewState();
        }

        void CheckResolutionAndViewState()
        {
            VisualStateManager.GoToState(this, ApplicationView.Value.ToString() + DisplayProperties.ResolutionScale.ToString(), false);
        }

        void MainPage_SizeChanged(Object sender, Windows.UI.Core.WindowSizeChangedEventArgs args)
                {
                         CheckResolutionAndViewState();
        }

        #endregion

        private void SetFeatureName(string str)
        {
            FeatureName.Text = str;
        }

        async void Footer_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(((HyperlinkButton)sender).Tag.ToString()));
        }

        public void NotifyUser(string strMessage, NotifyType type)
        {
            switch (type)
            {
                case NotifyType.StatusMessage:
                    StatusBlock.Style = Resources["StatusStyle"] as Style;
                    break;
                case NotifyType.ErrorMessage:
                    StatusBlock.Style = Resources["ErrorStyle"] as Style;
                    break;
            }
            StatusBlock.Text = strMessage;
        }

        public void DoNavigation(Type pageType, Frame frame)
        {
            frame.Navigate(pageType, this);
            if (pageType.Name.Contains("Input"))
            {
                // Raise InputFrameLoaded so downstream pages know that the input frame content has been loaded.
                if (InputFrameLoaded != null)
                {
                    InputFrameLoaded(this, new EventArgs());
                }
            }
            else
            {
                // Raise OutputFrameLoaded so downstream pages know that the output frame content has been loaded.
                if (OutputFrameLoaded != null)
                {
                    OutputFrameLoaded(this, new EventArgs());
                }
            }
        }
    }

    public enum NotifyType
    {
        StatusMessage,
        ErrorMessage
    };
}
