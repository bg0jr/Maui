using System;
using System.Windows;
using Maui.Entities;
using Maui.Tools.Studio.Controller;
using Maui.Tools.Studio.Controls;
using Maui.Tools.Studio.ViewModel;
using Microsoft.Practices.Unity;

namespace Maui.Tools.Studio.Views
{
    /// <summary>
    /// Interaction logic for Sectors.xaml
    /// </summary>
    public partial class Sectors : Window
    {
        private IEntityRepository myTom;
        private TomViewModel myTomViewModel;

        public Sectors()
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

            mySectorsAliasesTree.DataContext = myTomViewModel;
            mySectorsAliasesTree.ItemsSource = myTomViewModel.Sectors;
            using ( var child = container.CreateChildContainer() )
            {
                child.RegisterInstance<EntitiyGrouping>( mySectorsAliasesTree );
                mySectorsAliasesTree.Initialize( child.Resolve<SectorSectorAliasGroupingController>() );
            }

            mySectorsCompaniesTree.DataContext = myTomViewModel;
            mySectorsCompaniesTree.ItemsSource = myTomViewModel.Sectors;
            mySectorsCompaniesTree.Initialize( container.Resolve<SectorCompanyGroupingController>() );
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
