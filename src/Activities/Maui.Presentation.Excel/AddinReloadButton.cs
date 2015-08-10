using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Presentation.Excel
{
    public class AddinReloadButton : IToolbarButton
    {
        public AddinReloadButton( Action reloadAction )
        {
            OnClickHandler = reloadAction;
        }

        public string Caption
        {
            get { return "Reload add-in"; }
        }

        public int FaceId
        {
            get { return 610; /* earth */ }
        }

        public Action OnClickHandler
        {
            get;
            private set;
        }
    }
}
