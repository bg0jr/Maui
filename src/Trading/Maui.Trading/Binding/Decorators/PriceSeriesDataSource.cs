using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;
using Maui.Trading.Data;
using Maui.Entities;

namespace Maui.Trading.Binding.Decorators
{
    public class PriceSeriesDataSource : IPriceSeriesDataSource
    {
        public PriceSeriesDataSource( IEnumerableDataSource<SimplePrice> realDataSource, IEnumerable<IPriceSeriesOperator> operators )
        {
            if ( realDataSource == null )
            {
                throw new ArgumentNullException( "realDataSource" );
            }

            RealDataSource = realDataSource;
            Operators = operators;
        }

        public string Name
        {
            get { return RealDataSource.Name; }
        }

        public IEnumerableDataSource<SimplePrice> RealDataSource
        {
            get;
            private set;
        }

        public IEnumerable<IPriceSeriesOperator> Operators
        {
            get;
            private set;
        }

        public IPriceSeries ForStock( StockHandle stock )
        {
            var prices = RealDataSource.ForStock( stock );

            var series = CreateSeries( stock, prices );
            series = ApplyOperators( series );

            return series;
        }

        private IPriceSeries CreateSeries( StockHandle stock, IEnumerable<SimplePrice> prices )
        {
            var seriesId = new SeriesIdentifier(
                new StockObjectIdentifier( stock ),
                new ObjectDescriptor( Name ) );

            return new PriceSeries( seriesId, prices );
        }

        private IPriceSeries ApplyOperators( IPriceSeries series )
        {
            foreach ( var op in Operators )
            {
                series = op.Apply( series );
            }

            return series;
        }
    }
}
