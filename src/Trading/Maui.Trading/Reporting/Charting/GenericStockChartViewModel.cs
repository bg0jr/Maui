using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Indicators;
using Maui.Trading.Model;
using Maui.Trading.Utils;
using Maui.Trading.Data;
using Blade.Collections;

namespace Maui.Trading.Reporting.Charting
{
    public class GenericStockChartViewModel
    {
        private List<Curve> myCurves;
        private TimeRange myViewPort;

        // dont make public accessible at the moment because we do not
        // reprocess the curves when s.o. changes the settings
        private Settings mySettings;

        public class Settings
        {
            public Settings()
            {
                CurvesPreRenderingOperators = new List<IPriceSeriesOperator>();
            }

            public IList<IPriceSeriesOperator> CurvesPreRenderingOperators
            {
                get;
                private set;
            }
        }

        public GenericStockChartViewModel()
            : this( new Settings() )
        {
        }

        public GenericStockChartViewModel( Settings settings )
        {
            mySettings = settings;

            myCurves = new List<Curve>();
            Signals = SignalCurve.Empty;

            myViewPort = TimeRange.All;
        }

        public IEnumerable<Curve> Curves
        {
            get
            {
                return myCurves;
            }
        }

        public SignalCurve Signals
        {
            get;
            private set;
        }

        // expect: series already sorted by date
        public Curve AddCurve( string name, IPriceSeries points )
        {
            var curve = new Curve( name, points );
            curve.ViewPort = myViewPort;
            curve.PreRenderingOperators.AddRange( mySettings.CurvesPreRenderingOperators );

            myCurves.Add( curve );

            return curve;
        }

        public void SetSignals( ISignalSeries signals )
        {
            Signals = new SignalCurve( signals );
            Signals.ViewPort = ViewPort;
        }

        // default: all
        public TimeRange ViewPort
        {
            get
            {
                return myViewPort;
            }
            set
            {
                if ( myViewPort == value )
                {
                    return;
                }
                myViewPort = value;

                foreach ( var curve in myCurves )
                {
                    curve.ViewPort = myViewPort;
                }
                Signals.ViewPort = ViewPort;
            }
        }

        public double MinValue
        {
            get
            {
                double min = myCurves.Min( curve => curve.MinValue.Value );
                return min;
            }
        }

        public double MaxValue
        {
            get
            {
                double max = myCurves.Max( curve => curve.MaxValue.Value );
                return max;
            }
        }

        public DateTime MinTime
        {
            get
            {
                var min = myCurves.Min( curve => curve.MinTime );
                return min;
            }
        }

        public DateTime MaxTime
        {
            get
            {
                var max = myCurves.Max( curve => curve.MaxTime );
                return max;
            }
        }

        public void Add( StockPriceChart model )
        {
            var priceCurve = AddCurve( "Prices", model.Prices );

            foreach ( var indicatorPoints in model.IndicatorPoints )
            {
                AddCurve( indicatorPoints.Key, indicatorPoints.Value );
            }

            if ( model.Signals.Any() )
            {
                SetSignals( model.Signals );
            }
        }
    }
}
