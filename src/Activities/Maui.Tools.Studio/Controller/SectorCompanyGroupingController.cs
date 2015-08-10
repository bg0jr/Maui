using System;
using System.ComponentModel.Composition;
using Maui.Entities;
using Maui.Tools.Studio.ViewModel;

namespace Maui.Tools.Studio.Controller
{
    public class SectorCompanyGroupingController : AbstractEntityGroupingController
    {
        private TomViewModel myTom;

        [ImportingConstructor]
        public SectorCompanyGroupingController( TomViewModel tom, EntityGroupingManager groupingMgr )
            : base( groupingMgr )
        {
            myTom = tom;
        }

        public override void CreateGroup()
        {
            myTom.Sectors.Add( new Sector( TomViewModel.UNDEFINED_ENTITY_NAME ) );
        }

        public override void Delete( object group, object item )
        {
            if ( item == null )
            {
                throw new ArgumentNullException( "item" );
            }

            var sector = item as Sector;
            if ( sector != null )
            {
                myTom.Sectors.Remove( sector );
                return;
            }

            var company = item as Company;
            if ( company != null )
            {
                myTom.Companies.Remove( company );
                return;
            }

            throw new NotSupportedException( "Cannot delete items of type: " + item.GetType() );
        }
    }
}
