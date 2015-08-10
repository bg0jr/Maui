using System.Linq;
using Maui;

namespace Maui.Dynamics.Data
{
    public static class ServiceProviderExtensions
    {
        public static ScriptingInterface TomScripting( this ServiceProvider serviceProvider )
        {
            if ( !serviceProvider.RegisteredServices.Contains( typeof( ScriptingInterface ).ToString() ) )
            {
                ScriptingInterface service = new ScriptingInterface();
                service.Init( serviceProvider );

                serviceProvider.RegisterService( typeof( ScriptingInterface ), service );
            }

            return serviceProvider.GetService<ScriptingInterface>();
        }
    }
}
