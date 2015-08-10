using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;
using Maui.Trading.Data;

namespace Maui.Trading.Binding.Decorators
{
    public class PriceSeriesDataSourceFactory : AbstractDataSourceFactory
    {
        private Map<string, IList<IPriceSeriesOperator>> myOperators;

        public PriceSeriesDataSourceFactory( IDataSourceFactory realFactory )
        {
            if ( realFactory == null )
            {
                throw new ArgumentNullException( "realFactory" );
            }

            RealFactory = realFactory;

            myOperators = new Map<string, IList<IPriceSeriesOperator>>();
        }

        public IDataSourceFactory RealFactory
        {
            get;
            private set;
        }

        // key: name of the datasource, value: set of registered operators
        // global operators valid for all datasources have string.empty as key
        public IMap<string, IList<IPriceSeriesOperator>> Operators
        {
            get
            {
                // TODO: try to use ienumerable here for value
                return myOperators;
            }
        }

        public void AddOperator( string dataSource, IPriceSeriesOperator op )
        {
            if ( !myOperators.ContainsKey( dataSource ) )
            {
                myOperators[ dataSource ] = new List<IPriceSeriesOperator>();
            }

            myOperators[ dataSource ].Add( op );
        }

        public void AddOperator( IPriceSeriesOperator op )
        {
            AddOperator( string.Empty, op );
        }

        public override bool CanCreate( string name, Type type )
        {
            return ( type == typeof( IPriceSeriesDataSource ) && RealFactory.CanCreate( name, typeof( IEnumerableDataSource<SimplePrice> ) ) )
                || RealFactory.CanCreate( name, type );
        }

        protected override object CreateDataSource( string name, Type type, NamedParameters ctorArgs )
        {
            if ( type == typeof( IPriceSeriesDataSource ) )
            {
                var realDS = RealFactory.Create<IEnumerableDataSource<SimplePrice>>( name, ctorArgs );
                var operators = GetOperatorsForDataSource( name );
                return new PriceSeriesDataSource( realDS, operators );
            }

            return RealFactory.Create( name, type, ctorArgs );
        }

        private IEnumerable<IPriceSeriesOperator> GetOperatorsForDataSource( string dataSource )
        {
            var globalOperators = Operators.Contains( string.Empty ) ? Operators[ string.Empty ] : new IPriceSeriesOperator[] { };
            var dataSourceOperators = Operators.Contains( dataSource ) ? Operators[ dataSource ] : new IPriceSeriesOperator[] { };

            return globalOperators.Concat( dataSourceOperators );
        }
    }
}
