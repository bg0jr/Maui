using System;
using System.Collections.Generic;
using Blade;
using Maui;
using Maui.Entities;

namespace Maui.Dynamics.Data
{
    /// <summary>
    /// Series of stock prices.
    /// <remarks>
    /// The series is always ordered by date
    /// TODO: no unittest till now because will be replaced by series of Maui.Trading building block
    /// </remarks>
    /// </summary>
    public class StockPriceSeries : AbstractTimeSeries<StockPrice, StockPrice>
    {
        private DateTime? myCachedDate = null;
        private int myCachedIndex = -1;

        public StockPriceSeries( TradedStock tradedStock, IEnumerable<StockPrice> prices )
            : this( tradedStock, prices, Timeframes.DAY )
        {
        }

        public StockPriceSeries( TradedStock tradedStock, IEnumerable<StockPrice> prices, Timeframe timeframe )
            : base( prices )
        {
            this.Require( x => timeframe != null );

            TradedStock = tradedStock;
            Timeframe = timeframe;
        }

        public TradedStock TradedStock { get; private set; }

        /// <summary>
        /// Defines the time frame used for the prices.
        /// </summary>
        public Timeframe Timeframe { get; private set; }

        /// <summary>
        /// Return true if the object has prices for the corresponding date.
        /// </summary>
        public override bool Contains( DateTime date )
        {
            int idx = IndexOf( date );
            if ( idx == -1 )
            {
                return false;
            }

            // cache the value because often we will ask for the value
            // after having called Contains()
            myCachedDate = date;
            myCachedIndex = idx;

            return true;
        }

        public override int IndexOf( DateTime date )
        {
            if ( myCachedDate == date ) return myCachedIndex;

            return base.IndexOf( date );
        }

        /// <summary>
        /// Creates a new Prices object using the new timeframe by merging the
        /// required prices. You can only convert to a larger timeframe.
        /// </summary>
        public StockPriceSeries ConvertToTimeframe( Timeframe timeframe )
        {
            // #WAR# WARN "new timeframe must be larger" unless ($timeframe > $self->timeframe);

            var prices = new List<StockPrice>();

            // Initialize the iteration
            double? open = null;
            double? high = null;
            double? low = null;
            double? close = null;
            long? volume = null;
            DateTime? prevdate = null;

            // Iterate over all the prices (hope they are sorted)
            foreach ( var q in this )
            {
                // Build the date in the new timeframe corresponding to the prices
                // being treated
                var newdate = Timeframes.ConvertDate( q.Date, Timeframe, timeframe );

                // If the date differs from the previous one then we have completed
                // a new item
                if ( prevdate != null && newdate != prevdate.Value )
                {
                    // Store the new item
                    prices.Add( new StockPrice( TradedStock, prevdate.Value, open, high, low, close.Value, volume ) );

                    // Initialize the open/high/low/close with the following item
                    open = q.Open;
                    high = q.High;
                    low = q.Low;
                    close = q.Close;
                    volume = 0;
                }
                // Update the data of the item that is being built
                if ( high.HasValue && q.High.HasValue ) high = Math.Max( q.High.Value, high.Value );
                if ( low.HasValue && q.Low.HasValue ) low = Math.Min( q.Low.Value, low.Value );
                close = q.Close;
                if ( volume.HasValue )
                {
                    if ( q.Volume.HasValue )
                    {
                        volume += q.Volume.Value;
                    }
                }

                //Update the previous date
                prevdate = newdate;
            }

            // Store the last item
            if ( close.HasValue )
            {
                prices.Add( new StockPrice( TradedStock, prevdate.Value, open, high, low, close.Value, volume ) );
            }

            return new StockPriceSeries( TradedStock, prices, timeframe );
        }

    }
}
