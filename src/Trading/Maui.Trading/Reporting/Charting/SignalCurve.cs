using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;
using Maui.Trading.Indicators;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;

namespace Maui.Trading.Reporting.Charting
{
    public class SignalCurve : ISeriesViewPort<DateTime, Signal, ISignalSeries>
    {
        private ISignalSeries mySignals;
        private SignalSeriesViewport myViewport;

        // index based on "Time" for performance reasons
        private Dictionary<DateTime, TimedValue<DateTime, double>> myReferenceTimeIndex;

        public static readonly SignalCurve Empty = new SignalCurve();

        public SignalCurve( ISignalSeries signals )
        {
            mySignals = signals;
            myViewport = new SignalSeriesViewport( signals, TimeRange.All );

            BuildUpIndex();
        }

        private void BuildUpIndex()
        {
            myReferenceTimeIndex = new Dictionary<DateTime, TimedValue<DateTime, double>>();
            foreach ( var item in mySignals.Reference )
            {
                myReferenceTimeIndex.Add( item.Time, item );
            }
        }

        /// <summary>
        /// Creates a view to an empty set (to avoid null refs)
        /// </summary>
        private SignalCurve()
        {
            myViewport = SignalSeriesViewport.Null;
        }

        public DateTime MinTime
        {
            get { return myViewport.MinTime; }
        }

        public DateTime MaxTime
        {
            get { return myViewport.MaxTime; }
        }

        public TimedValue<DateTime, Signal> MinValue
        {
            get { return myViewport.MinValue; }
        }

        public TimedValue<DateTime, Signal> MaxValue
        {
            get { return myViewport.MaxValue; }
        }

        public ISignalSeries Series
        {
            get { return myViewport.Series; }
        }

        // default: all
        public ClosedInterval<DateTime> ViewPort
        {
            get
            {
                return myViewport.ViewPort;
            }
            set
            {
                myViewport.ViewPort = value;
            }
        }

        public TimedValue<DateTime, double> GetPointForSignal( TimedValue<DateTime, Signal> signal )
        {
            if ( mySignals == null )
            {
                return null;
            }

            TimedValue<DateTime, double> value = null;
            myReferenceTimeIndex.TryGetValue( signal.Time, out value );
            return value;
        }

        public Series CreateSeries()
        {
            var series = new Series( "Signals" );

            series.ChartType = SeriesChartType.Point;
            series.XValueType = ChartValueType.DateTime;
            series.YValueType = ChartValueType.Double;
            series.MarkerSize = 7;

            // do not display (None-signals)
            var signals = GetSignalsToDisplay();

            foreach ( var signal in signals )
            {
                AddSignal( series, signal );
            }

            return series;
        }

        private IEnumerable<TimedValue<DateTime, Signal>> GetSignalsToDisplay()
        {
            return Series
                .Where( timedSignal => IsSignalToDisplay( timedSignal ) );
        }

        // it is completely ok to get None-Signals here.
        // we could also get neutral signals here if it is a combined chart and a combined signal
        private static bool IsSignalToDisplay( TimedValue<DateTime, Signal> timedSignal )
        {
            return timedSignal.Value.Type == SignalType.Buy ||
                timedSignal.Value.Type == SignalType.Sell;
        }

        private void AddSignal( Series series, TimedValue<DateTime, Signal> signal )
        {
            var price = GetPointForSignal( signal );
            if ( price == null )
            {
                // actually this should result at least in a warning
                price = new SimplePrice( signal.Time, 0.0 );
            }

            series.Points.AddXY( signal.Time, price.Value );

            var point = series.Points[ series.Points.Count - 1 ];

            if ( signal.Value.Type == SignalType.Buy )
            {
                point.MarkerStyle = MarkerStyle.Triangle;
                point.MarkerColor = Color.Green;
            }
            else if ( signal.Value.Type == SignalType.Sell )
            {
                point.MarkerStyle = MarkerStyle.Cross;
                point.MarkerColor = Color.Red;
            }
        }
    }
}
