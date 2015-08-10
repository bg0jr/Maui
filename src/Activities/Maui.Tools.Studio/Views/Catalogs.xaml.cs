using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Maui.Entities;
using Maui.Tools.Studio.Controller;
using Maui.Tools.Studio.Controls;
using Maui.Tools.Studio.ViewModel;
using Microsoft.Practices.Unity;

namespace Maui.Tools.Studio.Views
{
    /// <summary>
    /// Interaction logic for Catalogs.xaml
    /// </summary>
    public partial class Catalogs : Window
    {
        private IEntityRepository myTom;
        private TomViewModel myTomViewModel;

        public Catalogs()
        {
            InitializeComponent();

            myTom = Engine.ServiceProvider.CreateEntityRepository();
        }

        private void Window_Loaded( object sender, RoutedEventArgs e )
        {
            var ioc = new IoCContainer();
            ioc.SetupDefaults();

            var container = ioc.Container;

            myTomViewModel = new TomViewModel( myTom );
            container.RegisterInstance<TomViewModel>( myTomViewModel );

            myStocks.DataContext = myTom;
            myStocks.ItemsSource = myTom.TradedStocks.ToList();
            myStocks.Items.SortDescriptions.Add( new SortDescription( "Company.Name", ListSortDirection.Ascending ) );
            myStocks.AddHandler( ListBoxItem.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler( myStocks_MouseLeftButtonDown ) );
            myStocks.AddHandler( ListBoxItem.PreviewMouseMoveEvent, new MouseEventHandler( myStocks_MouseMove ) );

            myCatalogs.DataContext = myTomViewModel;
            myCatalogs.ItemsSource = myTomViewModel.StockCatalogs;
            using ( var child = container.CreateChildContainer() )
            {
                child.RegisterInstance<EntitiyGrouping>( myCatalogs );
                myCatalogs.Initialize( child.Resolve<StockCatalogController>() );
            }
        }

        private Point myLastClickPos;

        private void myStocks_MouseLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            myLastClickPos = e.GetPosition( myStocks );
        }

        private void myStocks_MouseMove( object sender, MouseEventArgs e )
        {
            // Get the current mouse position
            Point mousePos = e.GetPosition( myStocks );
            Vector diff = myLastClickPos - mousePos;

            if ( e.LeftButton == MouseButtonState.Pressed &&
                Math.Abs( diff.X ) > SystemParameters.MinimumHorizontalDragDistance &&
                Math.Abs( diff.Y ) > SystemParameters.MinimumVerticalDragDistance )
            {
                var draggedItem = myStocks.GetItemAtLocation<ListBoxItem>( myLastClickPos );
                if ( draggedItem == null )
                {
                    return;
                }

                var dragInfo = new EntityDragInfo( draggedItem.DataContext );
                dragInfo.Action = DragDropEffects.Copy;

                DragDrop.DoDragDrop( myStocks, dragInfo.ToDataObject(), dragInfo.Action );
                e.Handled = true;
            }
        }

        private void myOkBtn_Click( object sender, RoutedEventArgs e )
        {
            myTomViewModel.SaveChanges();

            Close();
        }

        private void myCancelBtn_Click( object sender, RoutedEventArgs e )
        {
            Close();
        }

        protected override void OnClosed( EventArgs e )
        {
            base.OnClosed( e );

            myTom.Dispose();
        }
    }
}
