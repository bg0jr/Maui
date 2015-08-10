using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;
using System.Xml.Linq;
using System.IO;
using Maui.Entities;
using Blade.Binding;
using Blade;

namespace Maui.Trading.Binding.Xml
{
    /// <remarks>This class needs to be thread-safe</remarks>
    public class XmlDataStore
    {
        private List<DataSources> myStockDataSources;
        private List<AbstractDataSource> myStaticDataSources;
        private IXamlReader myReader;

        public XmlDataStore()
        {
            myStockDataSources = new List<DataSources>();
            myStaticDataSources = new List<AbstractDataSource>();

            myReader = new ValidatingXamlReader();
        }

        public void Add( string dataRoot )
        {
            if ( !Directory.Exists( dataRoot ) )
            {
                // lets accept directories which do not exists. The user might already specified it in the configuration
                // for later use even if it is not yet created
                return;
            }

            foreach ( var file in Directory.GetFiles( dataRoot, "*.xaml" ) )
            {
                var ds = myReader.Read<DataSources>( file );
                Add( ds );
            }
        }

        public void Add( XElement dataSources )
        {
            var ds = myReader.Read<DataSources>( dataSources );
            Add( ds );
        }

        public void Add( params DataSources[] dataSources )
        {
            foreach ( var ds in dataSources )
            {
                if ( ds.Stock == null )
                {
                    myStaticDataSources.AddRange( ds.Sources );
                }
                else
                {
                    myStockDataSources.Add( ds );
                }
            }
        }

        /// <summary>
        /// Gets the data for the specified datasource and stock.
        /// </summary>
        public IEnumerable<AbstractDataSource> GetData( string dataSourceName, StockHandle stock )
        {
            return myStockDataSources
                .Where( ds => ds.Stock.Isin == stock.Isin )
                .SelectMany( ds => ds.Sources )
                .Where( ds => ds.Name == dataSourceName )
                .ToList();
        }

        /// <summary>
        /// Gets the data for the specified datasource (static datasource, stock independent).
        /// </summary>
        public IEnumerable<AbstractDataSource> GetData( string dataSourceName )
        {
            return myStaticDataSources
                .Where( ds => ds.Name == dataSourceName )
                .ToList();
        }
    }
}
