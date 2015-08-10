using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Reporting;
using Maui.Trading.Model;
using Maui.Trading.Indicators;
using Blade.Collections;
using Maui.Entities;

namespace Maui.Trading.Modules.Indicators
{
    public class GenericIndicatorReport : Report
    {
        private StockHandle myStock;

        public class Data
        {
            public Data()
            {
                Points = new Dictionary<string, IPriceSeries>();
            }

            public IPriceSeries Prices
            {
                get;
                set;
            }

            public Dictionary<string, IPriceSeries> Points
            {
                get;
                set;
            }

            public ISignalSeries Signals
            {
                get;
                set;
            }

            public Signal SignalOfDayUnderAnalysis
            {
                get;
                set;
            }
        }

        public GenericIndicatorReport( string name, string title, StockHandle stock, Data data )
            : base( name, title )
        {
            myStock = stock;

            Generate( data );
        }

        private void Generate( Data data )
        {
            AddOverviewSection( data );
            AddPointsSection( data );
            AddSignals( data );
            AddChart( data );
        }

        private void AddOverviewSection( Data data )
        {
            var section = new KeyValueSection( "Overview" );

            if ( data.SignalOfDayUnderAnalysis.Type == SignalType.None )
            {
                // add hint to the user that there was missing data to calc a current signal
                section.Entries[ "Current signal" ] = "None (Not enough data to calculate signal)";
            }
            else
            {
                section.Entries[ "Current signal" ] = data.SignalOfDayUnderAnalysis;
            }

            Sections.Add( section );
        }

        private void AddPointsSection( Data data )
        {
            foreach ( var entry in data.Points )
            {
                var section = new IndicatorPointsSection( entry.Key, entry.Value );
                Sections.Add( section );
            }
        }

        private void AddSignals( Data data )
        {
            var section = new SignalSeriesSection( Name, data.Signals );
            Sections.Add( section );
        }

        private void AddChart( Data data )
        {
            var chart = new StockPriceChart( Name, myStock, data.Prices );

            foreach ( var entry in data.Points )
            {
                chart.IndicatorPoints.Add( entry.Key, entry.Value );
            }

            chart.Signals = data.Signals;

            var section = new GenericChartSection( Name, chart );
            Sections.Add( section );
        }
    }
}
