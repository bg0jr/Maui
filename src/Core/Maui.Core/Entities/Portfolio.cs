using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Entities
{
    /// <summary>
    /// How to suspend the date of it? we actually dont want to 
    /// store the broker in 
    /// </summary>
    public class AbstractBroker
    {

    }

    public partial class Portfolio
    {
        private AbstractBroker myBroker = null;

        public AbstractBroker Broker
        {
            get
            {
                if ( myBroker == null )
                {
                    myBroker = (AbstractBroker)Activator.CreateInstance( Type.GetType( BrokerInternal ) );
                }

                return myBroker;
            }
            set { BrokerInternal = value.GetType().AssemblyQualifiedName; }
        }
    }
}
