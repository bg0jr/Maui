using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;
using System.Windows.Forms.DataVisualization.Charting;
using Maui.Trading.Data;

namespace Maui.Trading.Reporting.Charting
{
    public class Curve : ISeriesViewPort<DateTime, double, IPriceSeries>
    {
        private PriceSeriesViewport myViewport;

        public Curve( string name, IPriceSeries points )
        {
            Name = name;

            myViewport = new PriceSeriesViewport( points, TimeRange.All );
            PreRenderingOperators = new List<IPriceSeriesOperator>();
        }

        public string Name
        {
            get;
            private set;
        }

        public DateTime MinTime
        {
            get { return myViewport.MinTime; }
        }

        public DateTime MaxTime
        {
            get { return myViewport.MaxTime; }
        }

        public TimedValue<DateTime, double> MinValue
        {
            get { return myViewport.MinValue; }
        }

        public TimedValue<DateTime, double> MaxValue
        {
            get { return myViewport.MaxValue; }
        }

        public IPriceSeries Series
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

        public IList<IPriceSeriesOperator> PreRenderingOperators
        {
            get;
            private set;
        }

        public Series CreateSeries()
        {
            var series = new Series( Name );

            series.ChartType = SeriesChartType.Line;
            series.XValueType = ChartValueType.DateTime;
            series.YValueType = ChartValueType.Double;

            var processedPoints = PreProcessPoints();

            foreach ( var price in processedPoints )
            {
                series.Points.AddXY( price.Time, price.Value );
            }

            return series;
        }

        private IPriceSeries PreProcessPoints()
        {
            var series = Series;

            foreach ( var op in PreRenderingOperators )
            {
                series = op.Apply( series );
            }

            return series;
        }
    }
}
