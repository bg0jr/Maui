using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace Maui.Tools.Studio.Controls
{
    public static class TreeViewExtensions
    {

        /// <summary>
        /// Does not work if the same object (DataContext) is stored multiple times in the tree.
        /// </summary>
        public static TreeViewItem ContainerFromItem( this TreeView treeView, object item )
        {
            var containerThatMightContainItem = (TreeViewItem)treeView.ItemContainerGenerator.ContainerFromItem( item );
            if ( containerThatMightContainItem != null )
            {
                return containerThatMightContainItem;
            }
            else
            {
                return ContainerFromItem( treeView.ItemContainerGenerator, treeView.Items, item );
            }
        }

        private static TreeViewItem ContainerFromItem( ItemContainerGenerator parentItemContainerGenerator, ItemCollection itemCollection, object item )
        {
            foreach ( object curChildItem in itemCollection )
            {
                var parentContainer = (TreeViewItem)parentItemContainerGenerator.ContainerFromItem( curChildItem );
                if ( parentContainer == null )
                {
                    continue;
                }

                var containerThatMightContainItem = (TreeViewItem)parentContainer.ItemContainerGenerator.ContainerFromItem( item );

                if ( containerThatMightContainItem != null )
                {
                    return containerThatMightContainItem;
                }

                var recursionResult = ContainerFromItem( parentContainer.ItemContainerGenerator, parentContainer.Items, item );
                if ( recursionResult != null )
                {
                    return recursionResult;
                }
            }

            return null;
        }
    }
}
