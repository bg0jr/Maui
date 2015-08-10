using System.Windows.Documents;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Maui.Tools.Studio.Controls
{
    /// <summary>
    /// Adorner class which shows textbox over the text block when the Edit mode is on.
    /// </summary>
    public class EditableTextBlockAdorner : Adorner
    {
        private readonly VisualCollection myCollection;
        private readonly TextBox myTextBox;
        private readonly EditableTextBlock myTextBlock;

        public EditableTextBlockAdorner( EditableTextBlock adornedElement )
            : base( adornedElement )
        {
            myTextBlock = adornedElement;

            myTextBox = CreateTextBox( adornedElement );

            myCollection = new VisualCollection( this );
            myCollection.Add( myTextBox );
        }

        private TextBox CreateTextBox( EditableTextBlock adornedElement )
        {
            var textBox = new TextBox();

            Binding binding = new Binding( "Text" )
            {
                Source = adornedElement,
                UpdateSourceTrigger = UpdateSourceTrigger.Explicit
            };
            textBox.SetBinding( TextBox.TextProperty, binding );

            textBox.AcceptsReturn = true;
            textBox.MaxLength = adornedElement.MaxLength;
            textBox.KeyUp += myTextBox_KeyUp;
            textBox.LostFocus += myTextBox_LostFocus;
            textBox.SelectAll();

            return textBox;
        }

        private void myTextBox_KeyUp( object sender, KeyEventArgs e )
        {
            if ( e.Key == Key.Enter )
            {
                myTextBox.Text = myTextBox.Text.Replace( "\r\n", string.Empty );
                myTextBlock.IsInEditMode = false;

                BindingExpression expression = myTextBox.GetBindingExpression( TextBox.TextProperty );
                if ( null != expression )
                {
                    expression.UpdateSource();
                }
                
                e.Handled = true;
            }
            else if ( e.Key == Key.Escape )
            {
                myTextBlock.IsInEditMode = false;
                
                BindingExpression expression = myTextBox.GetBindingExpression( TextBox.TextProperty );
                if ( null != expression )
                {
                    expression.UpdateTarget();
                }
                
                e.Handled = true;
            }
        }

        private void myTextBox_LostFocus( object sender, RoutedEventArgs e )
        {
            myTextBlock.IsInEditMode = false;
        
            BindingExpression expression = myTextBox.GetBindingExpression( TextBox.TextProperty );
            if ( null != expression )
            {
                expression.UpdateTarget();
            }
        }

        protected override Visual GetVisualChild( int index )
        {
            return myCollection[ index ];
        }

        protected override int VisualChildrenCount
        {
            get
            {
                return myCollection.Count;
            }
        }

        protected override Size ArrangeOverride( Size finalSize )
        {
            myTextBox.Arrange( new Rect( 0, 0, myTextBlock.DesiredSize.Width + 50, myTextBlock.DesiredSize.Height * 1.5 ) );
            myTextBox.Focus();
            return finalSize;
        }

        protected override void OnRender( DrawingContext drawingContext )
        {
            drawingContext.DrawRectangle( null, new Pen
                                                   {
                                                       Brush = Brushes.Gold,
                                                       Thickness = 2
                                                   }, new Rect( 0, 0, myTextBlock.DesiredSize.Width + 50, myTextBlock.DesiredSize.Height * 1.5 ) );
        }
    }
}
