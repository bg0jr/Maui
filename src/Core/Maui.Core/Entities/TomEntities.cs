using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;

namespace Maui.Entities
{
    internal partial class TomEntities : IEntityRepository
    {
        IObjectSet<StockCatalog> IEntityRepository.StockCatalogs { get { return StockCatalogs; } }

        IObjectSet<Company> IEntityRepository.Companies { get { return Companies; } }

        IObjectSet<Currency> IEntityRepository.Currencies { get { return Currencies; } }

        IObjectSet<DatumOrigin> IEntityRepository.DatumOrigins { get { return DatumOrigins; } }

        IObjectSet<Sector> IEntityRepository.Sectors { get { return Sectors; } }

        IObjectSet<Stock> IEntityRepository.Stocks { get { return Stocks; } }

        IObjectSet<StockExchange> IEntityRepository.StockExchanges { get { return StockExchanges; } }

        IObjectSet<StockPrice> IEntityRepository.StockPrices { get { return StockPrices; } }

        IObjectSet<TradedStock> IEntityRepository.TradedStocks { get { return TradedStocks; } }

        IObjectSet<Order> IEntityRepository.Orders { get { return Orders; } }

        IObjectSet<Portfolio> IEntityRepository.Portfolios { get { return Portfolios; } }

        IObjectSet<Position> IEntityRepository.Positions { get { return Positions; } }

        IObjectSet<Country> IEntityRepository.Countries { get { return Countries; } }

        IObjectSet<CountryAlias> IEntityRepository.CountryAliases { get { return CountryAliases; } }

        IObjectSet<SectorAlias> IEntityRepository.SectorAliases { get { return SectorAliases; } }
    }
}
