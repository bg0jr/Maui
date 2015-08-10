using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Binding.Decorators;
using Maui.Trading.Utils;
using Maui.Trading.Data;

namespace Maui.Trading.Binding
{
    public static class FluentDataSourceFactoryBuilder
    {
        public static CachingDataSourceFactory EnableCaching( this IDataSourceFactory self )
        {
            return new CachingDataSourceFactory( self );
        }

        public static StackDataSourceFactory UseAsFallbacks( this IDataSourceFactory self, params IDataSourceFactory[] fallbacks )
        {
            return new StackDataSourceFactory( self.Join( fallbacks ).ToArray() );
        }

        public static PriceSeriesDataSourceFactory SupportPriceSeries( this IDataSourceFactory self )
        {
            return new PriceSeriesDataSourceFactory( self );
        }

        public static ApplyOperatorSyntax ApplyOperator( this PriceSeriesDataSourceFactory self, IPriceSeriesOperator op )
        {
            return new ApplyOperatorSyntax( self, op );
        }
    }

    public class ApplyOperatorSyntax
    {
        private PriceSeriesDataSourceFactory myFactory;
        private IPriceSeriesOperator myOperator;

        public ApplyOperatorSyntax( PriceSeriesDataSourceFactory factory, IPriceSeriesOperator op )
        {
            myFactory = factory;
            myOperator = op;
        }

        public PriceSeriesDataSourceFactory For( string dataSource )
        {
            myFactory.AddOperator( dataSource, myOperator );
            return myFactory;
        }

        public PriceSeriesDataSourceFactory Always
        {
            get
            {
                myFactory.AddOperator( myOperator );
                return myFactory;
            }
        }
    }
}
