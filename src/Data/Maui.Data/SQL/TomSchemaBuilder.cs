using System.Collections.Generic;
using System.Transactions;
using Blade.Collections;

namespace Maui.Data.SQL
{
    internal class TomSchemaBuilder
    {
        private IDatabaseSC myDB;
        private IList<IDBUpdateAction> myUpdateActions;

        public TomSchemaBuilder()
        {
            myDB = Engine.ServiceProvider.Database();
            myUpdateActions = new List<IDBUpdateAction>();

            BuildUpdateActions();
        }

        public void CreateNewSchema()
        {
            DropSchema();
            CreateSchema();
        }

        public void UpdateSchema()
        {
            CreateSchema();
        }

        private void BuildUpdateActions()
        {
            myUpdateActions.Add( new MetadataUpdateAction(
                "DB_VERSION", "2" ) );

            myUpdateActions.Add( new CreateTableAction(
                "currency",
                "CREATE TABLE currency (" +
                    "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                    "name TEXT NOT NULL," +
                    "symbol TEXT NOT NULL" +
                ");" ) );

            myUpdateActions.Add( new CreateTableAction(
                "stock_exchange",
                "CREATE TABLE stock_exchange (" +
                    "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                    "name TEXT NOT NULL," +
                    "symbol TEXT NOT NULL," +
                    "currency_id INTEGER NOT NULL" +
                ");" ) );

            myUpdateActions.Add( new CreateTableAction(
                "datum_origin",
                "CREATE TABLE datum_origin (" +
                    "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                    "name TEXT NOT NULL" +
                ");" ) );

            myUpdateActions.Add( new CreateTableAction(
                "sector",
                "CREATE TABLE sector (" +
                    "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                    "name TEXT NOT NULL" +
                ");" ) );

            myUpdateActions.Add( new CreateTableAction(
                "sector_aliases",
                "CREATE TABLE sector_aliases (" +
                     "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                     "sector_id INTEGER NOT NULL," +
                     "alias TEXT NOT NULL" +
                 ");" ) );

            myUpdateActions.Add( new CreateTableAction(
                "company",
                "CREATE TABLE company (" +
                    "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                    "name TEXT NOT NULL," +
                    "symbol TEXT NULL," +
                    "comment TEXT NULL," +
                    "business_year TEXT NULL," +
                    "sector_id INTEGER NULL," +
                    "origin_country_id INTEGER NULL" +
                ");" ) );

            myUpdateActions.Add( new CreateTableAction(
                "company_sectors",
                "CREATE TABLE company_sectors (" +
                    "company_id INTEGER NOT NULL," +
                    "sector_id INTEGER NOT NULL" +
                ");" ) );

            myUpdateActions.Add( new CreateTableAction(
                "countries",
                "CREATE TABLE countries (" +
                     "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                     "lcid INTEGER NOT NULL" +
                 ");" ) );

            myUpdateActions.Add( new CreateTableAction(
                "country_aliases",
                "CREATE TABLE country_aliases (" +
                     "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                     "country_id INTEGER NOT NULL," +
                     "alias TEXT NOT NULL" +
                 ");" ) );

            myUpdateActions.Add( new CreateTableAction(
                "stock",
                "CREATE TABLE stock (" +
                     "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                     "isin TEXT NOT NULL," +
                     "type INTEGER NOT NULL," +
                     "company_id INTEGER NOT NULL" +
                 ");" +
                 "CREATE INDEX stock_isin_idx ON stock (isin);" +
                 "CREATE INDEX stock_company_idx ON stock (company_id);" ) );

            myUpdateActions.Add( new CreateTableAction(
                "traded_stock",
                "CREATE TABLE traded_stock (" +
                    "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                    "wpkn TEXT NULL," +
                    "symbol TEXT NOT NULL," +
                    "stock_id INTEGER NOT NULL," +
                    "stock_exchange_id INTEGER NOT NULL" +
                ");" +
                "CREATE INDEX traded_stock_wpkn_idx ON traded_stock (wpkn);" +
                "CREATE INDEX traded_stock_stock_idx ON traded_stock (stock_id);" +
                "CREATE INDEX traded_stock_stock_exchange_idx ON traded_stock (stock_exchange_id);" ) );

            myUpdateActions.Add( new CreateTableAction(
                "catalog",
                "CREATE TABLE catalog (" +
                     "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                     "name TEXT NOT NULL" +
                 ");" ) );

            myUpdateActions.Add( new CreateTableAction(
                "catalog_contents",
                "CREATE TABLE catalog_contents (" +
                     "catalog_id INTEGER NOT NULL," +
                     "traded_stock_id INTEGER NOT NULL" +
                 ");" ) );

            myUpdateActions.Add( new CreateTableAction(
                "stock_price",
                "CREATE TABLE stock_price (" +
                    "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                    "datum_origin_id INTEGER, " +
                    "timestamp TEXT, " +
                    "traded_stock_id INTEGER, " +
                    "date TEXT, " +
                    "open DOUBLE, " +
                    "high DOUBLE, " +
                    "low DOUBLE, " +
                    "close DOUBLE, " +
                    "volume INTEGER " +
                ");" +
                "CREATE INDEX stock_price_traded_stock_id_idx ON stock_price (traded_stock_id);" ) );

            myUpdateActions.Add( new CreateTableAction(
                "orders",
                "CREATE TABLE orders (" +
                    "id INTEGER  PRIMARY KEY AUTOINCREMENT," +
                    "cost DOUBLE NOT NULL," +
                    "indicative_stop DOUBLE," +
                    "is_buy BOOL NOT NULL," +
                    "is_discardable BOOL NOT NULL," +
                    "is_marged BOOL NOT NULL," +
                    "price DOUBLE NOT NULL," +
                    "submission_date TEXT NOT NULL," +
                    "quantity INTEGER NOT NULL," +
                    "source TEXT," +
                    "traded_stock_id INTEGER NOT NULL," +
                    "type SMALLINT NOT NULL" +
                ");" ) );

            myUpdateActions.Add( new CreateTableAction(
                "portfolios",
                "CREATE TABLE portfolios (" +
                    "id INTEGER  PRIMARY KEY AUTOINCREMENT," +
                    "broker TEXT NOT NULL," +
                    "cash DOUBLE NOT NULL," +
                    "initial_sum DOUBLE NOT NULL" +
                ");" ) );

            myUpdateActions.Add( new CreateTableAction(
                "positions",
                "CREATE TABLE positions (" +
                    "id INTEGER  PRIMARY KEY AUTOINCREMENT," +
                    "quantity INTEGER NOT NULL," +
                    "open_date TEXT NOT NULL," +
                    "open_price DOUBLE NOT NULL," +
                    "close_date TEXT," +
                    "close_price DOUBLE," +
                    "is_long BOOL NOT NULL," +
                    "is_marged BOOL NOT NULL," +
                    "stop_price DOUBLE," +
                    "source TEXT," +
                    "intended_to_close BOOL NOT NULL," +
                    "traded_stock_id INTEGER NOT NULL" +
                ");" ) );

            myUpdateActions.Add( new CreateTableAction(
                "position_orders",
                "CREATE TABLE position_orders (" +
                    "position_id INTEGER NOT NULL," +
                    "order_id INTEGER NOT NULL," +
                    "type smallint NOT NULL" +
                ");" +
                "CREATE VIEW position_pending_orders AS " +
                    "SELECT position_id, order_id " +
                    "FROM position_orders " +
                    "WHERE type = 1" +
                ";" +
                "CREATE VIEW position_executed_orders AS " +
                    "SELECT position_id, order_id " +
                    "FROM position_orders " +
                    "WHERE type = 2" +
                ";" ) );
        }

        private void DropSchema()
        {
            if ( !myDB.ExistsDatabase() )
            {
                return;
            }

            using ( var trans = new TransactionScope() )
            {
                myUpdateActions.Foreach( action => action.Rollback( myDB ) );

                trans.Complete();
            }
        }

        private void CreateSchema()
        {
            if ( !myDB.ExistsDatabase() )
            {
                myDB.CreateDatabase();
            }

            using ( var trans = new TransactionScope() )
            {
                myUpdateActions.Foreach( action => action.Execute( myDB ) );

                trans.Complete();
            }
        }
    }
}
