using Maui.Data.SQL;
using Maui;
using System.Linq;

namespace Maui.Data
{
    public static class ServiceProviderExtensions
    {
        public static IDatabaseSC Database( this ServiceProvider serviceProvider )
        {
            if ( !serviceProvider.RegisteredServices.Contains( "TOM Database" ) )
            {
                return null;
            }

            return (IDatabaseSC)serviceProvider.GetService( "TOM Database" );
        }
    }
}
