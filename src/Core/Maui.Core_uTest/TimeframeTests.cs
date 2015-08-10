using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Blade;
using NUnit.Framework;


namespace Maui.UnitTests
{
    /// <summary>
    /// Tests the timeframe concept.
    /// </summary>
    [TestFixture]
    public class TimeframeTest : TestBase
    {
        private DateTime FromLocalTime( int totalSeconds )
        {
            return new DateTime( 1970, 1, 1 ).AddSeconds( totalSeconds );
        }

        [Test]
        public void MapDateToTime()
        {
            Assert.AreEqual( FromLocalTime( 976623127 ), Timeframes.PERIOD_TICK.ConvertToDateTime( "2000-12-12 12:12:07" ) );
            Assert.AreEqual( FromLocalTime( 976623120 ), Timeframes.PERIOD_1MIN.ConvertToDateTime( "2000-12-12 12:12:07" ) );
            Assert.AreEqual( FromLocalTime( 976623000 ), Timeframes.PERIOD_5MIN.ConvertToDateTime( "2000-12-12 12:12:07" ) );
            Assert.AreEqual( FromLocalTime( 976623000 ), Timeframes.PERIOD_10MIN.ConvertToDateTime( "2000-12-12 12:12:07" ) );
            Assert.AreEqual( FromLocalTime( 976622400 ), Timeframes.PERIOD_15MIN.ConvertToDateTime( "2000-12-12 12:12:07" ) );
            Assert.AreEqual( FromLocalTime( 976622400 ), Timeframes.PERIOD_30MIN.ConvertToDateTime( "2000-12-12 12:12:07" ) );
            Assert.AreEqual( FromLocalTime( 976622400 ), Timeframes.HOUR.ConvertToDateTime( "2000-12-12 12:12:07" ) );
            Assert.AreEqual( FromLocalTime( 976622400 ), Timeframes.PERIOD_2HOUR.ConvertToDateTime( "2000-12-12 12:12:07" ) );
            Assert.AreEqual( FromLocalTime( 976622400 ), Timeframes.PERIOD_3HOUR.ConvertToDateTime( "2000-12-12 12:12:07" ) );
            Assert.AreEqual( FromLocalTime( 976622400 ), Timeframes.PERIOD_4HOUR.ConvertToDateTime( "2000-12-12 12:12:07" ) );
            Assert.AreEqual( FromLocalTime( 976579200 ), Timeframes.DAY.ConvertToDateTime( "2000-12-12 12:12:07" ) );
            Assert.AreEqual( FromLocalTime( 976492800 ), Timeframes.WEEK.ConvertToDateTime( "2000-12-12 12:12:07" ) );
            Assert.AreEqual( FromLocalTime( 975628800 ), Timeframes.MONTH.ConvertToDateTime( "2000-12-12 12:12:07" ) );
            Assert.AreEqual( FromLocalTime( 946684800 ), Timeframes.YEAR.ConvertToDateTime( "2000-12-12 12:12:07" ) );
        }

        [Test]
        public void MapTimeToDate()
        {
            Assert.AreEqual( "2000-12-12 12:12:07", Timeframes.PERIOD_TICK.ConvertToString( FromLocalTime( 976623127 ) ) );
            Assert.AreEqual( "2000-12-12 12:12:00", Timeframes.PERIOD_1MIN.ConvertToString( FromLocalTime( 976623120 ) ) );
            Assert.AreEqual( "2000-12-12 12:10:00", Timeframes.PERIOD_5MIN.ConvertToString( FromLocalTime( 976623000 ) ) );
            Assert.AreEqual( "2000-12-12 12:10:00", Timeframes.PERIOD_10MIN.ConvertToString( FromLocalTime( 976623000 ) ) );
            Assert.AreEqual( "2000-12-12 12:00:00", Timeframes.PERIOD_15MIN.ConvertToString( FromLocalTime( 976622400 ) ) );
            Assert.AreEqual( "2000-12-12 12:00:00", Timeframes.PERIOD_30MIN.ConvertToString( FromLocalTime( 976622400 ) ) );
            Assert.AreEqual( "2000-12-12 12:00:00", Timeframes.HOUR.ConvertToString( FromLocalTime( 976622400 ) ) );
            Assert.AreEqual( "2000-12-12 12:00:00", Timeframes.PERIOD_2HOUR.ConvertToString( FromLocalTime( 976622400 ) ) );
            Assert.AreEqual( "2000-12-12 12:00:00", Timeframes.PERIOD_3HOUR.ConvertToString( FromLocalTime( 976622400 ) ) );
            Assert.AreEqual( "2000-12-12 12:00:00", Timeframes.PERIOD_4HOUR.ConvertToString( FromLocalTime( 976622400 ) ) );
            Assert.AreEqual( "2000-12-12", Timeframes.DAY.ConvertToString( FromLocalTime( 976579200 ) ) );
            Assert.AreEqual( "2000-50", Timeframes.WEEK.ConvertToString( FromLocalTime( 976492800 ) ) );
            Assert.AreEqual( "2000-12", Timeframes.MONTH.ConvertToString( FromLocalTime( 975628800 ) ) );
            Assert.AreEqual( "2000", Timeframes.YEAR.ConvertToString( FromLocalTime( 946684800 ) ) );
        }

    }
}
