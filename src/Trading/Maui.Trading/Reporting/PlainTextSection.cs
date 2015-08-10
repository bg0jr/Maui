using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Trading.Reporting
{
    public class PlainTextSection : AbstractSection
    {
        private StringBuilder myText;

        public PlainTextSection( string name )
            : base( name )
        {
            myText = new StringBuilder();
        }

        public void AddText( string text )
        {
            myText.Append( text );
        }

        public string GetText()
        {
            return myText.ToString();
        }
    }
}
