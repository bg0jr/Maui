using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Maui.Trading.Binding
{
    public class BindingContainer
    {
        protected BindingContainer()
        {
        }

        public BindingContainer( IDataSourceFactory dataSourceFactory )
        {
            DataSourceFactory = dataSourceFactory;
        }

        public IDataSourceFactory DataSourceFactory
        {
            get;
            protected set;
        }

        public void Bind( object instance )
        {
            var walker = new ObjectTreeWalker( new BindingVisitor( this ) );
            walker.Visit( instance );
        }

        private class BindingVisitor : IObjectVisitor
        {
            private BindingContainer myContainer;

            public BindingVisitor( BindingContainer container )
            {
                myContainer = container;
            }

            public void Visit( Member member )
            {
                var dataSourceAttr = member.GetDataSourceAttribute();
                if ( dataSourceAttr == null )
                {
                    return;
                }

                myContainer.BindDataSource( member );
            }

            public bool StepInto( Member member )
            {
                return true;
            }
        }

        private void BindDataSource( Member member )
        {
            if ( !DataSourceFactory.CanCreate( member.DataSourceName, member.ReturnType ) )
            {
                throw new Exception( "Cannot bind a datasource to member: " + member.MemberInfo.Name );
            }

            var attr = member.GetDataSourceAttribute();
            var dataSource = DataSourceFactory.Create( member.DataSourceName, member.ReturnType, attr.GetConstructorArguments() );
            member.Value = dataSource;
        }
    }
}
