using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Entities;
using System.Collections.ObjectModel;
using System.Data;
using System.Collections.Specialized;

namespace Maui.Tools.Studio.ViewModel
{
    public class TomViewModel
    {
        private IEntityRepository myTom;
        private SelfTrackingEntitySet<Company> myCompanies;
        private SelfTrackingEntitySet<StockCatalog> myStockCatalogs;
        private SelfTrackingEntitySet<Sector> mySectors;
        private SelfTrackingEntitySet<SectorAlias> mySectorAliases;

        public static readonly string UNDEFINED_ENTITY_NAME = "<undefined>";

        public TomViewModel( IEntityRepository tom )
        {
            myTom = tom;
        }

        public ObservableCollection<Company> Companies
        {
            get
            {
                if ( myCompanies == null )
                {
                    myCompanies = new SelfTrackingEntitySet<Company>( myTom.Companies.ToList() );
                }

                return myCompanies;
            }
        }

        public ObservableCollection<StockCatalog> StockCatalogs
        {
            get
            {
                if ( myStockCatalogs == null )
                {
                    myStockCatalogs = new SelfTrackingEntitySet<StockCatalog>( myTom.StockCatalogs.ToList() );
                }

                return myStockCatalogs;
            }
        }

        public ObservableCollection<Sector> Sectors
        {
            get
            {
                if ( mySectors == null )
                {
                    mySectors = new SelfTrackingEntitySet<Sector>( myTom.Sectors.ToList() );
                    mySectors.CollectionChanged += mySectors_CollectionChanged;
                }

                return mySectors;
            }
        }

        private void mySectors_CollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
        {
            if ( e.Action == NotifyCollectionChangedAction.Remove )
            {
                var removedSectors = e.OldItems.OfType<Sector>();
                foreach ( var company in Companies )
                {
                    var sectorsToRemoveFromCompany = company.Sectors.Intersect( removedSectors ).ToList();
                    foreach ( var sector in sectorsToRemoveFromCompany )
                    {
                        company.Sectors.Remove( sector );
                    }
                }
            }
        }

        public ObservableCollection<SectorAlias> SectorAliases
        {
            get
            {
                if ( mySectorAliases == null )
                {
                    mySectorAliases = new SelfTrackingEntitySet<SectorAlias>( myTom.SectorAliases.ToList() );
                    mySectorAliases.CollectionChanged += mySectorAliases_CollectionChanged;
                }

                return mySectorAliases;
            }
        }

        private void mySectorAliases_CollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
        {
            if ( e.Action == NotifyCollectionChangedAction.Remove )
            {
                var removedAliases = e.OldItems.OfType<SectorAlias>();
                foreach ( var sector in Sectors )
                {
                    var aliasesToRemoveFromSector = sector.Aliases.Intersect( removedAliases ).ToList();
                    foreach ( var alias in aliasesToRemoveFromSector )
                    {
                        sector.Aliases.Remove( alias );
                    }
                }
            }
        }

        public void SaveChanges()
        {
            SyncEntitySet( "Companies", myCompanies );
            SyncEntitySet( "Sectors", mySectors );
            SyncEntitySet( "SectorAliases", mySectorAliases );
            SyncEntitySet( "StockCatalogs", myStockCatalogs );

            myTom.SaveChanges();
        }

        private void SyncEntitySet<T>( string entitySetName, SelfTrackingEntitySet<T> entitySet )
        {
            if ( entitySet == null )
            {
                return;
            }

            foreach ( var item in entitySet.AddedItems )
            {
                myTom.AddObject( entitySetName, item );
            }

            foreach ( var item in entitySet.RemovedItems )
            {
                myTom.DeleteObject( item );
            }
        }
    }
}
