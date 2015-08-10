using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Indicators;
using Maui.Trading.Binding;
using Maui.Trading.Model;
using Maui.Trading.Reporting;

namespace Maui.Trading.Modules.Indicators.ChannelBreakOut
{
    public class HighLowChannel : AbstractIndicator
    {
        [DataSource]
        public IEnumerableDataSource<SimplePrice> Prices
        {
            get;
            set;
        }

        [DataSource]
        public IEnumerableDataSource<FundamentalValue> Eps
        {
            get;
            set;
        }

        [DataSource]
        public IEnumerableDataSource<FundamentalValue> Dividend
        {
            get;
            set;
        }

        public override IndicatorResult Calculate( IIndicatorContext context )
        {
            return new IndicatorResult( Name, context.Stock, new NeutralSignal( 50 ) );
        }
    }
}
