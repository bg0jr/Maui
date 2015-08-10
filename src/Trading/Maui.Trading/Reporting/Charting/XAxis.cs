using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;

namespace Maui.Trading.Reporting.Charting
{
    public class XAxis : Axis
    {
        private const int MaxLabels = 12;

        public XAxis( DateTime min, DateTime max )
        {
            MinTime = min;
            MaxTime = max;

            SetDefaults();
        }

        public DateTime MinTime
        {
            get;
            private set;
        }

        public DateTime MaxTime
        {
            get;
            private set;
        }

        private void SetDefaults()
        {
            MinorGrid.Enabled = false;

            Minimum = MinTime.ToOADate();
            Maximum = MaxTime.ToOADate();

            SetupIntervalAndLabeling();
        }

        private void SetupIntervalAndLabeling()
        {
            Interval = 1;
            IntervalType = DateTimeIntervalType.Days;

            var timeRange = MaxTime - MinTime;

            if ( timeRange.TotalDays > 365 * 4 )
            {
                // more than 4 years 
                // -> only display the years
                Interval = timeRange.TotalDays / 365 / MaxLabels;
                IntervalType = DateTimeIntervalType.Years;
                LabelStyle.Format = "yyyy";
            }
            else if ( timeRange.TotalDays > 365 )
            {
                // more than one year but less than 4 years
                // -> display year and quartals
                Interval = timeRange.TotalDays / 30 / MaxLabels;
                IntervalType = DateTimeIntervalType.Months;
                LabelStyle.Format = "MMM.yyyy";
            }
            else
            {
                // less than one year
                // -> display day and month
                Interval = timeRange.TotalDays / MaxLabels;
                LabelStyle.Format = "dd.MMM";
            }

            if ( Interval < 1 )
            {
                Interval = 1;
            }
        }
    }
}
