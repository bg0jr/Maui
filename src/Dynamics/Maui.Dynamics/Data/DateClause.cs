using System;
using Blade;

namespace Maui.Dynamics.Data
{

    /// <summary>
    /// Represents a date query clause.
    /// A range will be normalized ("to" less or equal "from").
    /// The time part of a date will always be cut off.
    /// </summary>
    public class DateClause
    {
        public static DateClause All = new DateClause();

        /// <summary>
        /// Creates a DateClause which has no limitations.
        /// This means all available data will be returned from DB.
        /// </summary>
        public DateClause()
            : this( DateTime.MinValue, DateTime.MaxValue )
        {
        }

        public DateClause( DateTime date )
            : this( date, date )
        {
        }

        public DateClause( DateTime from, DateTime to )
        {
            DateTimeExtensions.NormalizeRange( ref from, ref to );

            From = from.Date;
            To = to.Date;
        }

        public DateTime From { get; private set; }
        public DateTime To { get; private set; }

        public bool IsRange
        {
            get
            {
                return From != To;
            }
        }

        public bool IsInRange( DateTime date )
        {
            return date >= From && date <= To;
        }

        public bool IsInRange( int year )
        {
            return year >= From.Year && year <= To.Year;
        }
    }
}
