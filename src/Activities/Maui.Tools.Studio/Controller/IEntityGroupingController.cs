
namespace Maui.Tools.Studio.Controller
{
    public interface IEntityGroupingController
    {
        bool IsGroupingAllowed( object element, object group );
        void ReleaseGrouping( object group, object element );
        void MoveElementToGroup( object group, object element, object oldGroup );
        void AddElemntToGroup( object group, object element );

        void CreateGroup();
        void Delete( object group, object item );
    }
}
