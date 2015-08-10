using System.Collections.Generic;
using System.Data;
using System.Linq;
using Blade.Data;
using Blade.Reflection;

namespace Maui.Dynamics.Data
{
    public class Datum : TableSchema
    {
        public static DataColumn[] Preset = 
            { 
                 TableSchema.CreateReference( "datum_origin" ),
                 new DataColumn( "timestamp", typeof( string ) ) { AllowDBNull = false }
            };

        public Datum( string name, bool isPersistent, params DataColumn[] columns )
            : this( name, isPersistent, (IEnumerable<DataColumn>)columns )
        {
        }

        public Datum( string name, bool isPersistent, IEnumerable<DataColumn> columns )
            : base( name, Preset.Union( columns, new DataColumnComparer() ), isPersistent )
        {
        }

        public Datum( string name, params DataColumn[] columns )
            : this( name, false, columns )
        {
        }

        public Datum( string name, IEnumerable<DataColumn> columns )
            : this( name, false, columns )
        {
        }

        public Datum Clone( params TransformAction[] rules )
        {
            return new Datum(
                rules.ApplyTo<string>( () => this.Name ),
                rules.ApplyTo<bool>( () => this.IsPersistent ),
                rules.ApplyTo<IEnumerable<DataColumn>>( () => this.Columns ) );
        }
    }
}
