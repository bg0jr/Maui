using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Reporting;

namespace Maui.Trading.Modules.Indicators
{
    public class MissingDataReport : Report
    {
        public MissingDataReport( string indicatorName )
            : base( indicatorName + ".error.missingdata", "'" + indicatorName + "' error report" )
        {
            AddErrorDescription();
        }

        private void AddErrorDescription()
        {
            var section = new PlainTextSection( "Error description" );
            section.AddText( "Not enough data to calculate this indicator." );

            Sections.Add( section );
        }
    }
}
