using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Shell;
using Maui.Data.SQL;
using Blade.Shell;

namespace Maui.Data.Scripts
{
    public class CreateDbSchema : IScript
    {
        /// <summary>
        /// Can only be set via Xaml starter hook
        /// </summary>
        public bool UpdateOnly
        {
            get;
            set;
        }

        public void Run( string[] args )
        {
            var builder = new TomSchemaBuilder();
            if ( UpdateOnly )
            {
                builder.UpdateSchema();
            }
            else
            {
                builder.CreateNewSchema();
            }
        }
    }
}
