using System;
using System.IO;
using System.Windows.Markup;

namespace Maui.Data.Scripts
{
    [MarkupExtensionReturnType( typeof( Func<bool> ) )]
    public class DatabaseExistsExtension : MarkupExtension
    {
        public override object ProvideValue( IServiceProvider serviceProvider )
        {
            var config = new Config();
            Func<bool> func = () => File.Exists( config.TomDatabaseUri );
            return func;
        }
    }
}
