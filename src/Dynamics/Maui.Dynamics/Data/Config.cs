using System;
using Blade;
using Maui.Configuration;

namespace Maui.Dynamics.Data
{
    /// <summary>
    /// Access class to the MSL config.
    /// </summary>
    public class Config : ConfigurationBase<ScriptingInterface>
    {
        protected Config()
            : base()
        { }

        private static Config myInstance = null;
        public static Config Instance
        {
            get
            {
                if ( myInstance == null )
                {
                    myInstance = new Config();
                }
                return myInstance;
            }
        }

        public string DailyStockPriceTable
        {
            get
            {
                return this[ "Tables.DailyStockPrices" ];
            }
        }

        public string[] DatumOriginRanking
        {
            get
            {
                return this[ "DatumOrigin.Ranking" ].Split( new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries );
            }
        }

        public bool DatumOriginMergingAllowed
        {
            get
            {
                return this[ "DatumOrigin.MergingAllowed" ].IsTrue();
            }
        }
    }
}
