using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Entities;
using Maui;
using Blade.Collections;
using System.ComponentModel.Composition;
using Maui.Logging;
using Blade.Logging;

namespace Maui.Tools.Studio.ViewModel
{
    [Export( typeof( Grouping ) )]
    public class SectorSectorGrouping : Grouping<Sector, Sector>
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( SectorSectorGrouping ) );
        
        private TomViewModel myTom;

        [ImportingConstructor]
        public SectorSectorGrouping( TomViewModel tom )
        {
            myTom = tom;
        }

        protected override void Release( Sector group, Sector element )
        {
            throw new NotSupportedException( "Grouping between Sectors cannot be released" );
        }

        protected override void MoveElementToGroup( Sector group, Sector element, Sector oldGroup )
        {
            myLogger.Info( "{0} becomes alias of {1}", element.Name, group.Name );

            var alias = new SectorAlias();
            alias.Name = element.Name;
            alias.Sector = group;

            myTom.SectorAliases.Add( alias );

            element.Companies
                .ToList()
                .Foreach( group.Companies.Add );

            element.Aliases
                .ToList()
                .Foreach( group.Aliases.Add );

            myTom.Sectors.Remove( element );
        }

        protected override void AddElemntToGroup( Sector group, Sector element )
        {
            throw new NotSupportedException( "Sector cannot be copied as element to another sector" );
        }
    }
}
