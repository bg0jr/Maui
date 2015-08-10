using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Indicators;
using Maui.Trading.Reporting;
using Maui.Trading.Modules;
using Maui.Trading.Model;

namespace Maui.Trading.Evaluation
{
    public class BackTestingReport : Report
    {
        private TableSection myTestResultSection;

        public BackTestingReport( string systemName, DateTime dateUnderUnderAnalysis )
            : base( "BackTesting", "Simple back testing" )
        {
            AddOverviewSection( systemName, dateUnderUnderAnalysis );

            myTestResultSection = new TableSection( "Result", new TableHeader( "Stock", "Gain" ) );
            myTestResultSection.View = new OrderByGainView( myTestResultSection );
            Sections.Add( myTestResultSection );
        }

        private void AddOverviewSection( string systemName, DateTime dateUnderUnderAnalysis )
        {
            var section = new KeyValueSection( "Overview" );
            section.Entries[ "Trading system" ] = systemName;
            section.Entries[ "Date under analysis" ] = dateUnderUnderAnalysis.ToShortDateString();

            Sections.Add( section );
        }

        public void AddResult( TradingResult result )
        {
            var row = new TableRow( myTestResultSection );
            row[ "Stock" ] = result.Stock;
            row[ "Gain" ] = new ValueWithDetails( result.GainPerAnno, new BackTestingStockReport( result ) );

            myTestResultSection.Rows.Add( row );
        }

        private class OrderByGainView : AbstractTableView
        {
            public OrderByGainView( TableSection table )
                : base( table )
            {
            }

            public override IEnumerable<TableRow> Rows
            {
                get
                {
                    return Table.Rows
                        .OrderByDescending( r => ( (ValueWithDetails)r[ "Gain" ] ).Value )
                        .ToList();
                }
            }
        }
    }
}
