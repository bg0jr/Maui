using System.IO;
using Maui.Configuration;

namespace Maui.Data
{
    internal class Config : ConfigurationBase
    {
        public Config()
            : base( "Maui.Data" )
        {
        }

        public string TomDatabaseUri
        {
            get
            {
                return Path.GetFullPath( this[ "TOM.Database.Uri" ] );
            }
            set
            {
                this[ "TOM.Database.Uri" ] = Path.GetFullPath( value );
            }
        }
    }
}
