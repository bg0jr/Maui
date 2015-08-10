using System;
using System.Data;
using System.Linq;
using System.Windows;
using Blade;
using Maui.Entities;
using Maui;
using Maui.Dynamics;
using Maui.Tasks.Dynamics;
using Maui.Dynamics.Presets;
using Maui.Data.Recognition.DatumLocators;
using Maui.Logging;
using Blade.Logging;

namespace Maui.Tools.Studio.Views
{
    /// <summary>
    /// Interaction logic for NewStock.xaml
    /// </summary>
    public partial class NewStock : Window, IMslScript
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( NewStock ) );

        private IEntityRepository myTom;

        public NewStock()
        {
            InitializeComponent();
            myTom = Engine.ServiceProvider.CreateEntityRepository();
        }

        private void myAddBtn_Click( object sender, RoutedEventArgs e )
        {
            AddStock();

            myTom.SaveChanges();

            ResetForm();
        }

        private void ResetForm()
        {
            myIsinTxt.Clear();
            myWpknTxt.Clear();
            mySymbolTxt.Clear();
            myCompanyNameTxt.Clear();
        }

        private void AddStock()
        {
            var stock = new Stock();
            stock.Isin = myIsinTxt.Text;
            stock.Company = new Company();
            stock.Company.Name = myCompanyNameTxt.Text;

            var tradedStock = new TradedStock( stock, myTom.StockExchanges.First( se => se.Symbol == "F" ) );
            tradedStock.Symbol = mySymbolTxt.Text;
            tradedStock.Wpkn = myWpknTxt.Text;

            myTom.Stocks.AddObject( stock );
        }

        private void myCancelBtn_Click( object sender, RoutedEventArgs e )
        {
            Close();
        }

        protected override void OnClosed( EventArgs e )
        {
            base.OnClosed( e );

            myTom.Dispose();
        }

        private void myFetchBtn_Click( object sender, RoutedEventArgs e )
        {
            mySymbolTxt.Text = string.Empty;
            myWpknTxt.Text = string.Empty;
            myCompanyNameTxt.Text = string.Empty;

            if ( string.IsNullOrEmpty( myIsinTxt.Text ) )
            {
                return;
            }

            var isin = myIsinTxt.Text.Trim();

            var stock = myTom.Stocks.FirstOrDefault( s => s.Isin == isin );
            if ( stock == null )
            {
                FetchStock( isin );
            }
            else
            {
                ShowStock( stock );
            }
        }

        private void FetchStock( string isin )
        {
            mySymbolTxt.Text = Fetch( isin, DatumLocatorDefinitions.Standing.StockSymbol.Name );
            myCompanyNameTxt.Text = Fetch( isin, DatumLocatorDefinitions.Standing.CompanyName.Name );
            myWpknTxt.Text = Fetch( isin, DatumLocatorDefinitions.Standing.Wpkn.Name );
        }

        private string Fetch( string isin, string standingValue )
        {
            try
            {
                return this.FetchSingle<string>( isin, standingValue ).Value;
            }
            catch ( Exception ex )
            {
                myLogger.Error( ex, "Failed to fetch: {0}{1}{2}", standingValue );
                return string.Empty;
            }
        }

        private void ShowStock( Stock stock )
        {
            if ( stock.EntityState != EntityState.Detached && stock.EntityState != EntityState.Added )
            {
                stock.CompanyReference.Load();
                stock.TradedStocks.Load();
            }

            if ( stock.TradedStocks.Count == 0 )
            {
                throw new ArgumentException( "TradedStock missing" );
            }
            if ( stock.TradedStocks.Count > 1 )
            {
                throw new ArgumentException( "Cannot handle multiple TradedStocks" );
            }

            var tradedStock = stock.TradedStocks.Single();

            mySymbolTxt.Text = tradedStock.Symbol;
            myWpknTxt.Text = tradedStock.Wpkn;
            myCompanyNameTxt.Text = stock.Company.Name;
        }

        private void Window_Loaded( object sender, RoutedEventArgs e )
        {
            myIsinTxt.Focus();
        }
    }
}
