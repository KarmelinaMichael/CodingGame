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

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace CodingGame
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Assesment : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public Assesment()
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
            YoutubeVideo video = e.NavigationParameter as YoutubeVideo;
            counter = video.index;
            await ReadFileString("cquiz.txt");
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
        private async void ok_Click(object sender, RoutedEventArgs e)
        {
            String s = "";
            Debug.WriteLine(q[counter].answer);
            // Debug.WriteLine("\n" + Answer.Text + "\n");
            if (AnswerA.IsChecked == true)
            {
                s = "A)";
            }
            else if (AnswerB.IsChecked == true)
            {
                s = "B)";
            }
            else if (AnswerC.IsChecked == true)
            {
                s = "C)";
            }
            else if (AnswerD.IsChecked == true)
            {
                s = "D)";
            }
            if (q[counter].answer.Contains(s))
            {
                /*Correct.Text = "اجابة صحيحة";
                counter++;
                putQ();
                Correct.Text = "";
                AnswerA.Content = "";*/
                counter++;
                var dialog = new MessageDialog(" الان يمكنك ان تشاهد الفيديو التالى ");
                await dialog.ShowAsync();
                int level = 1;
                var DB = new SQLiteConnection(App.DBpath);
                var query = DB.Table<ZooT>().Where(x => x.ID == 1);
                var list = query.ToList();
                if (list.Count > 0)
                    level = list[0].Level;
                if (counter == level)
                {
                    DB.Delete<ZooT>(1);
                    DB.Insert(new ZooT() { ID = 1, Level = level + 1 });
                }
                this.Frame.Navigate(typeof(VideoList));
            }
            else
            {
                Correct.Text = "اجابة خاطئة";
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

                var dialog = new MessageDialog(" الان يمكنك ان تشاهد الفيديو التالى ");
                await dialog.ShowAsync();
                int level = 1;
                var DB = new SQLiteConnection(App.DBpath);
                var query = DB.Table<ZooT>().Where(x => x.ID == 1);
                var list = query.ToList();
                if (list.Count > 0)
                    level = list[0].Level;
                DB.Delete<ZooT>(1);
                DB.Insert(new ZooT() { ID = 1, Level = level + 1 });
                this.Frame.Navigate(typeof(VideoList));
            }
        }

        private void Correct_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }


    }
}
