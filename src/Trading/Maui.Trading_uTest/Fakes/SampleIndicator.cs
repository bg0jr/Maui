using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Binding;

namespace Maui.Trading.UnitTests.Fakes
{
    public class SampleIndicator
    {
        [DataSource]
        public ISingleDataSource<double> SampleDouble
        {
            get;
            set;
        }

        [DataSource]
        public DoubleSetDataSource SetOfDoubles
        {
            get;
            set;
        }

        [DataSource( Datum = "D1" )]
        public ISingleDataSource<double> AnotherSampleDouble
        {
            get;
            set;
        }

        [DataSource( Datum = "D2" )]
        public DoubleSetDataSource AnotherSetOfDoubles
        {
            get;
            set;
        }
    }
}
