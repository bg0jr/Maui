using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blade;
using HtmlAgilityPack;

namespace Maui.Tools.Scripts
{
    [CLSCompliant(false)]
    public static class HtmlDocumentOfflineExtensions
    {
        public static void RemoveWebReferences( this HtmlDocument doc )
        {
            TraverseNodeTree( doc.DocumentNode, RemoveWebReferences );
        }

        public static void TraverseNodeTree( HtmlNode root, Action<HtmlNode> visitorAction )
        {
            foreach ( HtmlNode node in root.ChildNodes )
            {
                visitorAction( node );

                TraverseNodeTree( node, visitorAction );
            }
        }

        public static void RemoveWebReferences( HtmlNode node )
        {
            RemoveScriptTag( node );
            RemoveImgTag( node );
            RemoveIFrameTag( node );
            RemoveLinkTag( node );
            RemoveEmbedTag( node );
            RemoveComment( node );
        }

        private static void RemoveScriptTag( HtmlNode node )
        {
            if ( !node.Name.EqualsI( "script" ) ) return;

            node.RemoveAll();
        }

        private static void RemoveImgTag( HtmlNode node )
        {
            if ( !node.Name.EqualsI( "img" ) ) return;

            node.RemoveAll();
        }

        private static void RemoveIFrameTag( HtmlNode node )
        {
            if ( !node.Name.EqualsI( "iframe" ) ) return;

            node.Attributes.RemoveAll();
        }
        private static void RemoveLinkTag( HtmlNode node )
        {
            if ( !node.Name.EqualsI( "link" ) ) return;

            node.Attributes.RemoveAll();
        }
        private static void RemoveEmbedTag( HtmlNode node )
        {
            if ( !node.Name.EqualsI( "embed" ) ) return;

            node.Attributes.RemoveAll();
        }

        private static void RemoveComment( HtmlNode node )
        {
            if ( node.NodeType != HtmlNodeType.Comment ) return;

            node.RemoveAll();
        }
    }
}
