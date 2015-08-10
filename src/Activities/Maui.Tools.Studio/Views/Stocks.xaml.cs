using System;
using System.Linq;
using System.Windows;
using Maui.Entities;
using Maui;
using System.ComponentModel;

namespace Maui.Tools.Studio.Views
{
    /// <summary>
    /// Interaction logic for Stocks.xaml
    /// </summary>
    public partial class Stocks : Window
    {
        private IEntityRepository myTom;

        public Stocks()
        {
            InitializeComponent();

            myStocksTable.AutoGenerateColumns = false;
            myStocksTable.CanUserAddRows = false;
            myStocksTable.CanUserDeleteRows = true;
            myStocksTable.CanUserReorderColumns = true;
            //myStocksTable.CanUserSortColumns = true;

            myTom = Engine.ServiceProvider.CreateEntityRepository();
        }

        private void myCancelBtn_Click( object sender, RoutedEventArgs e )
        {
            Close();
        }

        private void myOkBtn_Click( object sender, RoutedEventArgs e )
        {
            myTom.SaveChanges();
            Close();
        }

        protected override void OnClosed( EventArgs e )
        {
            base.OnClosed( e );

            myTom.Dispose();
        }
        
        private void Window_Loaded( object sender, RoutedEventArgs e )
        {
            myStocksTable.DataContext = myTom;
            myStocksTable.ItemsSource = myTom.TradedStocks.OrderBy( s => s.Stock.Company.Name );
            //myStocksTable.Items.SortDescriptions.Add( new SortDescription( "Company", ListSortDirection.Ascending ) );
        }

        private void myAddBtn_Click( object sender, RoutedEventArgs e )
        {
            var newStockDlg = new NewStock();
            newStockDlg.Show();
        }
    }
}
