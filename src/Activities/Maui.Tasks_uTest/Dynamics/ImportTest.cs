using System;
using System.Linq;
using Blade.Data;
using Blade.Reflection;
using Maui.Data.Recognition;
using Maui.Entities;
using Maui.Dynamics.Data;
using Maui;
using Maui.Tasks.Dynamics;
using Maui.Dynamics.Presets;
using NUnit.Framework;
using Maui.Data.Recognition.DatumLocators;
using Maui.Dynamics.UnitTest;
using Maui.Dynamics;
using Maui.Data.Recognition.Spec;

namespace Maui.Tasks.UnitTests.Dynamics
{
    [TestFixture]
    public class ImportTest : TestBase, IMslScript
    {
        public static Datum Eps = null;
        public static Datum Dividend = null;
        public static Datum StockPrice = null;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            Eps = DatumDefines.Eps.Clone( Let.Member( () => DatumDefines.Eps.IsPersistent ).Become( false ) );
            Dividend = DatumDefines.Dividend.Clone( Let.Member( () => DatumDefines.Dividend.IsPersistent ).Become( false ) );
            StockPrice = DatumDefines.StockPrice.Clone( Let.Member( () => DatumDefines.StockPrice.IsPersistent ).Become( false ) );
        }

        [Test( Description = "Tests ImportFunction for eps from web site" )]
        public void ImportEpsFromHtml()
        {
            PrepareImport();
            AddDummyStock();

            Interpreter.Context.Scope.Catalog = new StockCatalog( "catalog" );
            Interpreter.Context.Scope.Catalog.TradedStocks.Add( Interpreter.Context.Scope.Stock.TradedStock );

            this.Import( "eps" );

            var rows = Eps.Manager().Query( CurrentStockId ).Rows.ToList();

            Assert.AreEqual( 6, rows.Count );

            Assert.AreEqual( 2.78d, (double)rows[ 0 ][ "value" ], 0.000001d );
            Assert.AreEqual( 3.00d, (double)rows[ 1 ][ "value" ], 0.000001d );
            Assert.AreEqual( 2.89d, (double)rows[ 2 ][ "value" ], 0.000001d );
            Assert.AreEqual( 3.30d, (double)rows[ 3 ][ "value" ], 0.000001d );
            Assert.AreEqual( 3.33d, (double)rows[ 4 ][ "value" ], 0.000001d );
            Assert.AreEqual( 4.38d, (double)rows[ 5 ][ "value" ], 0.000001d );

            Assert.AreEqual( 2001, (int)rows[ 0 ][ "year" ] );
            Assert.AreEqual( 2002, (int)rows[ 1 ][ "year" ] );
            Assert.AreEqual( 2003, (int)rows[ 2 ][ "year" ] );
            Assert.AreEqual( 2004, (int)rows[ 3 ][ "year" ] );
            Assert.AreEqual( 2005, (int)rows[ 4 ][ "year" ] );
            Assert.AreEqual( 2006, (int)rows[ 5 ][ "year" ] );
        }

        [Test]
        public void ImportDividendFromDataSheet()
        {
            PrepareImport();
            AddDummyStock( "DE994776362" );

            Interpreter.Context.Scope.Catalog = new StockCatalog( "catalog" );
            Interpreter.Context.Scope.Catalog.TradedStocks.Add( Interpreter.Context.Scope.Stock.TradedStock );

            this.Import( "dividend" );

            var rows = Dividend.Manager().Query( CurrentStockId ).Rows.ToList();

            Assert.AreEqual( 10, rows.Count );

            Assert.AreEqual( 3.2d, (double)rows[ 0 ][ "value" ], 0.000001d );
            Assert.AreEqual( 3.4d, (double)rows[ 1 ][ "value" ], 0.000001d );
            Assert.AreEqual( 3.4d, (double)rows[ 2 ][ "value" ], 0.000001d );
            Assert.AreEqual( 3.3d, (double)rows[ 3 ][ "value" ], 0.000001d );
            Assert.AreEqual( 2.9d, (double)rows[ 4 ][ "value" ], 0.000001d );
            Assert.AreEqual( 2.8d, (double)rows[ 5 ][ "value" ], 0.000001d );
            Assert.AreEqual( 3.0d, (double)rows[ 6 ][ "value" ], 0.000001d );
            Assert.AreEqual( 3.0d, (double)rows[ 7 ][ "value" ], 0.000001d );
            Assert.AreEqual( 3.1d, (double)rows[ 8 ][ "value" ], 0.000001d );
            Assert.AreEqual( 3.5d, (double)rows[ 9 ][ "value" ], 0.000001d );

            Assert.AreEqual( 1997, (int)rows[ 0 ][ "year" ] );
            Assert.AreEqual( 1998, (int)rows[ 1 ][ "year" ] );
            Assert.AreEqual( 1999, (int)rows[ 2 ][ "year" ] );
            Assert.AreEqual( 2000, (int)rows[ 3 ][ "year" ] );
            Assert.AreEqual( 2001, (int)rows[ 4 ][ "year" ] );
            Assert.AreEqual( 2002, (int)rows[ 5 ][ "year" ] );
            Assert.AreEqual( 2003, (int)rows[ 6 ][ "year" ] );
            Assert.AreEqual( 2004, (int)rows[ 7 ][ "year" ] );
            Assert.AreEqual( 2005, (int)rows[ 8 ][ "year" ] );
            Assert.AreEqual( 2006, (int)rows[ 9 ][ "year" ] );
        }

        [Test( Description = "Tests ImportFunction for stock_prices from taipan file" )]
        public void ImportStockPricesFromTaipan()
        {
            PrepareImport();
            AddDummyStock( "DE994776362" );

            Interpreter.Context.Scope.Catalog = new StockCatalog( "catalog" );
            Interpreter.Context.Scope.Catalog.TradedStocks.Add( Interpreter.Context.Scope.Stock.TradedStock );

            this.Import( "stock_price" );

            var rows = StockPrice.Manager().Query( CurrentStockId ).Rows.ToList();

            Assert.AreEqual( 10, rows.Count );

            Assert.AreEqual( 16.2370d, (double)rows[ 0 ][ "close" ], 0.0001d );
            Assert.AreEqual( 16.6521d, (double)rows[ 1 ][ "close" ], 0.0001d );
            Assert.AreEqual( 16.8782d, (double)rows[ 2 ][ "close" ], 0.0001d );
            Assert.AreEqual( DBNull.Value, rows[ 2 ][ "volume" ] );
            Assert.AreEqual( 90731, (int)rows[ 3 ][ "volume" ] );
            Assert.AreEqual( 19.3200d, (double)rows[ 9 ][ "open" ], 0.0001d );
        }

        private void PrepareImport()
        {
            Interpreter.Context.Scope[ "TestDataDir" ] = TestDataRoot;

            Eps.Create();
            Dividend.Create();
            StockPrice.Create();

            var epsProvider = new DatumLocator( "eps",
                new Site( "Ariva",
                    new Navigation( DocumentType.Html, "${TestDataDir}/ariva.html" ),
                    new PathSeriesFormat( "Ariva.Eps" )
                    {
                        Path = @"/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]/TR[6]/TD[1]",
                        TimeAxisPosition = 1,
                        Expand = CellDimension.Row,
                        SeriesNamePosition = 0,
                        // seriesname-contains="unverwässertes Ergebnis pro Aktie">
                        ValueFormat = new FormatColumn( "value", typeof( double ), Format.PriceDE ),
                        TimeAxisFormat = new FormatColumn( "year", typeof( int ), Format.Number )
                    },
                    new DataContent( "Euro" ) ) );
            Interpreter.Context.DatumProviderFactory.LocatorRepository.Add( epsProvider );

            var dividendProvider = new DatumLocator( "dividend",
               new Site( "Sheet",
                    new Navigation( DocumentType.Text, @"${TestDataDir}/dividend.csv" ),
                    new SeparatorSeriesFormat( "Sheet.Dividend" )
                    {
                        Separator = ";",
                        Anchor = Anchor.ForRow( new StringContainsLocator( 1, "${stock.isin}" ) ),
                        TimeAxisPosition = 0,
                        Expand = CellDimension.Row,
                        SeriesNamePosition = 1,
                        SkipColumns = new int[] { 0, 2, 3 },
                        SkipRows = new[] { 1 },
                        ValueFormat = new FormatColumn( "value", typeof( double ), Format.PriceDE ),
                        TimeAxisFormat = new FormatColumn( "year", typeof( int ), Format.Number )
                    },
                    new DataContent( "Euro" ) ) );
            Interpreter.Context.DatumProviderFactory.LocatorRepository.Add( dividendProvider );

            var stockPriceProvider = new DatumLocator( "stock_price",
                new Site( "TaiPan",
                    new Navigation( DocumentType.Text, @"${TestDataDir}/*/555200.TXT" ),
                    new CsvFormat( "TaiPan.Prices", ";",
                        new FormatColumn( "date", typeof( DateTime ), "dd.MM.yyyy" ),
                        new FormatColumn( "close", typeof( double ), Format.PriceDE ),
                        new FormatColumn( "volume", typeof( int ), Format.PriceDE ),
                        new FormatColumn( "high", typeof( double ), Format.PriceDE ),
                        new FormatColumn( "low", typeof( double ), Format.PriceDE ),
                        new FormatColumn( "open", typeof( double ), Format.PriceDE ) ),
                    new DataContent( "Euro" ) ) );
            Interpreter.Context.DatumProviderFactory.LocatorRepository.Add( stockPriceProvider );

            using ( var tom = Engine.ServiceProvider.CreateEntityRepository() )
            {
                var cur = new Currency( "Euro", "Euro" );
                tom.Currencies.AddObject( cur );
                tom.SaveChanges();
            }
        }
    }
}
