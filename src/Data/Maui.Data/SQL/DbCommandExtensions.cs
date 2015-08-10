using System.Data.Common;
using System.Linq;
using Blade;

namespace Maui.Data.SQL
{
    public static class DbCommandExtensions
    {
        /// <summary>
        /// Determines the parameters of this command based on the '?'
        /// chars in the CommandText.
        /// </summary>
        public static void InitParameters( this DbCommand cmd )
        {
            cmd.Parameters.Clear();

            cmd.CommandText.ToCharArray().Count( c => c == '?' ).
                Times( i => cmd.Parameters.Add( cmd.CreateParameter() ) );
        }
    }
}
