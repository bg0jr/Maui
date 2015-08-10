using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Metadata.Edm;
using System.Data;
using Maui.Reflection;
using Maui.Entities;

namespace Maui.UnitTests.Entities
{
    public class InMemoryEntityRepository : IEntityRepository
    {
        private static IEntityRepository myDB = new InMemoryEntityRepository();

        public InMemoryEntityRepository()
        {
            if ( myDB != null )
            {
                StockCatalogs = new InMemoryObjectSet<StockCatalog>( myDB.StockCatalogs );
                Companies = new InMemoryObjectSet<Company>( myDB.Companies );
                Currencies = new InMemoryObjectSet<Currency>( myDB.Currencies );
                DatumOrigins = new InMemoryObjectSet<DatumOrigin>( myDB.DatumOrigins );
                Sectors = new InMemoryObjectSet<Sector>( myDB.Sectors );
                Stocks = new InMemoryObjectSet<Stock>( myDB.Stocks );
                StockExchanges = new InMemoryObjectSet<StockExchange>( myDB.StockExchanges );
                StockPrices = new InMemoryObjectSet<StockPrice>( myDB.StockPrices );
                TradedStocks = new InMemoryObjectSet<TradedStock>( myDB.TradedStocks );
                Orders = new InMemoryObjectSet<Order>( myDB.Orders );
                Portfolios = new InMemoryObjectSet<Portfolio>( myDB.Portfolios );
                Positions = new InMemoryObjectSet<Position>( myDB.Positions );
                Countries = new InMemoryObjectSet<Country>( myDB.Countries );
                CountryAliases = new InMemoryObjectSet<CountryAlias>( myDB.CountryAliases );
                SectorAliases = new InMemoryObjectSet<SectorAlias>( myDB.SectorAliases );
            }
            else
            {
                StockCatalogs = new InMemoryObjectSet<StockCatalog>();
                Companies = new InMemoryObjectSet<Company>();
                Currencies = new InMemoryObjectSet<Currency>();
                DatumOrigins = new InMemoryObjectSet<DatumOrigin>();
                Sectors = new InMemoryObjectSet<Sector>();
                Stocks = new InMemoryObjectSet<Stock>();
                StockExchanges = new InMemoryObjectSet<StockExchange>();
                StockPrices = new InMemoryObjectSet<StockPrice>();
                TradedStocks = new InMemoryObjectSet<TradedStock>();
                Orders = new InMemoryObjectSet<Order>();
                Portfolios = new InMemoryObjectSet<Portfolio>();
                Positions = new InMemoryObjectSet<Position>();
                Countries = new InMemoryObjectSet<Country>();
                CountryAliases = new InMemoryObjectSet<CountryAlias>();
                SectorAliases = new InMemoryObjectSet<SectorAlias>();
            }
        }

        public IObjectSet<StockCatalog> StockCatalogs
        {
            get;
            private set;
        }

        public IObjectSet<Company> Companies
        {
            get;
            private set;
        }

        public IObjectSet<Currency> Currencies
        {
            get;
            private set;
        }

        public IObjectSet<DatumOrigin> DatumOrigins
        {
            get;
            private set;
        }

        public IObjectSet<Sector> Sectors
        {
            get;
            private set;
        }

        public IObjectSet<Stock> Stocks
        {
            get;
            private set;
        }

        public IObjectSet<StockExchange> StockExchanges
        {
            get;
            private set;
        }

        public IObjectSet<StockPrice> StockPrices
        {
            get;
            private set;
        }

        public IObjectSet<TradedStock> TradedStocks
        {
            get;
            private set;
        }

        public IObjectSet<Order> Orders
        {
            get;
            private set;
        }

        public IObjectSet<Portfolio> Portfolios
        {
            get;
            private set;
        }

        public IObjectSet<Position> Positions
        {
            get;
            private set;
        }

        public IObjectSet<Country> Countries
        {
            get;
            private set;
        }

        public IObjectSet<CountryAlias> CountryAliases
        {
            get;
            private set;
        }

        public IObjectSet<SectorAlias> SectorAliases
        {
            get;
            private set;
        }

        public MetadataWorkspace MetadataWorkspace
        {
            get;
            private set;
        }

        public object GetObjectByKey( EntityKey key )
        {
            throw new NotImplementedException();
        }

        public void AddObject( string entitySetName, object entity )
        {
            throw new NotImplementedException();
        }

        public void DeleteObject( object entity )
        {
            throw new NotImplementedException();
        }

        public int SaveChanges()
        {
            return SaveChanges( SaveOptions.None );
        }

        public int SaveChanges( SaveOptions options )
        {
            CopyTo( StockCatalogs, myDB.StockCatalogs );
            CopyTo( Companies, myDB.Companies );
            CopyTo( Currencies, myDB.Currencies );
            CopyTo( DatumOrigins, myDB.DatumOrigins );
            CopyTo( Sectors, myDB.Sectors );
            CopyTo( Stocks, myDB.Stocks );
            CopyTo( StockExchanges, myDB.StockExchanges );
            CopyTo( StockPrices, myDB.StockPrices );
            CopyTo( TradedStocks, myDB.TradedStocks );
            CopyTo( Orders, myDB.Orders );
            CopyTo( Portfolios, myDB.Portfolios );
            CopyTo( Positions, myDB.Positions );
            CopyTo( Countries, myDB.Countries );
            CopyTo( CountryAliases, myDB.CountryAliases );
            CopyTo( SectorAliases, myDB.SectorAliases );

            return 0;
        }

        private void CopyTo<T>( IObjectSet<T> source, IObjectSet<T> target ) where T : class
        {
            foreach ( var entity in source )
            {
                target.AddObject( entity );
            }
        }

        public void Dispose()
        {
            var objectSetProperties = GetType().GetProperties()
                .Where( pi => IsObjectSet( pi.PropertyType ) );
            foreach ( var pi in objectSetProperties )
            {
                pi.SetValue( this, null, null );
            }
        }

        private bool IsObjectSet( Type type )
        {
            var typeDef = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
            return typeof( IObjectSet<> ).IsAssignableFrom( typeDef );
        }
    }
}
