using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;

namespace Maui.Trading.Reporting.Charting
{
    public class PriceAxis : Axis
    {
        public PriceAxis( double min, double max )
        {
            Minimum = min;
            Maximum = max;

            SetDefaults();
        }

        private void SetDefaults()
        {
            Enabled = AxisEnabled.True;

            IsLogarithmic = true;
            LogarithmBase = 2;
            Interval = CalculateInterval();

            LabelStyle.Format = ( Maximum + Minimum ) / 2 < 20 ? "0.00" : "0";
           
            MajorTickMark.Enabled = false;
        }

        private double CalculateInterval()
        {
            const int NumLabels = 10;

            double logMin = Math.Log( Minimum, 2 );
            double logMax = Math.Log( Maximum, 2 );

            return ( logMax - logMin ) / NumLabels;
        }
    }
}
