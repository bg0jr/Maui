using System.Linq;

using Maui.Configuration;

namespace Maui
{
    public static class ServiceProviderExtensions
    {
        public static ConfigurationSC ConfigurationSC( this ServiceProvider serviceProvider )
        {
            if ( !serviceProvider.RegisteredServices.Contains( typeof( ConfigurationSC ).ToString() ) )
            {
                ConfigurationSC service = Maui.Configuration.ConfigurationSC.Instance;
                service.Init( serviceProvider );

                serviceProvider.RegisterService( typeof( ConfigurationSC ), service );
            }

            return serviceProvider.GetService<ConfigurationSC>();
        }
    }
}
