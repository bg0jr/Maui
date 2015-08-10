
using Maui.Data.Sheets;
namespace Maui.Presentation.Excel
{
    /// <summary>
    /// Base class for all plugins.
    /// </summary>
    public abstract class AbstractPlugin
    {
        /// <summary/>
        protected IWorksheetContext Context { get; private set; }

        /// <summary/>
        public abstract IToolbarButton ToolbarButton { get; }

        /// <summary>
        /// Open/activate the plugin. At the time of calling the environment
        /// for the plugin has been set up and the plugin can start working.
        /// </summary>
        public virtual void Open( IWorksheetContext context )
        {
            Context = context;
        }

        /// <summary>
        /// Close/deactivate the plugin. The plugin needs to stop all activities
        /// at this point so that the environment can be shut down.
        /// </summary>
        public virtual void Close()
        {
        }
    }
}
