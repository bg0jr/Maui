using Maui.Trading.Binding;
using Maui.Trading.Binding.Tom;
using Maui.Trading.Binding.Xml;
using Maui.Trading.Data;

namespace Maui.Trading.Modules
{
    public class DefaultBindingContainer : BindingContainer
    {
        public DefaultBindingContainer()
        {
            DataSourceFactory = CreateDataSourceFactory();
        }

        public XmlDataStore DataStore
        {
            get;
            private set;
        }

        private IDataSourceFactory CreateDataSourceFactory()
        {
            // TODO: order matters! putting the cache before a decorator which creates a more semantically 
            // datasource (e.g. IPriceSeriesDataSource) results in performance loss because we would then get 
            // the data twice: once for more semantically interface and once for the generic one
            // -> better solution?

            var dataSourceFactory = CreateTomDataSourceFactory()
                .UseAsFallbacks( CreateXmlDataSourceFactory() )
                .EnableCaching()
                .SupportPriceSeries()
                    .ApplyOperator( new InterpolateMissingDatesOperator() ).For( DataSourceNames.Prices );

            return dataSourceFactory;
        }

        private IDataSourceFactory CreateXmlDataSourceFactory()
        {
            DataStore = new XmlDataStore();

            var dataSourceFactory = new XmlDataSourceFactory( DataStore );
            return dataSourceFactory;
        }

        private IDataSourceFactory CreateTomDataSourceFactory()
        {
            var dataSourceFactory = new TomDataSourceFactory();
            return dataSourceFactory;
        }

    }
}
