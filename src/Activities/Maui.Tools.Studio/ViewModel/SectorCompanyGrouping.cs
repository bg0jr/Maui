using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Entities;
using Maui;
using Blade.Collections;
using System.ComponentModel.Composition;

namespace Maui.Tools.Studio.ViewModel
{
    [Export( typeof( Grouping ) )]
    public class SectorCompanyGrouping : Grouping<Sector, Company>
    {
        protected override void Release( Sector group, Company element )
        {
            element.Sectors.Remove( group );
        }

        protected override void MoveElementToGroup( Sector group, Company element, Sector oldGroup )
        {
            element.Sectors.Remove( oldGroup );
            element.Sectors.Add( group );
        }

        protected override void AddElemntToGroup( Sector group, Company element )
        {
            element.Sectors.Add( group );
        }
    }
}
