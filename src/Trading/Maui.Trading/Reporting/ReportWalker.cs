using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Reporting
{
    public class ReportWalker
    {
        private IReportVisitor myVisitor;
        // avoid cyclic linking and duplicate linking
        private List<object> myAlreadyVisited;

        public ReportWalker( IReportVisitor visitor )
        {
            if ( visitor == null )
            {
                throw new ArgumentNullException( "visitor" );
            }

            myVisitor = visitor;

            myAlreadyVisited = new List<object>();
        }

        public void Visit( Report report )
        {
            if ( CheckAlreadyVisited( report ) ) return;

            myVisitor.Visit( report );

            foreach ( var section in report.Sections )
            {
                Visit( section );
            }
        }

        private bool CheckAlreadyVisited( object node )
        {
            if ( myAlreadyVisited.Contains( node ) )
            {
                return true;
            }

            myAlreadyVisited.Add( node );
            return false;
        }

        private void Visit( AbstractSection section )
        {
            if ( CheckAlreadyVisited( section ) ) return;

            myVisitor.Visit( section );

            var combinedReport = section as IReportGrouping;
            if ( combinedReport != null )
            {
                Visit( combinedReport );
            }
        }

        private void Visit( IReportGrouping combinedReport )
        {
            foreach ( var report in combinedReport.DetailedReports )
            {
                Visit( report );
            }
        }
    }
}
