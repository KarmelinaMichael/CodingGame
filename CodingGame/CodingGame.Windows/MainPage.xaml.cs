using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SQLite;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CodingGame
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    { 
        public MainPage()
        { 
            this.InitializeComponent();
           var DB = new SQLiteConnection(App.DBpath);
            //var query = DB.Table<ZooT>();
    DB.DeleteAll<ZooT>();
            
        }

        private void start_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(VideoList));
        }

        private void quizzes_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Levels));
        }

        private void dashboard_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Dashboard));
        }
    }
}
