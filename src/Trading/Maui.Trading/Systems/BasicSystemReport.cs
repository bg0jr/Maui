using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Indicators;
using Maui.Trading.Reporting;

namespace Maui.Trading.Modules
{
    public class BasicSystemReport : Report
    {
        private AdditionalReportsSection myAdditionalReportsSection;

        public BasicSystemReport( string systemName, DateTime dateUnderUnderAnalysis, SystemResult result )
            : base( systemName, "Analyis of system: " + result.Stock.Name )
        {
            SystemResult = result;

            AddSections( dateUnderUnderAnalysis );
        }

        public SystemResult SystemResult
        {
            get;
            private set;
        }

        private void AddSections( DateTime dateUnderUnderAnalysis )
        {
            AddOverviewSection( dateUnderUnderAnalysis );

            Sections.Add( new StockSection( SystemResult.Stock ) );
            Sections.Add( CreateSummarySection() );

            SystemDetails = new SystemDetailsSection();
            Sections.Add( SystemDetails );

            myAdditionalReportsSection = new AdditionalReportsSection();
            Sections.Add( myAdditionalReportsSection );
        }

        private void AddOverviewSection( DateTime dateUnderUnderAnalysis )
        {
            var section = new KeyValueSection( "Overview" );

            section.Entries[ "Trading system" ] = SystemResult.System;
            section.Entries[ "Date under analysis" ] = dateUnderUnderAnalysis.ToShortDateString();

            Sections.Add( section );
        }

        private AbstractSection CreateSummarySection()
        {
            var section = new KeyValueSection( "Summary" );

            section.Entries[ "Signal" ] = SystemResult.Signal;
            section.Entries[ "ExpectedGain" ] = SystemResult.ExpectedGain;
            section.Entries[ "GainRiskRatio" ] = SystemResult.GainRiskRatio;

            return section;
        }
        
        public SystemDetailsSection SystemDetails
        {
            get;
            private set;
        }

        public IList<Report> AdditionalReports
        {
            get
            {
                return myAdditionalReportsSection.AdditionalReports;
            }
        }
    }
}
