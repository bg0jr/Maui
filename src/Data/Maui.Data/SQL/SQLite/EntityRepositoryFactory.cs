using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.EntityClient;
using System.Data;
using System.Configuration;
using System.Data.Objects;
using Maui.Entities;

namespace Maui.Data.SQL.SQLite
{
    internal class EntityRepositoryFactory : EntityRepositoryFactoryBase
    {
        private EntityConnectionStringBuilder myConnectionBuilder;

        public EntityRepositoryFactory( string dbConnectionUri )
        {
            myConnectionBuilder = CreateConnectionBuilder( dbConnectionUri );
        }

        private EntityConnectionStringBuilder CreateConnectionBuilder( string connectionUri )
        {
            // register TOM Entities
            var dataSet = ConfigurationManager.GetSection( "system.data" ) as DataSet;
            var providers = dataSet.Tables[ 0 ].Rows;
            var sqliteProviderRow = providers.OfType<DataRow>()
                .FirstOrDefault( row => row[ 0 ].Equals( "SQLite Data Provider" ) );
            if ( sqliteProviderRow != null )
            {
                providers.Remove( sqliteProviderRow );
            }
            providers.Add(
                 "SQLite Data Provider",
                 ".Net Framework Data Provider for SQLite",
                 "System.Data.SQLite",
                 "System.Data.SQLite.SQLiteFactory, System.Data.SQLite" );

            return new EntityConnectionStringBuilder()
            {
                Provider = "System.Data.SQLite",
                Metadata = CreateMetadataString(),
                ProviderConnectionString = @"data source=" + connectionUri + ";New=false;Enlist=False;Version=3;"
            };
        }

        private string CreateMetadataString()
        {
            var assembly = typeof( BusinessYear ).Assembly.FullName;
            var resourceBaseName = "Entities.TOM";

            return string.Format( @"res://{0}/{1}.csdl|res://{0}/{1}.ssdl|res://{0}/{1}.msl", assembly, resourceBaseName );
        }

        protected override string GetConnectionString()
        {
            return myConnectionBuilder.ConnectionString;
        }
    }
}
