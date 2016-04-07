using CodingGame.Common;
using MyToolkit.Multimedia;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Syndication;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace CodingGame
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class VideoList : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public VideoList()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        int level = 1;
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            var DB = new SQLiteConnection(App.DBpath);
            //DB.Insert(new ZooT() { ID = 1, name = "Dog", desc = "a" });
            //DB.DeleteAll<ZooT>();
            //var query = DB.Table<ZooT>();
            var query = DB.Table<ZooT>().Where(x => x.ID == 1);
            var list = query.ToList();
            if(list.Count > 0)
                level = list[0].Level;
            else
                DB.Insert(new ZooT() { ID = 1, Level = 1});

            YoutubeVideo lockedVideo = new YoutubeVideo();
            lockedVideo.Title = "Locked Level";
           
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                try
                {
                    int index = 1;
                    int max = 50;

                    progressRing.Visibility = Visibility.Visible;
                    playlist.Visibility = Visibility.Collapsed;

                    string playlistId = "PLXbTW-KnvoWTiTjRM9DsI_hEPDsc_kQo4";
                    List<YoutubeVideo> videos = await getPlaylist("http://gdata.youtube.com/feeds/base/playlists/" + playlistId + "?max-results=" + max + "&start-index=" + index);
                    List<YoutubeVideo> newVideos = new List<YoutubeVideo>();

                    for (int i = 0; i < level; i++)
                    {
                        newVideos.Add(videos[i]);
                    }

                    for (int i = level; i < videos.Count; i++)
                    {
                        newVideos.Add(lockedVideo);
                    }
                    playlist.ItemsSource = newVideos;

                    progressRing.Visibility = Visibility.Collapsed;
                    playlist.Visibility = Visibility.Visible;
                }
                catch { }
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
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
        private void playlist_ItemClick(object sender, ItemClickEventArgs e)
        {
            YoutubeVideo video = e.ClickedItem as YoutubeVideo;

            if (video != null && video.Title != "Locked Level")
                this.Frame.Navigate(typeof(PickVideoOrQuiz), video);
        }

        private async Task<List<YoutubeVideo>> getPlaylist(String url)
        {
            try
            {
                SyndicationClient client = new SyndicationClient();
                SyndicationFeed feed = await client.RetrieveFeedAsync(new Uri(url));

                List<YoutubeVideo> list = new List<YoutubeVideo>();

                int i = 0;
                foreach (SyndicationItem item in feed.Items)
                {
                    YoutubeVideo video = new YoutubeVideo();
                    video.Title = item.Title.Text;
                    video.Date = item.PublishedDate.DateTime;
                    video.YoutubeLink = item.Links[0].Uri;
                    string a = video.YoutubeLink.ToString().Remove(0, 31);
                    video.ID = a.Substring(0, 11);
                    video.Image = YouTube.GetThumbnailUri(video.ID, YouTubeThumbnailSize.Small);
                    video.index = i;
                    list.Add(video);
                    i++;
                }

                return list;
            }
            catch
            {
                return null;
            }
        }
    }
}
