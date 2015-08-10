using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Maui.Tools.Studio.Controller;
using System.ComponentModel;

namespace Maui.Tools.Studio.Controls
{
    /// <summary/>
    public partial class EntitiyGrouping : UserControl
    {
        private TreeViewItem mySelectedItem;
        private IEntityGroupingController myController;

        private class TreeDragInfo : EntityDragInfo
        {
            public TreeDragInfo( TreeViewItem item )
                : base( item.DataContext )
            {
                Item = item;
            }

            public TreeViewItem Item
            {
                get;
                private set;
            }
        }

        public EntitiyGrouping()
        {
            InitializeComponent();

            Title = "unknown";
        }

        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// Source of the groups.
        /// </summary>
        public IEnumerable ItemsSource
        {
            get;
            set;
        }

        /// <summary>
        /// Binding path to form the relationship between group entity and element entity.
        /// </summary>
        public string GroupingBindingPath
        {
            get;
            set;
        }

        public string GroupNameBindingPath
        {
            get;
            set;
        }

        public string ElementNameBindingPath
        {
            get;
            set;
        }

        public string SortingDescription
        {
            get;
            set;
        }

        public event EventHandler<ValueChangedEventArgs> GroupNameChanged;

        public void Initialize( IEntityGroupingController controller )
        {
            myController = controller;

            var template = LoadDataTemplate();
            template.ItemsSource = new Binding( GroupingBindingPath );
            template.GroupNameChanged += OnGroupNameChanged;

            myTree.ItemTemplate = template;
            myTree.DataContext = DataContext;
            myTree.ItemsSource = ItemsSource;
            myTree.Items.SortDescriptions.Add( new SortDescription( SortingDescription, ListSortDirection.Ascending ) );

            myTree.Drop += myRelationshipTree_Drop;
            myTree.DragOver += myRelationshipTree_DragOver;
            myTree.AddHandler( TreeViewItem.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler( myRelationshipTree_MouseLeftButtonDown ) );
            myTree.AddHandler( TreeViewItem.PreviewKeyUpEvent, new KeyEventHandler( myRelationshipTree_KeyUp ) );
        }

        private EntityGroupingDataTemplate LoadDataTemplate()
        {
            var stream = GetType().Assembly.GetManifestResourceStream( "Maui.Tools.Studio.Controls.EntityGrouping.DataTemplate.xaml" );
            return EntityGroupingDataTemplate.Load( stream, GroupNameBindingPath, ElementNameBindingPath );
        }

        private void myRelationshipTree_MouseLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            var dragAction = DragDropEffects.None;

            if ( ( Keyboard.Modifiers & ModifierKeys.Shift ) != ModifierKeys.None )
            {
                dragAction = DragDropEffects.Move;
            }
            else if ( ( Keyboard.Modifiers & ModifierKeys.Control ) != ModifierKeys.None )
            {
                dragAction = DragDropEffects.Copy;
            }

            if ( dragAction == DragDropEffects.None )
            {
                return;
            }

            var draggedItem = myTree.GetItemAtLocation<TreeViewItem>( MouseUtilities.GetMousePosition( myTree ) );
            if ( draggedItem == null )
            {
                return;
            }

            var dragInfo = new TreeDragInfo( draggedItem );
            dragInfo.Action = dragAction;

            DragDrop.DoDragDrop( myTree, dragInfo.ToDataObject(), dragInfo.Action );
            e.Handled = true;
        }

        private void myRelationshipTree_DragOver( object sender, DragEventArgs e )
        {
            try
            {
                var dragTarget = myTree.GetDataContextAtLocation<object>( MouseUtilities.GetMousePosition( myTree ) );
                var dragInfo = (EntityDragInfo)e.Data.GetData( EntityDragInfo.DataFormat );
                var draggedItem = dragInfo.Entity;

                if ( myController.IsGroupingAllowed( draggedItem, dragTarget ) )
                {
                    e.Effects = dragInfo.Action;
                }
                else
                {
                    e.Effects = DragDropEffects.None;
                }
            }
            finally
            {
                e.Handled = true;
            }
        }

        private void myRelationshipTree_Drop( object sender, DragEventArgs e )
        {
            var dragInfo = (EntityDragInfo)e.Data.GetData( EntityDragInfo.DataFormat );
            var draggedItem = dragInfo.Entity;
            var dragTarget = myTree.GetDataContextAtLocation<object>( MouseUtilities.GetMousePosition( myTree ) );

            if ( dragInfo.Action == DragDropEffects.Move )
            {
                var treeDragInfo = dragInfo as TreeDragInfo;
                var oldGroup = treeDragInfo == null ? null : GetGroupEntity( treeDragInfo.Item );
                TryAction( () => myController.MoveElementToGroup( dragTarget, draggedItem, oldGroup ) );
            }
            else if ( dragInfo.Action == DragDropEffects.Copy )
            {
                TryAction( () => myController.AddElemntToGroup( dragTarget, draggedItem ) );
            }

            e.Handled = true;
        }

        private void TryAction( Action action )
        {
            try
            {
                action();
            }
            catch ( NotSupportedException ex )
            {
                MessageBox.Show( ex.Message );
            }
        }

        private object GetGroupEntity( TreeViewItem item )
        {
            var treeViewParent = ItemsControl.ItemsControlFromItemContainer( item );

            var logicalParent = treeViewParent is TreeViewItem ? treeViewParent.DataContext : null;

            return logicalParent;
        }

        private void myRelationshipTree_KeyUp( object sender, KeyEventArgs e )
        {
            if ( myTree.SelectedItem == null )
            {
                return;
            }

            if ( mySelectedItem.Tag != null && (bool)mySelectedItem.Tag )
            {
                // in edit mode
                e.Handled = false;
                return;
            }

            if ( e.Key == Key.Back )
            {
                var logicalParent = GetGroupEntity( mySelectedItem );
                if ( logicalParent == null )
                {
                    // now we are sure that the selected item is no parent but a child

                    TryAction( () => myController.ReleaseGrouping( logicalParent, myTree.SelectedItem ) );

                    e.Handled = true;
                }
            }
            else if ( e.Key == Key.F2 )
            {
                // http://blogs.msdn.com/b/wpfsdk/archive/2007/04/16/how-do-i-programmatically-interact-with-template-generated-elements-part-ii.aspx
                mySelectedItem.Tag = true;

                e.Handled = true;
            }
        }

        private void OnGroupNameChanged( object sender, ValueChangedEventArgs e )
        {
            if ( GroupNameChanged != null )
            {
                GroupNameChanged( sender, e );
            }
        }

        private void myTree_Selected( object sender, RoutedEventArgs e )
        {
            mySelectedItem = (TreeViewItem)e.OriginalSource;
        }

        private void myNewMenu_Click( object sender, RoutedEventArgs e )
        {
            myController.CreateGroup();

        }

        private void myDeleteMenu_Click( object sender, RoutedEventArgs e )
        {
            myController.Delete( GetGroupEntity( mySelectedItem ), myTree.SelectedItem );
        }
    }
}
