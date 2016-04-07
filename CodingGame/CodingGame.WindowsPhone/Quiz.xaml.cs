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
using Windows.Graphics.Display;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.DataTransfer;


// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace CodingGame
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Quiz : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public Quiz()
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
        int counter = 0;
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            string filename = e.NavigationParameter as string;
            await ReadFileString(filename);
            splitQ();
            putQ();
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

        int trials = 0;
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
        private void ok_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(q[counter].answer);
            Debug.WriteLine("\n" + Answer.Text + "\n");
            if (q[counter].answer.Contains(Answer.Text)&&Answer.Text!="")
            {
                Correct.Text = "اجابة صحيحة";
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

                Questions recipe = new Questions { text = s[0], answer = s[1] };
                q.Add(recipe);
            }
        }

        private async void putQ()
        {
            if (counter < q.Count)
                Question.Text = q[counter].text;
            else
            {
                int l = 1;
                int coins = 0;
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

                DB.Delete<ZooT>(2);
                DB.Insert(new ZooT() { ID = 2, Level = l + 1, Coins = coins });

                this.Frame.Navigate(typeof(Levels));           

            }
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
