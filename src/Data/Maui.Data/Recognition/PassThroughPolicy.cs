﻿using System.Data;
using Maui.Data.Recognition.Spec;

namespace Maui.Data.Recognition
{
    /// <summary>
    /// Implements a fetch policy which directly passes the input 
    /// data through without any modification.
    /// </summary>
    public class PassThroughPolicy : IFetchPolicy
    {
        /// <summary>
        /// Returns the navigation object of the given site object 
        /// without any modification.
        /// <seealso cref="IFetchPolicy.GetNavigation"/>
        /// </summary>
        public virtual Navigation GetNavigation( Site site )
        {
            return site.Navigation;
        }

        /// <summary>
        /// Returns the form object of the given site object 
        /// without any modification.
        /// <seealso cref="IFetchPolicy.GetFormat"/>
        /// </summary>
        public virtual IFormat GetFormat( Site site )
        {
            return site.Format;
        }

        /// <summary>
        /// Returns the given raw date without any modification.
        /// <seealso cref="IFetchPolicy.ApplyPreprocessing"/>
        /// </summary>
        public virtual DataTable ApplyPreprocessing( DataTable table )
        {
            return table;
        }
    }

}
