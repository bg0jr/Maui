using System.Data.EntityClient;
using System.Linq;
using Maui;

namespace Maui.Entities
{
    public static class ServiceProviderExtensions
    {
        public static IEntityRepository CreateEntityRepository( this ServiceProvider provider )
        {
            var factory = Engine.ServiceProvider.GetService<IEntityRepositoryFactory>();
            return factory.Create();
        }
    }
}
