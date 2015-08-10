using System;
using Blade;

namespace Maui.Entities
{
    public partial class Sector
    {
        public Sector()
        {
        }

        public Sector( string name )
        {
            Name = name;
        }

        partial void OnNameChanging( string value )
        {
            if ( string.IsNullOrWhiteSpace( value ) )
            {
                throw new ArgumentException( "Name must not be null, empty or whitespace only" );
            }
        }
        
        partial void OnNameChanged()
        {
            var trimmedValue = Name.TrimOrNull();
            if ( trimmedValue != Name )
            {
                Name = trimmedValue;
            }
        }
    
        public override string ToString()
        {
            return Name;
        }
    }
}
