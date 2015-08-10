using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui;
using Maui.Entities;

namespace Maui.Dynamics
{
    public static class CalcTool
    {
        public static double Yield( this MauiX.ICalc self, StockPrice from, StockPrice to )
        {
            return ( to.Close - from.Close ) * 100 / from.Close;
        }
    }
}
