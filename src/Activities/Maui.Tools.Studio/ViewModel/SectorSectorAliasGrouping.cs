using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Entities;
using Maui;
using System.ComponentModel.Composition;
using Microsoft.Practices.Unity;
using System.Transactions;
using Maui.Logging;
using Blade.Logging;

namespace Maui.Tools.Studio.ViewModel
{
    [Export( typeof( Grouping ) )]
    public class SectorSectorAliasGrouping : Grouping<Sector, SectorAlias>
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( SectorSectorAliasGrouping ) );
        
        private TomViewModel myTom;

        [ImportingConstructor]
        public SectorSectorAliasGrouping( TomViewModel tom )
        {
            myTom = tom;
        }

        protected override void Release( Sector group, SectorAlias element )
        {
            myLogger.Info( "{0} becomes sector again", element.Name );

            var sector = new Sector();
            sector.Name = element.Name;

            myTom.Sectors.Add( sector );
            myTom.SectorAliases.Remove( element );
        }

        protected override void MoveElementToGroup( Sector group, SectorAlias element, Sector oldGroup )
        {
            myLogger.Info( "{0} moving to {1}", element.Name, group.Name );

            element.Sector = group;
        }

        protected override void AddElemntToGroup( Sector group, SectorAlias element )
        {
            if ( element.Sector == null )
            {
                element.Sector = group;
            }
            else
            {
                throw new NotSupportedException( "SectorAlias already pointing to a sector cannot be copied to another sector" );
            }
        }
    }
}
