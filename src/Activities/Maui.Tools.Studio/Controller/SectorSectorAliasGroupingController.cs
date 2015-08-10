using System;
using System.ComponentModel.Composition;
using Maui.Entities;
using Maui.Tools.Studio.Controls;
using Maui.Tools.Studio.ViewModel;

namespace Maui.Tools.Studio.Controller
{
    public class SectorSectorAliasGroupingController : AbstractEntityGroupingController
    {
        private TomViewModel myTom;

        [ImportingConstructor]
        public SectorSectorAliasGroupingController( TomViewModel tom, EntityGroupingManager groupingMgr, EntitiyGrouping view )
            : base( groupingMgr )
        {
            myTom = tom;

            view.GroupNameChanged += OnGroupNameChanged;
        }

        private void OnGroupNameChanged( object sender, ValueChangedEventArgs e )
        {
            var sector = sender as Sector;
            if ( sector == null )
            {
                return;
            }

            if ( e.OldValue == TomViewModel.UNDEFINED_ENTITY_NAME )
            {
                return;
            }

            var alias = new SectorAlias();
            alias.Name = e.OldValue;
            alias.Sector = sector;

            myTom.SectorAliases.Add( alias );
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

            var alias = item as SectorAlias;
            if ( alias != null )
            {
                myTom.SectorAliases.Remove( alias );
                return;
            }

            throw new NotSupportedException( "Cannot delete items of type: " + item.GetType() );
        }
    }
}
