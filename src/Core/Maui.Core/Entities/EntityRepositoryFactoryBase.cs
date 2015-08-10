using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Entities
{
    public abstract class EntityRepositoryFactoryBase : IEntityRepositoryFactory
    {
        protected EntityRepositoryFactoryBase()
        {
        }

        public IEntityRepository Create()
        {
            return new TomEntities( GetConnectionString() );
        }

        protected abstract string GetConnectionString();
    }
}
