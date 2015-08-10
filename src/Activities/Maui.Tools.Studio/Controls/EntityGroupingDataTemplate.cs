using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.IO;
using System.Xml.Linq;
using System.Windows.Markup;

namespace Maui.Tools.Studio.Controls
{
    // http://blogs.windowsclient.net/rob_relyea/archive/2009/06/03/xaml-events-in-compiled-and-uncompiled-scenarios.aspx
    public class EntityGroupingDataTemplate : HierarchicalDataTemplate
    {
        public event EventHandler<ValueChangedEventArgs> GroupNameChanged;

        private void OnGroupNameChanged( object sender, ValueChangedEventArgs e )
        {
            var dataContext = ( (FrameworkElement)sender ).DataContext;

            if ( GroupNameChanged != null )
            {
                GroupNameChanged( dataContext, e );
            }
        }

        public static EntityGroupingDataTemplate Load( Stream stream, string groupNameBindingPath, string elementNameBindingPath )
        {
            var root = XElement.Load( stream );

            Func<string, XName> WPF = name => XName.Get( name, @"http://schemas.microsoft.com/winfx/2006/xaml/presentation" );
            Func<string, XName> Local = name => XName.Get( name, @"clr-namespace:Maui.Tools.Studio.Controls;assembly=Maui.Tools.Studio" );

            var parentText = root
                .Element( Local( "EditableTextBlock" ) )
                .Attribute( "Text" );
            parentText.Value = "{Binding Path=" + groupNameBindingPath + ", Mode=TwoWay, NotifyOnTargetUpdated=True}";

            var childText = root
                .Element( WPF( "HierarchicalDataTemplate.ItemTemplate" ) )
                .Element( WPF( "DataTemplate" ) )
                .Element( WPF( "TextBlock" ) )
                .Attribute( "Text" );
            childText.Value = "{Binding Path=" + elementNameBindingPath + "}";

            return (EntityGroupingDataTemplate)XamlReader.Parse( root.ToString() );
        }
    }
}
