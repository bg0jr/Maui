using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Entities
{
    public interface IEntityRepositoryFactory
    {
        IEntityRepository Create();
    }
}
