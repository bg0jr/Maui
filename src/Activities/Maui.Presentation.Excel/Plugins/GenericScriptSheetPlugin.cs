using System;
using Maui.Tasks.Sheets;

namespace Maui.Presentation.Excel.Plugins
{
    /// <summary>
    /// Generic plugin for InAction script sheets.
    /// </summary>
    public class GenericScriptSheetPlugin : AbstractPlugin, IToolbarButton
    {
        private ISheetTask mySheet;

        /// <summary/>
        public GenericScriptSheetPlugin( ISheetTask sheet, string caption, int buttonFaceId )
        {
            mySheet = sheet;

            Caption = caption;
            FaceId = buttonFaceId;
        }

        /// <summary/>
        public override void Open( IWorksheetContext context )
        {
            base.Open( context );

            mySheet.Open( Context.ActiveWorkbook );
        }

        /// <summary/>
        public override void Close()
        {
            mySheet.Close();

            base.Close();
        }

        /// <summary/>
        public override IToolbarButton ToolbarButton
        {
            get { return this; }
        }

        /// <summary/>
        public string Caption { get; private set; }

        /// <summary/>
        public int FaceId { get; private set; }

        /// <summary/>
        public Action OnClickHandler
        {
            get { return mySheet.Calculate; }
        }
    }
}
