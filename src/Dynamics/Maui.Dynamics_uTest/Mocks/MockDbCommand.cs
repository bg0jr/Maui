using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections;

namespace Maui.Dynamics.UnitTest.Mocks
{
    public class MockDbCommand : DbCommand
    {
        public override void Cancel()
        {
        }

        public override string CommandText
        {
            get
            {
                return string.Empty;
            }
            set
            {
            }
        }

        public override int CommandTimeout
        {
            get
            {
                return 0;
            }
            set
            {
            }
        }

        public override CommandType CommandType
        {
            get
            {
                return CommandType.Text;
            }
            set
            {
            }
        }

        protected override DbParameter CreateDbParameter()
        {
            return null;
        }

        protected override DbConnection DbConnection
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        protected override DbParameterCollection DbParameterCollection
        {
            get
            {
                return new MockDbParameterCollection();
            }
        }

        protected override DbTransaction DbTransaction
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public override bool DesignTimeVisible
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        protected override DbDataReader ExecuteDbDataReader( CommandBehavior behavior )
        {
            return null;
        }

        public override int ExecuteNonQuery()
        {
            return 0;
        }

        public override object ExecuteScalar()
        {
            return null;
        }

        public override void Prepare()
        {
        }

        public override UpdateRowSource UpdatedRowSource
        {
            get
            {
                return UpdateRowSource.None;
            }
            set
            {
            }
        }
    }

    public class MockDbParameterCollection : DbParameterCollection
    {
        private ArrayList myList = new ArrayList();

        public override int Add( object value )
        {
            return -1;
        }

        public override void AddRange( Array values )
        {
        }

        public override void Clear()
        {
        }

        public override bool Contains( string value )
        {
            return false;
        }

        public override bool Contains( object value )
        {
            return false;
        }

        public override void CopyTo( Array array, int index )
        {
        }

        public override int Count
        {
            get { return 0; }
        }

        public override System.Collections.IEnumerator GetEnumerator()
        {
            return myList.GetEnumerator();
        }

        protected override DbParameter GetParameter( string parameterName )
        {
            return null;
        }

        protected override DbParameter GetParameter( int index )
        {
            return null;
        }

        public override int IndexOf( string parameterName )
        {
            return -1;
        }

        public override int IndexOf( object value )
        {
            return -1;
        }

        public override void Insert( int index, object value )
        {
        }

        public override bool IsFixedSize
        {
            get { return true; }
        }

        public override bool IsReadOnly
        {
            get { return true; }
        }

        public override bool IsSynchronized
        {
            get { return false; }
        }

        public override void Remove( object value )
        {
        }

        public override void RemoveAt( string parameterName )
        {
        }

        public override void RemoveAt( int index )
        {
        }

        protected override void SetParameter( string parameterName, DbParameter value )
        {
        }

        protected override void SetParameter( int index, DbParameter value )
        {
        }

        public override object SyncRoot
        {
            get { return null; }
        }
    }
}
