using System;
using System.Windows;
using Maui;
using Maui.Tools.Studio.Views;
using Maui.Logging;
using Maui.Tools.Studio.WebSpy;
using Blade.Logging;

namespace Maui.Tools.Studio
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ILoggingSink
    {
        public MainWindow()
        {
            InitializeComponent();

            LoggerFactory.AddGuiAppender( this );
        }

        private void myCatalogsMenu_Click( object sender, RoutedEventArgs e )
        {
            var dlg = new Catalogs();
            dlg.Show();
        }

        private void myStocksMenu_Click( object sender, RoutedEventArgs e )
        {
            var stocks = new Stocks();
            stocks.Show();
        }

        private void myNewStockMenu_Click( object sender, RoutedEventArgs e )
        {
            var newStock = new NewStock();
            newStock.Show();
        }

        private void mySectorsMenu_Click( object sender, RoutedEventArgs e )
        {
            var sectors = new Sectors();
            sectors.Show();
        }

        public void Write( ILoggingEntry entry )
        {
            myLog.AppendText( entry.Message + Environment.NewLine );
        }

        private void myWebSpyMenu_Click( object sender, RoutedEventArgs e )
        {
            var dlg = new WebSpyWindow();
            dlg.Show();
        }
    }
}
