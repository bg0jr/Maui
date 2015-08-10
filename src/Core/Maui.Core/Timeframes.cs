using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Blade;

namespace Maui
{
    /// <summary>
    /// Manage TimeFrames and provides date/time helper functions
    ///<remarks>
    /// This module exports all the variable describing the available "periods"
    /// commonly used for trading.
    /// The timeframes are represented by those variables which are only numbers.
    /// You can compare those numbers to know which timeframe is smaller or which
    /// one is bigger.
    /// 
    /// It also provides several functions to manipulate dates and periods. Those
    /// functions use modules GT::DateTime::* to do the actual work depending on
    /// the selected timeframe.
    /// </remarks>
    /// </summary>
    public class Timeframes
    {
        /// <summary/>
        public static readonly Timeframe PERIOD_TICK = new TimeframeTick();
        /// <summary/>
        public static readonly Timeframe PERIOD_1MIN = new Timeframe1Min();
        /// <summary/>
        public static readonly Timeframe PERIOD_5MIN = new Timeframe5Min();
        /// <summary/>
        public static readonly Timeframe PERIOD_10MIN = new Timeframe10Min();
        /// <summary/>
        public static readonly Timeframe PERIOD_15MIN = new Timeframe15Min();
        /// <summary/>
        public static readonly Timeframe PERIOD_30MIN = new Timeframe30Min();
        /// <summary/>
        public static readonly Timeframe HOUR = new TimeframeHour();
        /// <summary/>
        public static readonly Timeframe PERIOD_2HOUR = new Timeframe2Hour();
        /// <summary/>
        public static readonly Timeframe PERIOD_3HOUR = new Timeframe3Hour();
        /// <summary/>
        public static readonly Timeframe PERIOD_4HOUR = new Timeframe4Hour();
        /// <summary/>
        public static readonly Timeframe DAY = new TimeframeDay();
        /// <summary/>
        public static readonly Timeframe WEEK = new TimeframeWeek();
        /// <summary/>
        public static readonly Timeframe MONTH = new TimeframeMonth();
        /// <summary/>
        public static readonly Timeframe YEAR = new TimeframeYear();

        /// <summary>
        /// This function does convert the given date from the $orig_timeframe in a
        /// date of the $dest_timeframe. Take care that the destination timeframe must be
        /// bigger than the original timeframe.
        /// </summary>
        public static string ConvertDate( string date, Timeframe orig, Timeframe dest )
        {
            // #WAR#  WARN  "the destination time frame must be bigger" if ( $orig <= $dest);
            return dest.ConvertToString( orig.ConvertToDateTime( date ) );
        }

        /// <summary>
        /// This function does convert the given date from the $orig_timeframe in a
        /// date of the $dest_timeframe. Take care that the destination timeframe must be
        /// bigger than the original timeframe.
        /// </summary>
        public static DateTime ConvertDate( DateTime date, Timeframe orig, Timeframe dest )
        {
            // #WAR#  WARN  "the destination time frame must be bigger" if ( $orig <= $dest);
            return dest.ConvertToDateTime( orig.ConvertToString( date ) );
        }

        /// <summary>
        /// Returns the list of timeframes that are managed by the DateTime framework.
        /// </summary>
        public static IEnumerable<Timeframe> AvailableTimeframes
        {
            get
            {
                return typeof( Timeframes ).GetFields( BindingFlags.Static | BindingFlags.Public )
                    .Select( fi => (Timeframe)fi.GetValue( null ) );
            }
        }

        /// <summary>
        /// Returns the timeframe associated to the given name.
        /// </summary>
        public static Timeframe GetTimeframeByName( string name )
        {
            return AvailableTimeframes.First( tf => tf.Name == name );
        }

        /// <summary>
        /// Returns how many times the second timeframe fits in the first one.
        /// </summary>
        public static double GetTimeframeRatio( Timeframe first, Timeframe second )
        {
            if ( first == second ) return 1;

            if ( first < second )
            {
                return ( 1 / GetTimeframeRatio( second, first ) );
            }

            return first.GetTimeframeRatio( second );
        }
    }

    /// <summary/>
    public abstract class Timeframe
    {
        /// <summary/>
        protected Timeframe( int id, string name )
        {
            Id = id;
            Name = name;
        }

        /// <summary/>
        protected int Id { get; private set; }
        /// <summary/>
        public string Name { get; private set; }

        /// <summary>
        /// Returns a time (ie a number of seconds since 1970) representing that 
        /// date in the history. It is usually corresponding to the first second 
        /// of the given period.
        /// </summary>
        public abstract DateTime ConvertToDateTime( string date );

        /// <summary>
        /// Is the complementary function to map_date_to_time. It will return a
        /// date describing the period that includes the given time.
        /// </summary>
        public abstract string ConvertToString( DateTime time );

        /// <summary>
        /// Returns how many times the second timeframe fits in the first one.
        /// </summary>
        public abstract double GetTimeframeRatio( Timeframe other );

        /// <summary/>
        public override bool Equals( object obj )
        {
            if ( obj is Timeframe )
            {
                return ( (Timeframe)obj ).Id == Id;
            }

            return false;
        }

        /// <summary/>
        public bool Equals( Timeframe tf )
        {
            return tf != null && tf.Id == Id;
        }

        /// <summary/>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <summary/>
        public static bool operator ==( Timeframe lhs, Timeframe rhs )
        {
            if ( object.ReferenceEquals( lhs, rhs ) )
            {
                return true;
            }

            // If one is null, but not both, return false.
            if ( (object)lhs == null || (object)rhs == null )
            {
                return false;
            }

            return lhs != null && rhs != null && lhs.Id == rhs.Id;
        }

        /// <summary/>
        public static bool operator !=( Timeframe lhs, Timeframe rhs )
        {
            return !( lhs == rhs );
        }

        /// <summary/>
        public static bool operator <( Timeframe lhs, Timeframe rhs )
        {
            return lhs != null && rhs != null && lhs.Id < rhs.Id;
        }

        /// <summary/>
        public static bool operator <=( Timeframe lhs, Timeframe rhs )
        {
            return lhs != null && rhs != null && lhs.Id <= rhs.Id;
        }

        /// <summary/>
        public static bool operator >( Timeframe lhs, Timeframe rhs )
        {
            return !( lhs.Id <= rhs.Id );
        }

        /// <summary/>
        public static bool operator >=( Timeframe lhs, Timeframe rhs )
        {
            return !( lhs.Id < rhs.Id );
        }

        /// <summary/>
        public override string ToString()
        {
            return Name;
        }

        /// <summary/>
        protected DateTime Parse( string value )
        {
            var tokens = value.Split( ' ' );
            var date = tokens[ 0 ];
            var time = "00:00:00";

            if ( tokens.Length == 2 && !string.IsNullOrEmpty( tokens[ 1 ] ) ) time = tokens[ 1 ];

            tokens = date.Split( '-' );
            var y = int.Parse( tokens[ 0 ] );
            var m = ( tokens.Length >= 2 ? int.Parse( tokens[ 1 ] ) : 1 );
            var d = ( tokens.Length >= 3 ? int.Parse( tokens[ 2 ] ) : 1 );

            tokens = time.Split( ':' );
            var h = ( tokens.Length >= 1 ? int.Parse( tokens[ 0 ] ) : 0 );
            var n = ( tokens.Length >= 2 ? int.Parse( tokens[ 1 ] ) : 0 );
            var s = ( tokens.Length >= 3 ? int.Parse( tokens[ 2 ] ) : 0 );

            return new DateTime( y, m, d, h, n, s );
        }
    }

    /// <summary>
    /// This module treat dates describing ticks. They have the following format :
    /// YYYY-MM-DD HH:NN:SS
    /// </summary>
    public class TimeframeTick : Timeframe
    {
        /// <summary/>
        public TimeframeTick()
            : base( 1, "tick" )
        {
        }

        /// <summary/>
        public override DateTime ConvertToDateTime( string value )
        {
            return Parse( value );
        }

        /// <summary/>
        public override string ConvertToString( DateTime time )
        {
            return string.Format( "{0:0000}-{1:00}-{2:00} {3:00}:{4:00}:{5:00}",
                time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second );
        }

        /// <summary/>
        public override double GetTimeframeRatio( Timeframe tf )
        {
            throw new Exception( "Cannot set timeframe ratio for tick data" );
        }
    }

    /// <summary>
    /// This module treat dates describing a 1 minute period. They have the following format :
    /// YYYY-MM-DD HH:NN:00
    /// </summary>
    public class Timeframe1Min : Timeframe
    {
        /// <summary/>
        public Timeframe1Min()
            : base( 10, "1min" )
        {
        }

        /// <summary/>
        public override DateTime ConvertToDateTime( string value )
        {
            var t = Parse( value );

            return new DateTime( t.Year, t.Month, t.Day, t.Hour, t.Minute, 0 );
        }

        /// <summary/>
        public override string ConvertToString( DateTime time )
        {
            return string.Format( "{0:0000}-{1:00}-{2:00} {3:00}:{4:00}:00",
                time.Year, time.Month, time.Day, time.Hour, time.Minute );
        }

        /// <summary/>
        public override double GetTimeframeRatio( Timeframe tf )
        {
            if ( tf is Timeframe1Min ) return 60 / 60; // 8 hours approximatively
            if ( tf is Timeframe5Min ) return 12 / 60;
            if ( tf is Timeframe10Min ) return 6 / 60;
            if ( tf is Timeframe30Min ) return 2 / 60;
            if ( tf is TimeframeHour ) return 1 / 60;

            throw new ArgumentOutOfRangeException( "tf wrong: " + tf );
        }
    }

    /// <summary>
    /// This module treat dates describing a 5 minute period. They have the following format :
    /// YYYY-MM-DD HH:NN:00
    /// </summary>
    public class Timeframe5Min : Timeframe
    {
        /// <summary/>
        public Timeframe5Min()
            : base( 20, "5min" )
        {
        }

        /// <summary/>
        public override DateTime ConvertToDateTime( string value )
        {
            var t = Parse( value );
            var n = t.Minute;

            if ( n >= 55 ) { n = 55; }
            else if ( n >= 50 ) { n = 50; }
            else if ( n >= 45 ) { n = 45; }
            else if ( n >= 40 ) { n = 40; }
            else if ( n >= 35 ) { n = 35; }
            else if ( n >= 30 ) { n = 30; }
            else if ( n >= 25 ) { n = 25; }
            else if ( n >= 20 ) { n = 20; }
            else if ( n >= 15 ) { n = 15; }
            else if ( n >= 10 ) { n = 10; }
            else if ( n >= 5 ) { n = 5; }
            else { n = 0; }

            return new DateTime( t.Year, t.Month, t.Day, t.Hour, n, 0 );
        }

        /// <summary/>
        public override string ConvertToString( DateTime time )
        {
            var n = time.Minute;

            if ( n >= 55 ) { n = 55; }
            else if ( n >= 50 ) { n = 50; }
            else if ( n >= 45 ) { n = 45; }
            else if ( n >= 40 ) { n = 40; }
            else if ( n >= 35 ) { n = 35; }
            else if ( n >= 30 ) { n = 30; }
            else if ( n >= 25 ) { n = 25; }
            else if ( n >= 20 ) { n = 20; }
            else if ( n >= 15 ) { n = 15; }
            else if ( n >= 10 ) { n = 10; }
            else if ( n >= 5 ) { n = 5; }
            else { n = 0; ;}

            return string.Format( "{0:0000}-{1:00}-{2:00} {3:00}:{4:00}:00",
                time.Year, time.Month, time.Day, time.Hour, n );
        }

        /// <summary/>
        public override double GetTimeframeRatio( Timeframe tf )
        {
            if ( tf is Timeframe1Min ) return 60 / 12; // 8 hours approximatively
            if ( tf is Timeframe5Min ) return 12 / 12;
            if ( tf is Timeframe10Min ) return 6 / 12;
            if ( tf is Timeframe30Min ) return 2 / 12;
            if ( tf is TimeframeHour ) return 1 / 12;

            throw new ArgumentOutOfRangeException( "tf wrong: " + tf );
        }
    }

    /// <summary>
    /// This module treat dates describing a 10 minute period. They have the following format :
    /// YYYY-MM-DD HH:N0:00
    /// </summary>
    public class Timeframe10Min : Timeframe
    {
        /// <summary/>
        public Timeframe10Min()
            : base( 40, "10min" )
        {
        }

        /// <summary/>
        public override DateTime ConvertToDateTime( string value )
        {
            var t = Parse( value );
            var n = t.Minute;
            if ( n >= 50 ) { n = 50; }
            else if ( n >= 40 ) { n = 40; }
            else if ( n >= 30 ) { n = 30; }
            else if ( n >= 20 ) { n = 20; }
            else if ( n >= 10 ) { n = 10; }
            else { n = 0; }

            return new DateTime( t.Year, t.Month, t.Day, t.Hour, n, 0 );
        }

        /// <summary/>
        public override string ConvertToString( DateTime time )
        {
            var n = time.Minute;
            if ( n >= 50 ) { n = 50; }
            else if ( n >= 40 ) { n = 40; }
            else if ( n >= 30 ) { n = 30; }
            else if ( n >= 20 ) { n = 20; }
            else if ( n >= 10 ) { n = 10; }
            else { n = 0; }

            return string.Format( "{0:0000}-{1:00}-{2:00} {3:00}:{4:00}:00",
                time.Year, time.Month, time.Day, time.Hour, time.Minute );
        }

        /// <summary/>
        public override double GetTimeframeRatio( Timeframe tf )
        {
            if ( tf is Timeframe1Min ) return 60 / 6; // 8 hours approximatively
            if ( tf is Timeframe5Min ) return 12 / 6;
            if ( tf is Timeframe10Min ) return 6 / 6;
            if ( tf is Timeframe30Min ) return 2 / 6;
            if ( tf is TimeframeHour ) return 1 / 6;

            throw new ArgumentOutOfRangeException( "tf wrong: " + tf );
        }
    }

    /// <summary>
    /// This module treat dates describing a quarter-hour. They have the following format :
    /// YYYY-MM-DD HH:NN:00
    /// </summary>
    public class Timeframe15Min : Timeframe
    {
        /// <summary/>
        public Timeframe15Min()
            : base( 45, "15min" )
        {
        }

        /// <summary/>
        public override DateTime ConvertToDateTime( string value )
        {
            var t = Parse( value );
            var n = t.Minute;
            if ( n >= 45 ) { n = 45; }
            else if ( n >= 30 ) { n = 30; }
            else if ( n >= 15 ) { n = 15; }
            else { n = 0; }

            return new DateTime( t.Year, t.Month, t.Day, t.Hour, n, 0 );
        }

        /// <summary/>
        public override string ConvertToString( DateTime time )
        {
            var n = time.Minute;
            if ( n >= 45 ) { n = 45; }
            else if ( n >= 30 ) { n = 30; }
            else if ( n >= 15 ) { n = 15; }
            else { n = 0; }

            return string.Format( "{0:0000}-{1:00}-{2:00} {3:00}:{4:00}:00",
                time.Year, time.Month, time.Day, time.Hour, n );
        }

        /// <summary/>
        public override double GetTimeframeRatio( Timeframe tf )
        {
            if ( tf is Timeframe1Min ) return 60 / 4; // 8 hours approximatively
            if ( tf is Timeframe5Min ) return 12 / 4;
            if ( tf is Timeframe10Min ) return 6 / 4;
            if ( tf is Timeframe30Min ) return 2 / 4;
            if ( tf is TimeframeHour ) return 1 / 4;

            throw new ArgumentOutOfRangeException( "tf wrong: " + tf );
        }
    }

    /// <summary>
    /// This module treat dates describing an half-hour. They have the following format :
    /// YYYY-MM-DD HH:N0:00
    /// </summary>
    public class Timeframe30Min : Timeframe
    {
        /// <summary/>
        public Timeframe30Min()
            : base( 50, "30min" )
        {
        }

        /// <summary/>
        public override DateTime ConvertToDateTime( string value )
        {
            var t = Parse( value );
            var n = t.Minute;
            if ( n >= 30 ) { n = 30; } else { n = 0; }

            return new DateTime( t.Year, t.Month, t.Day, t.Hour, n, 0 );
        }

        /// <summary/>
        public override string ConvertToString( DateTime time )
        {
            var n = time.Minute;
            if ( n >= 30 ) { n = 30; } else { n = 0; }

            return string.Format( "{0:0000}-{1:00}-{2:00} {3:00}:{4:00}:00",
                time.Year, time.Month, time.Day, time.Hour, n );
        }

        /// <summary/>
        public override double GetTimeframeRatio( Timeframe tf )
        {
            if ( tf is Timeframe1Min ) return 60 / 2; // 8 hours approximatively
            if ( tf is Timeframe5Min ) return 12 / 2;
            if ( tf is Timeframe10Min ) return 6 / 2;
            if ( tf is Timeframe30Min ) return 2 / 2;
            if ( tf is TimeframeHour ) return 1 / 2;

            throw new ArgumentOutOfRangeException( "tf wrong: " + tf );
        }
    }

    /// <summary>
    /// This module treat dates describing an Hour. They have the following format :
    /// YYYY-MM-DD HH:00:00
    /// </summary>
    public class TimeframeHour : Timeframe
    {
        /// <summary/>
        public TimeframeHour()
            : base( 60, "hour" )
        {
        }

        /// <summary/>
        public override DateTime ConvertToDateTime( string value )
        {
            var t = Parse( value );

            return new DateTime( t.Year, t.Month, t.Day, t.Hour, 0, 0 );
        }

        /// <summary/>
        public override string ConvertToString( DateTime time )
        {
            return string.Format( "{0:0000}-{1:00}-{2:00} {3:00}:00:00",
                time.Year, time.Month, time.Day, time.Hour );
        }

        /// <summary/>
        public override double GetTimeframeRatio( Timeframe tf )
        {
            if ( tf is Timeframe1Min ) return 60; // 8 hours approximatively
            if ( tf is Timeframe5Min ) return 12;
            if ( tf is Timeframe10Min ) return 6;
            if ( tf is Timeframe30Min ) return 2;
            if ( tf is TimeframeHour ) return 1;

            throw new ArgumentOutOfRangeException( "tf wrong: " + tf );
        }
    }

    /// <summary>
    /// This module treat dates describing the 2Hour timeframe. They have the following format :
    /// YYYY-MM-DD HH:00:00
    /// </summary>
    public class Timeframe2Hour : Timeframe
    {
        /// <summary/>
        public Timeframe2Hour()
            : base( 62, "2hour" )
        {
        }

        /// <summary/>
        public override DateTime ConvertToDateTime( string value )
        {
            var t = Parse( value );
            var h = t.Hour;
            if ( h >= 22 ) { h = 22; }
            else if ( h >= 20 ) { h = 20; }
            else if ( h >= 18 ) { h = 18; }
            else if ( h >= 16 ) { h = 16; }
            else if ( h >= 14 ) { h = 14; }
            else if ( h >= 12 ) { h = 12; }
            else if ( h >= 10 ) { h = 10; }
            else if ( h >= 8 ) { h = 8; }
            else if ( h >= 6 ) { h = 6; }
            else if ( h >= 4 ) { h = 4; }
            else if ( h >= 2 ) { h = 2; }
            else { h = 0; }

            return new DateTime( t.Year, t.Month, t.Day, h, 0, 0 );
        }

        /// <summary/>
        public override string ConvertToString( DateTime time )
        {
            var h = time.Hour;

            if ( h >= 22 ) { h = 22; }
            else if ( h >= 20 ) { h = 20; }
            else if ( h >= 18 ) { h = 18; }
            else if ( h >= 16 ) { h = 16; }
            else if ( h >= 14 ) { h = 14; }
            else if ( h >= 12 ) { h = 12; }
            else if ( h >= 10 ) { h = 10; }
            else if ( h >= 8 ) { h = 8; }
            else if ( h >= 6 ) { h = 6; }
            else if ( h >= 4 ) { h = 4; }
            else if ( h >= 2 ) { h = 2; }
            else { h = 0; }

            return string.Format( "{0:0000}-{1:00}-{2:00} {3:00}:00:00",
                time.Year, time.Month, time.Day, h );
        }

        /// <summary/>
        public override double GetTimeframeRatio( Timeframe tf )
        {
            if ( tf is Timeframe1Min ) return 60 * 2; // 8 hours approximatively
            if ( tf is Timeframe5Min ) return 12 * 2;
            if ( tf is Timeframe10Min ) return 6 * 2;
            if ( tf is Timeframe30Min ) return 2 * 2;
            if ( tf is TimeframeHour ) return 1 * 2;

            throw new ArgumentOutOfRangeException( "tf wrong: " + tf );
        }
    }

    /// <summary>
    /// This module treat dates describing the 3Hour timeframe. They have the following format :
    /// YYYY-MM-DD HH:00:00
    /// </summary>
    public class Timeframe3Hour : Timeframe
    {
        /// <summary/>
        public Timeframe3Hour()
            : base( 64, "3hour" )
        {
        }

        /// <summary/>
        public override DateTime ConvertToDateTime( string value )
        {
            var t = Parse( value );
            var h = t.Hour;
            if ( h >= 21 ) { h = 21; }
            else if ( h >= 18 ) { h = 18; }
            else if ( h >= 15 ) { h = 15; }
            else if ( h >= 12 ) { h = 12; }
            else if ( h >= 9 ) { h = 9; }
            else if ( h >= 6 ) { h = 6; }
            else if ( h >= 3 ) { h = 3; }
            else { h = 0; }

            return new DateTime( t.Year, t.Month, t.Day, h, 0, 0 );
        }

        /// <summary/>
        public override string ConvertToString( DateTime time )
        {
            var h = time.Hour;
            if ( h >= 21 ) { h = 21; }
            else if ( h >= 18 ) { h = 18; }
            else if ( h >= 15 ) { h = 15; }
            else if ( h >= 12 ) { h = 12; }
            else if ( h >= 9 ) { h = 9; }
            else if ( h >= 6 ) { h = 6; }
            else if ( h >= 3 ) { h = 3; }
            else { h = 0; }

            return string.Format( "{0:0000}-{1:00}-{2:00} {3:00}:00:00",
                time.Year, time.Month, time.Day, h );
        }

        /// <summary/>
        public override double GetTimeframeRatio( Timeframe tf )
        {
            if ( tf is Timeframe1Min ) return 60 * 3; // 8 hours approximatively
            if ( tf is Timeframe5Min ) return 12 * 3;
            if ( tf is Timeframe10Min ) return 6 * 3;
            if ( tf is Timeframe30Min ) return 2 * 3;
            if ( tf is TimeframeHour ) return 1 * 3;

            throw new ArgumentOutOfRangeException( "tf wrong: " + tf );
        }
    }

    /// <summary>
    /// This module treat dates describing the 2Hour timeframe. They have the following format :
    /// YYYY-MM-DD HH:00:00
    /// </summary>
    public class Timeframe4Hour : Timeframe
    {
        /// <summary/>
        public Timeframe4Hour()
            : base( 66, "3hour" )
        {
        }

        /// <summary/>
        public override DateTime ConvertToDateTime( string value )
        {
            var t = Parse( value );
            var h = t.Hour;
            if ( h >= 20 ) { h = 20; }
            else if ( h >= 16 ) { h = 16; }
            else if ( h >= 12 ) { h = 12; }
            else if ( h >= 8 ) { h = 8; }
            else if ( h >= 4 ) { h = 4; }
            else { h = 0; }

            return new DateTime( t.Year, t.Month, t.Day, h, 0, 0 );
        }

        /// <summary/>
        public override string ConvertToString( DateTime time )
        {
            var h = time.Hour;
            if ( h >= 20 ) { h = 20; }
            else if ( h >= 16 ) { h = 16; }
            else if ( h >= 12 ) { h = 12; }
            else if ( h >= 8 ) { h = 8; }
            else if ( h >= 4 ) { h = 4; }
            else { h = 0; }

            return string.Format( "{0:0000}-{1:00}-{2:00} {3:00}:00:00",
                time.Year, time.Month, time.Day, h );
        }

        /// <summary/>
        public override double GetTimeframeRatio( Timeframe tf )
        {
            if ( tf is Timeframe1Min ) return 60 * 4; // 8 hours approximatively
            if ( tf is Timeframe5Min ) return 12 * 4;
            if ( tf is Timeframe10Min ) return 6 * 4;
            if ( tf is Timeframe30Min ) return 2 * 4;
            if ( tf is TimeframeHour ) return 1 * 4;

            throw new ArgumentOutOfRangeException( "tf wrong: " + tf );
        }
    }

    /// <summary>
    /// This module treat dates describing a day. They have the following format :
    /// YYYY-MM-DD
    /// </summary>
    public class TimeframeDay : Timeframe
    {
        /// <summary/>
        public TimeframeDay()
            : base( 70, "day" )
        {
        }

        /// <summary/>
        public override DateTime ConvertToDateTime( string value )
        {
            var t = Parse( value );

            return new DateTime( t.Year, t.Month, t.Day, 0, 0, 0 );
        }

        /// <summary/>
        public override string ConvertToString( DateTime time )
        {
            return string.Format( "{0:0000}-{1:00}-{2:00}", time.Year, time.Month, time.Day );
        }

        /// <summary/>
        public override double GetTimeframeRatio( Timeframe tf )
        {
            if ( tf is Timeframe1Min ) return 8 * 60; // 8 hours approximatively
            if ( tf is Timeframe5Min ) return 8 * 12;
            if ( tf is Timeframe10Min ) return 8 * 6;
            if ( tf is Timeframe30Min ) return 8 * 2;
            if ( tf is TimeframeHour ) return 8;

            throw new ArgumentOutOfRangeException( "tf wrong: " + tf );
        }
    }

    /// <summary>
    /// This module treat dates describing a week. They have the following format :
    /// YYYY-WW
    ///
    /// WW being the week number.
    /// </summary>
    public class TimeframeWeek : Timeframe
    {
        /// <summary/>
        public TimeframeWeek()
            : base( 80, "week" )
        {
        }

        /// <summary/>
        public override DateTime ConvertToDateTime( string value )
        {
            var t = Parse( value );

            t = t.MondayOfWeek();
            return new DateTime( t.Year, t.Month, t.Day, 0, 0, 0 );
        }

        /// <summary/>
        public override string ConvertToString( DateTime time )
        {
            var week = time.WeekOfYear();
            return string.Format( "{0:0000}-{1:00}", time.Year, week );
        }

        /// <summary/>
        public override double GetTimeframeRatio( Timeframe tf )
        {
            if ( tf == Timeframes.DAY ) return 5;
            return Timeframes.DAY.GetTimeframeRatio( tf ) * 5;
        }
    }

    /// <summary>
    /// This module treat dates describing a month. They have the following format :
    /// YYYY-MM
    /// </summary>
    public class TimeframeMonth : Timeframe
    {
        /// <summary/>
        public TimeframeMonth()
            : base( 90, "month" )
        {
        }

        /// <summary/>
        public override DateTime ConvertToDateTime( string value )
        {
            var t = Parse( value );

            return new DateTime( t.Year, t.Month, 1, 0, 0, 0 );
        }

        /// <summary/>
        public override string ConvertToString( DateTime time )
        {
            //    return sprintf("%04d/%02d", $y + 1900, $m + 1);
            return string.Format( "{0:0000}-{1:00}", time.Year, time.Month );
        }

        /// <summary/>
        public override double GetTimeframeRatio( Timeframe tf )
        {
            if ( tf == Timeframes.DAY ) return 30 * 5 / 7;
            if ( tf == Timeframes.WEEK ) return 30 / 7;

            return Timeframes.DAY.GetTimeframeRatio( tf ) * 30 * 5 / 7;
        }
    }

    /// <summary>
    /// This module treat dates describing a year. They have the following format :
    /// YYYY
    /// </summary>
    public class TimeframeYear : Timeframe
    {
        /// <summary/>
        public TimeframeYear()
            : base( 100, "year" )
        {
        }

        /// <summary/>
        public override DateTime ConvertToDateTime( string value )
        {
            var t = Parse( value );

            return new DateTime( t.Year, 1, 1, 0, 0, 0 );
        }

        /// <summary/>
        public override string ConvertToString( DateTime time )
        {
            return string.Format( "{0:0000}", time.Year );
        }

        /// <summary/>
        public override double GetTimeframeRatio( Timeframe tf )
        {
            if ( tf is TimeframeMonth ) return 12;
            if ( tf is TimeframeWeek ) return 52;
            if ( tf is TimeframeDay ) return 250;

            return Timeframes.DAY.GetTimeframeRatio( tf ) * 250;
        }
    }
}
