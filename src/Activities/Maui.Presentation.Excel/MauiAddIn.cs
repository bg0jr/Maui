using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Blade.Collections;
using Maui;
using MSExcel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;

namespace Maui.Presentation.Excel
{
    public partial class MauiAddIn
    {
        private Office.CommandBar myToolbar;
        // we have to collect all the buttons in a list or as member
        // otherwise the click handler will not work -> dont know why :(
        private IList<Office.CommandBarButton> myToolbarButtons = new List<Office.CommandBarButton>();

        private PluginManager myPluginManager;

        private void OnStartup( object sender, System.EventArgs e )
        {
            try
            {
                myPluginManager = new PluginManager( Application );

                SetupToolbar();
                SetupMauiAndMsl();

                Application.WorkbookActivate += OnWorkbookActivate;
            }
            catch ( Exception ex )
            {
                ReportError( ex.ToString() );
            }
        }

        // gets called everytime switching between woorkbooks but
        // actually we only want to do it once (if the proper workbook
        // has been loaded)
        private void OnWorkbookActivate( MSExcel.Workbook workbook )
        {
            if ( myPluginManager.IsActivated )
            {
                return;
            }

            SetupPlugins();
        }

        private void SetupMauiAndMsl()
        {
            Engine.Boot( "Excel" );

            Engine.Run();
        }

        private void SetupPlugins()
        {
            myPluginManager.Activate();

            myPluginManager.Plugins.Foreach( plugin => AddToolbarButton( plugin.ToolbarButton ) );
        }

        private void SetupToolbar()
        {
            myToolbar = Application.CommandBars.Add( "Maui", Office.MsoBarPosition.msoBarTop, false, true );
            if ( myToolbar == null )
            {
                return;
            }

            AddReloadButton();

            myToolbar.Visible = true;
        }

        private void AddReloadButton()
        {
            AddToolbarButton( new AddinReloadButton( OnReloadAddIn ) );
        }

        private void OnReloadAddIn()
        {
            ShutdownPlugins();

            // add this point we could create default config because
            // we expect that if the user pressed this button he is working
            // in an maui workbook
            new ExcelWorkbook( Application ).CreateConfigSheet();

            SetupPlugins();
        }

        private void AddToolbarButton( IToolbarButton buttonDescriptor )
        {
            var button = (Office.CommandBarButton)myToolbar.Controls.Add(
                Office.MsoControlType.msoControlButton, missing, missing, missing, true );

            button.Caption = buttonDescriptor.Caption;
            button.FaceId = buttonDescriptor.FaceId;

            button.Click += CreateHandler( buttonDescriptor.OnClickHandler );

            myToolbarButtons.Add( button );
        }

        private Office._CommandBarButtonEvents_ClickEventHandler CreateHandler( Action onCommand )
        {
            return ( Office.CommandBarButton ctrl, ref bool cancel ) =>
            {
                try
                {
                    ctrl.Enabled = false;

                    onCommand();
                }
                catch ( Exception ex )
                {
                    ReportError( ex );
                }
                finally
                {
                    ctrl.Enabled = true;
                }
            };
        }

        private void OnShutdown( object sender, System.EventArgs e )
        {
            Application.WorkbookActivate -= OnWorkbookActivate;

            ShutdownPlugins();

            ShutdownMauiAndMsl();
        }

        private void ShutdownPlugins()
        {
            myPluginManager.Plugins.Foreach( plugin => RemoveToolbarButton( plugin.ToolbarButton ) );

            myPluginManager.Deactivate();
        }

        private void RemoveToolbarButton( IToolbarButton buttonDescriptor )
        {
            var button = myToolbarButtons.SingleOrDefault( b => b.Caption == buttonDescriptor.Caption );
            if ( button == null )
            {
                return;
            }

            button.Delete( false );
        }

        private void ShutdownMauiAndMsl()
        {
            Engine.Shutdown();
        }

        public void ReportError( Exception exception )
        {
            ReportError( exception.Dump() );
        }

        public void ReportError( string message )
        {
            MessageBox.Show( message );
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += OnStartup;
            this.Shutdown += OnShutdown;
        }

        #endregion
    }
}
