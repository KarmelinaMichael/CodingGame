using CodingGame.Common;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class Quiz : Page
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


        public Quiz()
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
        int counter = 0;
        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            var DB = new SQLiteConnection(App.DBpath);
            var query = DB.Table<ZooT>().Where(x => x.ID == 2);
            var list = query.ToList();
            string filename = e.NavigationParameter as string;
            await ReadFileString(filename);
            splitQ();
            putQ();
            if (list.Count > 0)
            txt.Text = list[0].Coins.ToString();
           
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        /// 
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
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
        int trials = 0;
        private void ok_Click(object sender, RoutedEventArgs e)
        {
            var DB = new SQLiteConnection(App.DBpath);
            var query = DB.Table<ZooT>().Where(x => x.ID == 2);
            var list = query.ToList();
            Answer.Text = "\n" + Answer.Text + "\n"; 
            
            if (Answer.Text.Contains(q[counter].answer))
            {
                Correct.Text = "اجابة صحيحة";
                if (list.Count > 0)
            {
                txt.Text = list[0].Coins.ToString();
            }
            else txt.Text = "0";
                counter++;
               
                putQ();
                Correct.Text = "";
                Answer.Text = "";
            }
            else
            {
                Correct.Text = "اجابة خاطئة";
                trials++;
            }
            
        }

        string content;
        private async Task ReadFileString(String filename)
        {
            StorageFolder Folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            var file = await Folder.GetFileAsync("Assets\\files\\" + filename);
            content = await Windows.Storage.FileIO.ReadTextAsync(file);
        }

        List<Questions> q = new List<Questions>();
        string[] allRecipes;
        private void splitQ()
        {
            allRecipes = Regex.Split(content, "new");
            foreach (string r in allRecipes)
            {
                string[] s = Regex.Split(r, "answer");
         
                Questions recipe = new Questions { text = s[0], answer = s[1]};
                q.Add(recipe);
            }
        }

        private async void putQ()
        { if(counter< q.Count)
            Question.Text = q[counter].text;
        else
        {
            int l = 1;
            int coins = 0;
            media1.Play() ;
            var dialog = new MessageDialog(" لقد انهيت المرحلة ");
             await dialog.ShowAsync();

             var DB = new SQLiteConnection(App.DBpath);
             var query = DB.Table<ZooT>().Where(x => x.ID == 2);
             var list = query.ToList();
             if (list.Count == 1)
             {
                 l = list[0].Level;
                 coins = list[0].Coins;
             }
             if (trials == 0)
                 coins = coins + 50;
             else if (trials == 1)
                 coins = coins + 40;
             else if (trials == 2)
                 coins = coins + 30;
             else if (trials == 2)
                 coins = coins + 20;
             else
                 coins = coins + 10;
            // media.Play();
             DB.Delete<ZooT>(2);
             DB.Insert(new ZooT() { ID = 2, Level = l + 1, Coins = coins });
            

             this.Frame.Navigate(typeof(Levels));           
        }

        }

        private void Correct_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
        
        private void share_button(object sender, RoutedEventArgs e)
        {
            RegisterForShare();
            DataTransferManager.ShowShareUI();
        }
        private void RegisterForShare()
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager,
                DataRequestedEventArgs>(this.ShareTextHandler);
        }

        private void ShareTextHandler(DataTransferManager sender, DataRequestedEventArgs e)
        {
            DataRequest request = e.Request;
            request.Data.Properties.Title = "Share Text Example";
            request.Data.Properties.Description = "A demonstration that shows how to share text.";
            request.Data.SetText(Question.Text);
        }


    }
}
