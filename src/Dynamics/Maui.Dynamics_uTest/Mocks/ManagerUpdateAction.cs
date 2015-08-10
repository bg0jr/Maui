using System.Collections.Generic;
using System.IO;
using Maui.Entities;
using NMock2;
using NMock2.Monitoring;
using System.Reflection;
using System.Globalization;
using System;

namespace Maui.Dynamics.UnitTest.Mocks
{
    public class ManagerUpdateAction<TDatum> : IAction
    {
        private object myManager = null;
        //private int myIdSeq = 0;

        public ManagerUpdateAction( object manager )
        {
            myManager = manager;
            Values = new List<TDatum>();
        }

        public void Invoke( Invocation invocation )
        {
            if ( invocation.Receiver == myManager && invocation.Method.Name == "Add" )
            {
                // TODO: FIX
                //ITradingObjectTestAccess param = (ITradingObjectTestAccess)invocation.Parameters[ 0 ];
                //param.SetId( myIdSeq++ );

                //Values.Add( (TDatum)param );
            }
        }

        public IList<TDatum> Values { get; set; }

        public void DescribeTo( TextWriter writer )
        {
        }
    }
}
