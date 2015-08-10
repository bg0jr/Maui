using System;
using System.Linq;
using System.Text;
using Blade;
using Maui;

namespace Maui.Entities
{
    public partial class Company
    {
        public Company()
        {
        }

        public Company( string name )
        {
            Name = name;
        }

        private BusinessYear myBusinessYear;

        public BusinessYear BusinessYear
        {
            get
            {
                if ( myBusinessYear == null && RawBusinessYear != null )
                {
                    myBusinessYear = BusinessYear.FromString( RawBusinessYear );
                }
                return myBusinessYear;
            }
            set
            {
                myBusinessYear = value;
                RawBusinessYear = myBusinessYear != null ? myBusinessYear.ToString() : null;
            }
        }

        partial void OnNameChanging( string value )
        {
            ValidateName( value );
        }

        private static void ValidateName( string name )
        {
            if ( string.IsNullOrWhiteSpace( name ) )
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

        partial void OnSymbolChanged()
        {
            var trimmedValue = Symbol.TrimOrNull().ToNullIfEmpty();
            if ( trimmedValue != Symbol )
            {
                Symbol = trimmedValue;
            }
        }

        partial void OnCommentChanged()
        {
            var trimmedValue = Comment.TrimOrNull().ToNullIfEmpty();
            if ( trimmedValue != Comment )
            {
                Comment = trimmedValue;
            }
        }

        /// <summary/>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append( "Company(" );
            sb.Append( "ID = " );
            sb.Append( Id );
            sb.Append( ";Name = " );
            sb.Append( Name );
            sb.Append( ";Symbol = " );
            sb.Append( Symbol );
            sb.Append( ")" );

            return sb.ToString();
        }

        /// <summary>
        /// Adds this company to the given sector if it has not been added
        /// already. Also checks sector aliases of all sectors the company 
        /// is already added to.
        /// </summary>
        public void AddToSectorIfNew( Sector sector )
        {
            if ( Sectors.FirstOrDefault( s => s.Name == sector.Name ) != null )
            {
                return;
            }

            if ( Sectors.SelectMany( s => s.Aliases ).FirstOrDefault( a => a.Name == sector.Name ) != null )
            {
                return;
            }

            Sectors.Add( sector );
        }
    }
}
