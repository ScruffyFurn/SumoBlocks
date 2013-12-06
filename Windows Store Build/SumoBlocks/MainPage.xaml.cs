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

//added for share
using Windows.ApplicationModel.DataTransfer;
using UnityPlayer;
using System.Threading;

//Added for extended splash screen
using System.Threading.Tasks;

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

        //added for extended splash screen
        private DispatcherTimer extendedSplashTimer; 
        private bool isUnityLoaded; 

        //Place holder for our default screeen size
       // private double defaultWidth = Window.Current.Bounds.Width;
        //private double defaultHeight = Window.Current.Bounds.Height;

        //added for share
        DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();

        
       


        public MainPage(SplashScreen splashScreen)
        {
            this.InitializeComponent();

            splash = splashScreen;
            OnResize();
            Window.Current.SizeChanged += new WindowSizeChangedEventHandler((o, e) => OnResize());

            //Window Visibility event handler
            Window.Current.VisibilityChanged += OnWindowVisibilityChanged;

            //added for share
            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager,
            DataRequestedEventArgs>(this.ShareTextHandler);
            WindowsGateway.ShareHighScore = ShareHighScore;

            //added for extended splash screen
            // ensure we listen to when unity tells us game is ready             
            WindowsGateway.UnityLoaded = OnUnityLoaded;
 
            // create extended splash timer             
            extendedSplashTimer = new DispatcherTimer();
            extendedSplashTimer.Interval = TimeSpan.FromMilliseconds(100);
            extendedSplashTimer.Tick += ExtendedSplashTimer_Tick;
            extendedSplashTimer.Start();

        }

        /// <summary>         
        /// Control the extended splash experience
        /// </summary>         
        async void ExtendedSplashTimer_Tick(object sender, object e)
        {             
            var increment = extendedSplashTimer.Interval.TotalMilliseconds;
            if (!isUnityLoaded && SplashProgress.Value <= (SplashProgress.Maximum - increment))
            {                 
                SplashProgress.Value += increment;
            }             
            else             
            {                 
                SplashProgress.Value = SplashProgress.Maximum;
                await Task.Delay(250); 
                // force delay so user can see progress bar maxing out very briefly
                RemoveExtendedSplash();
            }
        } 
        /// <summary>         
        /// Unity has loaded and the game is playable
        /// </summary>
        private async void OnUnityLoaded()
        {             
            isUnityLoaded = true;         
        } 
        /// <summary>         
        /// Remove the extended splash
        /// </summary>         
        public void RemoveExtendedSplash()
        {             
            if (extendedSplashTimer != null)
            {                 
                extendedSplashTimer.Stop();
            }             
            if (DXSwapChainPanel.Children.Count > 0)
            {                 
                DXSwapChainPanel.Children.Remove(ExtendedSplashGrid);
            }         
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

        }

        private async void OnWindowVisibilityChanged(object sender, VisibilityChangedEventArgs e)
        {
            if (e.Visible)
            {
                if (AppCallbacks.Instance.IsInitialized())
                    AppCallbacks.Instance.UnityPause(0);
                return;
            }
            else
            {
                if (AppCallbacks.Instance.IsInitialized())
                {
                    AppCallbacks.Instance.UnityPause(1);
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

        //added for share
       
        private void ShareTextHandler(DataTransferManager sender, DataRequestedEventArgs e)
        {
            DataRequest request = e.Request;
            request.Data.Properties.Title = "Share HighScore Example";
            request.Data.Properties.Description = "A demonstration that shows how to share a highscore.";
            request.Data.SetText("I just got a new high score in SumoBlocks, I won in: " + GameController.Instance.GetHighScore().ToString("0.00"));
        }
        


        private static void ShareHighScore()
        {
            AppCallbacks.Instance.InvokeOnUIThread(() =>
            {
                DataTransferManager.ShowShareUI();
            }, false);
        }
    }
}
