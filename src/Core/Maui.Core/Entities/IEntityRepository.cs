using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Metadata.Edm;
using System.Data;

namespace Maui.Entities
{
    public interface IEntityRepository : IDisposable
    {
        IObjectSet<StockCatalog> StockCatalogs { get; }
        IObjectSet<Company> Companies { get; }
        IObjectSet<Currency> Currencies { get; }
        IObjectSet<DatumOrigin> DatumOrigins { get; }
        IObjectSet<Sector> Sectors { get; }
        IObjectSet<Stock> Stocks { get; }
        IObjectSet<StockExchange> StockExchanges { get; }
        IObjectSet<StockPrice> StockPrices { get; }
        IObjectSet<TradedStock> TradedStocks { get; }
        IObjectSet<Order> Orders { get; }
        IObjectSet<Portfolio> Portfolios { get; }
        IObjectSet<Position> Positions { get; }
        IObjectSet<Country> Countries { get; }
        IObjectSet<CountryAlias> CountryAliases { get; }
        IObjectSet<SectorAlias> SectorAliases { get; }

        MetadataWorkspace MetadataWorkspace { get; }
        object GetObjectByKey( EntityKey key );

        void AddObject( string entitySetName, object entity );
        void DeleteObject( object entity );

        int SaveChanges();
        int SaveChanges( SaveOptions options );
    }
}
