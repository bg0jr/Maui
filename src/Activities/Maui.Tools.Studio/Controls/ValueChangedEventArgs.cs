using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Tools.Studio.Controls
{
    public class ValueChangedEventArgs : EventArgs
    {
        public ValueChangedEventArgs( string oldValue, string newValue )
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public string OldValue
        {
            get;
            private set;
        }

        public string NewValue
        {
            get;
            private set;
        }
    }
}
