using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Template
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private SplashScreen splash;
        private Rect splashImageRect;

        //Place holder for our default screeen size
        private double defaultWidth = Window.Current.Bounds.Width;
        private double defaultHeight = Window.Current.Bounds.Height;
        

        public MainPage(SplashScreen splashScreen)
        {
            this.InitializeComponent();

            splash = splashScreen;
            OnResize();
            Window.Current.SizeChanged += new WindowSizeChangedEventHandler((o, e) => OnResize());

        }


        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            splash = (SplashScreen)e.Parameter;
            OnResize();
        }

        private void OnResize()
        {
            if (splash != null)
            {
                splashImageRect = splash.ImageLocation;
                PositionImage();
            }

            /// <summary>
            /// Added to handle screen size change. Pauses the game
            /// </summary>
            /// This is a simple example of how to handle screen resize
            /// for our game. If the game window is half size or less we pause 
            if(GameController.SP != null)
            { 
                if(Window.Current.Bounds.Width <= defaultWidth /2)
                {
                    GameController.SP.paused();
                }
                else if (Window.Current.Bounds.Width > defaultWidth / 2)
                {
                    GameController.SP.unpaused();
                }
            }
        }

        private void PositionImage()
        {
            ExtendedSplashImage.SetValue(Canvas.LeftProperty, splashImageRect.X);
            ExtendedSplashImage.SetValue(Canvas.TopProperty, splashImageRect.Y);
            ExtendedSplashImage.Height = splashImageRect.Height;
            ExtendedSplashImage.Width = splashImageRect.Width;
        }

		public SwapChainBackgroundPanel GetSwapChainBackgroundPanel()
		{
			return DXSwapChainPanel;
		}

        public void RemoveSplashScreen()
        {
            DXSwapChainPanel.Children.Remove(ExtendedSplashImage);
        }
    }
}
