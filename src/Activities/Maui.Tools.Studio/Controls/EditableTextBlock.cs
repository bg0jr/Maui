using System.Windows.Controls;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Data;
using System;
using System.ComponentModel;
using System.Data;

namespace Maui.Tools.Studio.Controls
{
    /// <summary>
    /// see: http://www.codeproject.com/KB/edit/EditableTextBlock_in_WPF.aspx
    /// </summary>
    public class EditableTextBlock : TextBlock
    {
        private string myOldText;
        private EditableTextBlockAdorner myAdorner;

        public event EventHandler<ValueChangedEventArgs> TextChanged;

        public EditableTextBlock()
        {
            // http://weblogs.asp.net/okloeten/archive/2007/09/18/3940922.aspx
            var prop = DependencyPropertyDescriptor.FromProperty( TextBlock.TextProperty, typeof( TextBlock ) );
            prop.AddValueChanged( this, OnTextChanged );

            myOldText = Text;
        }

        private void OnTextChanged( object sender, EventArgs e )
        {
            try
            {
                if ( TextChanged == null )
                {
                    return;
                }

                var args = new ValueChangedEventArgs( myOldText, Text );
                TextChanged( this, args );
            }
            finally
            {
                myOldText = Text;
            }
        }

        public bool IsInEditMode
        {
            get
            {
                return (bool)GetValue( IsInEditModeProperty );
            }
            set
            {
                SetValue( IsInEditModeProperty, value );
            }
        }

        // Using a DependencyProperty as the backing store for IsInEditMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsInEditModeProperty =
            DependencyProperty.Register( "IsInEditMode", typeof( bool ), typeof( EditableTextBlock ), new UIPropertyMetadata( false, IsInEditModeUpdate ) );

        /// <summary>
        /// Determines whether [is in edit mode update] [the specified obj].
        /// </summary>
        private static void IsInEditModeUpdate( DependencyObject obj, DependencyPropertyChangedEventArgs e )
        {
            EditableTextBlock textBlock = obj as EditableTextBlock;
            if ( textBlock == null )
            {
                return;
            }

            //Get the adorner layer of the uielement (here TextBlock)
            AdornerLayer layer = AdornerLayer.GetAdornerLayer( textBlock );

            //If the IsInEditMode set to true means the user has enabled the edit mode then
            //add the adorner to the adorner layer of the TextBlock.
            if ( textBlock.IsInEditMode )
            {
                if ( null == textBlock.myAdorner )
                {
                    textBlock.myAdorner = new EditableTextBlockAdorner( textBlock );
                }
                layer.Add( textBlock.myAdorner );
            }
            else
            {
                //Remove the adorner from the adorner layer.
                Adorner[] adorners = layer.GetAdorners( textBlock );
                if ( adorners != null )
                {
                    foreach ( Adorner adorner in adorners )
                    {
                        if ( adorner is EditableTextBlockAdorner )
                        {
                            layer.Remove( adorner );
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the length of the max.
        /// </summary>
        /// <value>The length of the max.</value>
        public int MaxLength
        {
            get
            {
                return (int)GetValue( MaxLengthProperty );
            }
            set
            {
                SetValue( MaxLengthProperty, value );
            }
        }

        // Using a DependencyProperty as the backing store for MaxLength.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register( "MaxLength", typeof( int ), typeof( EditableTextBlock ), new UIPropertyMetadata( 0 ) );

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseDown"/> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        protected override void OnMouseDown( MouseButtonEventArgs e )
        {
            if ( e.MiddleButton == MouseButtonState.Pressed )
            {
                IsInEditMode = true;
            }
            else if ( e.ClickCount == 2 )
            {
                IsInEditMode = true;
            }
        }
    }
}