using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui
{
    /// <summary/>
    public static class MauiStringExtensions
    {
        /// <summary/>
        public static string ToNullIfEmpty( this string str )
        {
            return string.IsNullOrEmpty( str ) ? null : str;
        }

        /// <summary/>
        public static string ToStringOrEmpty( this object obj )
        {
            return obj != null ? obj.ToString() : string.Empty;
        }
    }
}
