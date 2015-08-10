﻿using System;
using System.Text;
using System.Text.RegularExpressions;
using Blade;

namespace Maui.Data.Recognition.Spec
{
    /// <summary>
    /// Keep immutable!
    /// TODO: maybe we should have a generic FormatColumn passing the type info via generic type param
    /// </summary>
    [Serializable]
    public class FormatColumn : ValueFormat
    {
        public FormatColumn( string name )
            : this( name, typeof( string ), null )
        {
        }

        public FormatColumn( string name, Type type )
            : this( name, type, null )
        {
        }

        public FormatColumn( string name, Type type, string format )
            : this( name, type, format, null )
        {
        }
        public FormatColumn( string name, Type type, string format, Regex extractionPattern )
            : base( type, format, extractionPattern )
        {
            this.Require( x => !string.IsNullOrEmpty( name ) );

            Name = name;
        }

        /// <summary>
        /// Create a clone.
        /// </summary>
        public FormatColumn( FormatColumn other )
            : base( other )
        {
            this.Require( x => other != null );

            Name = other.Name;
        }

        public string Name { get; private set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append( "Name = " );
            sb.Append( Name );
            sb.Append( ", " );
            sb.Append( base.ToString() );

            return sb.ToString();
        }
    }
}
