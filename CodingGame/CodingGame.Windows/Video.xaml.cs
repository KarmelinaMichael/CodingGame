using CodingGame.Common;
using MyToolkit.Multimedia;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.DataTransfer;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace CodingGame
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class Video : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public Video()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        YouTubeUri url;
        private async void  navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {  
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                    progressRing.Visibility = Visibility.Visible;
                    player.Visibility = Visibility.Collapsed;
                
                    YoutubeVideo video = e.NavigationParameter as YoutubeVideo;
                    string videoId = String.Empty;
                    if(video != null && !video.ID.Equals(String.Empty))
                    {
                        Debug.WriteLine("1");
                         url = await YouTube.GetVideoUriAsync(video.ID, YouTubeQuality.Quality360P);
                        player.Source = url.Uri;
                        Debug.WriteLine("2");
                       
                    }

                    Debug.WriteLine("3");
                    progressRing.Visibility = Visibility.Collapsed;
                    player.Visibility = Visibility.Visible;
            }
            else
            {
                await new MessageDialog ("لا يمكن الاتصال بالانترنت!").ShowAsync();
            }
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        private void RegisterForShare()
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager,
                DataRequestedEventArgs>(this.ShareLinkHandler);
        }

        private  void ShareLinkHandler(DataTransferManager sender, DataRequestedEventArgs e)
        {
            
            DataRequest request = e.Request;
            request.Data.Properties.Title = "Share Link Example";
            request.Data.Properties.Description = "Demonstrates how to add a link to share.";
            request.Data.SetWebLink(new Uri(url.Uri.ToString()));
        }


        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void share_button(object sender, RoutedEventArgs e)
        {
            RegisterForShare();
            DataTransferManager.ShowShareUI();
        }
    }
}
