using CodingGame.Common;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace CodingGame
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class Levels : Page
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


        public Levels()
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
        int l;
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            var DB = new SQLiteConnection(App.DBpath);
            var query = DB.Table<ZooT>().Where(x => x.ID == 2);
            var list = query.ToList();
            if (list.Count > 0)
            {
                Debug.WriteLine("in");
                l = list[0].Level;
                Debug.WriteLine("database level :" + list[0].Level);
            }
            Debug.WriteLine("level :" + l);
            if (l == 2)
                enableLevel2();
            else if (l == 3)
            {
                enableLevel2();
                enableLevel3();
            }
            else if (l == 4)
            {
                enableLevel2();
                enableLevel3();
                enableLevel4();
            }
            else if (l == 5)
            {
                enableLevel2();
                enableLevel3();
                enableLevel4();
                enableLevel5();
            }
            else if (l == 6)
            {
                enableLevel2();
                enableLevel3();
                enableLevel4();
                enableLevel5();
                enableLevel6();
            }
            else if (l == 7)
            {
                enableLevel2();
                enableLevel3();
                enableLevel4();
                enableLevel5();
                enableLevel6();
                enableLevel7();
            }
            else if (l == 8)
            {
                enableLevel2();
                enableLevel3();
                enableLevel4();
                enableLevel5();
                enableLevel6();
                enableLevel7();
                enableLevel8();
            }
            else if (l == 9)
            {
                enableLevel2();
                enableLevel3();
                enableLevel4();
                enableLevel5();
                enableLevel6();
                enableLevel7();
                enableLevel8();
                enableLevel9();
            }
            else if (l == 10)
            {
                enableLevel2();
                enableLevel3();
                enableLevel4();
                enableLevel5();
                enableLevel6();
                enableLevel7();
                enableLevel8();
                enableLevel9();
                enableLevel10();
            }
            else if (l == 11)
            {
                enableLevel2();
                enableLevel3();
                enableLevel4();
                enableLevel5();
                enableLevel6();
                enableLevel7();
                enableLevel8();
                enableLevel9();
                enableLevel10();
                enableLevel11();
            }
            else if (l == 12)
            {
                enableLevel2();
                enableLevel3();
                enableLevel4();
                enableLevel5();
                enableLevel6();
                enableLevel7();
                enableLevel8();
                enableLevel9();
                enableLevel10();
                enableLevel11();
                enableLevel12();
            }
            else if (l == 13)
            {
                enableLevel2();
                enableLevel3();
                enableLevel4();
                enableLevel5();
                enableLevel6();
                enableLevel7();
                enableLevel8();
                enableLevel9();
                enableLevel10();
                enableLevel11();
                enableLevel12();
                enableLevel13();
            }
            else if (l == 14)
            {
                enableLevel2();
                enableLevel3();
                enableLevel4();
                enableLevel5();
                enableLevel6();
                enableLevel7();
                enableLevel8();
                enableLevel9();
                enableLevel10();
                enableLevel11();
                enableLevel12();
                enableLevel13();
                enableLevel14();
            }
            else if (l == 15)
            {
                enableLevel2();
                enableLevel3();
                enableLevel4();
                enableLevel5();
                enableLevel6();
                enableLevel7();
                enableLevel8();
                enableLevel9();
                enableLevel10();
                enableLevel11();
                enableLevel12();
                enableLevel13();
                enableLevel14();
                enableLevel15();
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

        string level;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            level = "level0.txt";
            this.Frame.Navigate(typeof(Quiz), level);
        }

        private void level2_Click(object sender, RoutedEventArgs e)
        {
            level = "level1.txt";
            this.Frame.Navigate(typeof(Quiz), level);
        }

        private void enableLevel2()
        {
            level2.IsEnabled = true;
            level2.IsTapEnabled = true;
            // level2.Background = new SolidColorBrush(Color.FromArgb(100, 255, 197,0));
        }

        private void level3_Click(object sender, RoutedEventArgs e)
        {
            level = "level2.txt";
            this.Frame.Navigate(typeof(Quiz), level);
        }
        private void level4_Click(object sender, RoutedEventArgs e)
        {
            level = "level3.txt";
            this.Frame.Navigate(typeof(Quiz), level);
        }
        private void level5_Click(object sender, RoutedEventArgs e)
        {
            level = "level4.txt";
            this.Frame.Navigate(typeof(Quiz), level);
        }
        private void level6_Click(object sender, RoutedEventArgs e)
        {
            level = "level5.txt";
            this.Frame.Navigate(typeof(Quiz), level);
        }
        private void level7_Click(object sender, RoutedEventArgs e)
        {
            level = "level6.txt";
            this.Frame.Navigate(typeof(Quiz), level);
        }
        private void level8_Click(object sender, RoutedEventArgs e)
        {
            level = "tracing.txt";
            this.Frame.Navigate(typeof(Quiz), level);
        }
        private void level9_Click(object sender, RoutedEventArgs e)
        {
            level = "tracing.txt";
            this.Frame.Navigate(typeof(Quiz), level);
        }
        private void level10_Click(object sender, RoutedEventArgs e)
        {
            level = "tracing.txt";
            this.Frame.Navigate(typeof(Quiz), level);
        }
        private void level11_Click(object sender, RoutedEventArgs e)
        {
            level = "tracing.txt";
            this.Frame.Navigate(typeof(Quiz), level);
        }
        private void level12_Click(object sender, RoutedEventArgs e)
        {
            level = "tracing.txt";
            this.Frame.Navigate(typeof(Quiz), level);
        }
        private void level13_Click(object sender, RoutedEventArgs e)
        {
            level = "tracing.txt";
            this.Frame.Navigate(typeof(Quiz), level);
        }
        private void level14_Click(object sender, RoutedEventArgs e)
        {
            level = "tracing.txt";
            this.Frame.Navigate(typeof(Quiz), level);
        }
        private void level15_Click(object sender, RoutedEventArgs e)
        {
            level = "tracing.txt";
            this.Frame.Navigate(typeof(Quiz), level);
        }

        private void enableLevel3()
        {
            level3.IsEnabled = true;
            level3.IsTapEnabled = true;

        }

        private void enableLevel4()
        {
            level4.IsEnabled = true;
            level4.IsTapEnabled = true;

        }

        private void enableLevel5()
        {
            level5.IsEnabled = true;
            level5.IsTapEnabled = true;

        }

        private void enableLevel6()
        {
            level6.IsEnabled = true;
            level6.IsTapEnabled = true;

        }
        private void enableLevel7()
        {
            level7.IsEnabled = true;
            level7.IsTapEnabled = true;

        }
        private void enableLevel8()
        {
            level8.IsEnabled = true;
            level8.IsTapEnabled = true;

        }
        private void enableLevel9()
        {
            level9.IsEnabled = true;
            level9.IsTapEnabled = true;

        }
        private void enableLevel10()
        {
            level10.IsEnabled = true;
            level10.IsTapEnabled = true;

        }
        private void enableLevel11()
        {
            level11.IsEnabled = true;
            level11.IsTapEnabled = true;

        }
        private void enableLevel12()
        {
            level12.IsEnabled = true;
            level12.IsTapEnabled = true;

        }
        private void enableLevel13()
        {
            level13.IsEnabled = true;
            level13.IsTapEnabled = true;

        }
        private void enableLevel14()
        {
            level14.IsEnabled = true;
            level14.IsTapEnabled = true;

        }
        private void enableLevel15()
        {
            level15.IsEnabled = true;
            level15.IsTapEnabled = true;

        }

       

    }
}
