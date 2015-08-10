using System.Collections.Generic;
using System.Linq;
using Blade.Collections;
using Maui.Entities;
using Maui;

namespace Maui.Dynamics.Data
{
    /// <summary>
    /// Represents a datum origin query clause.
    /// mergeAllowed is disabled on default.
    /// If no ranking is passed the data is processed in an undefined order.
    /// </summary>
    public class OriginClause
    {
        /// <summary>
        /// No merge, no ranking.
        /// </summary>
        public OriginClause()
            : this( null )
        {
        }

        public OriginClause( params long[] ranking )
            : this( false, ranking )
        {
        }

        public OriginClause( bool mergeAllowed, IEnumerable<DatumOrigin> ranking )
            : this( mergeAllowed, ranking.Select( o => o.Id ).ToArray() )
        {
        }

        public OriginClause( bool mergeAllowed, params long[] ranking )
        {
            Ranking = new ActiveList<long>( ranking );
            IsMergeAllowed = mergeAllowed;
        }

        public IArray<long> Ranking { get; private set; }
        public bool IsMergeAllowed { get; private set; }

        private static OriginClause myDefault = null;
        public static OriginClause Default
        {
            get
            {
                if ( myDefault == null )
                {
                    myDefault = new OriginClause( Config.Instance.DatumOriginMergingAllowed,
                        GetOrCreate( Config.Instance.DatumOriginRanking ).ToList() );
                }
                return myDefault;
            }
        }

        private static IEnumerable<DatumOrigin> GetOrCreate( params string[] names )
        {
            using ( var tom = Engine.ServiceProvider.CreateEntityRepository() )
            {
                foreach ( var name in names )
                {
                    var origin = tom.DatumOrigins.FirstOrDefault( o => o.Name == name );
                    if ( origin == null )
                    {
                        origin = new DatumOrigin( name );
                        tom.DatumOrigins.AddObject( origin );
                    }

                    yield return origin;
                }
                tom.SaveChanges();
            }
        }
    }
}
