using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Entities;

namespace Maui.UnitTests.Entities
{
    public class FakeEntityRepositoryFactory : IEntityRepositoryFactory
    {
        public IEntityRepository Create()
        {
            return new InMemoryEntityRepository();
        }
    }
}
