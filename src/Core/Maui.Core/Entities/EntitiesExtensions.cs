using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Maui.Entities
{
  public static  class EntitiesExtensions
    {
      public static T GetObjectByKey<T>( this IEntityRepository tom, EntityKey key )
      {
          tom.MetadataWorkspace.LoadFromAssembly( typeof( T ).Assembly );
          var entity = (T)tom.GetObjectByKey( key );
          return entity;
      }
    }
}
