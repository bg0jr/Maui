using System.Windows;
using System.Windows.Threading;
using Maui.Dynamics;
using Maui;

namespace Maui.Tools.Studio
{
    /// <summary/>
    public partial class App : Application
    {
        private void OnStartUp( object sender, StartupEventArgs e )
        {
            var application = e.Args.Length > 0 ? e.Args[ 0 ] : null;

            Engine.Boot( application );

            Engine.Run();

            var interpreter = new Interpreter();
            interpreter.Init( Engine.ServiceProvider );

            // just register it so that it can be cleaned up later
            Engine.ServiceProvider.RegisterService( interpreter.GetType(), interpreter );

            interpreter.Start();


        }

        private void OnUnhandledException( object sender, DispatcherUnhandledExceptionEventArgs e )
        {
            MessageBox.Show( e.Exception.ToString(), "Unhandled exception" );
        }
    }
}
