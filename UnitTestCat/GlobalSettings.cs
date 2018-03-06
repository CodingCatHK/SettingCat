using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestCat
{
    public class GlobalSettings
    {
        private static object obj = new object();
        private static GlobalSettings _INSTANCE;
        public static GlobalSettings Instance
        {
            get
            {
                lock (obj) { if (_INSTANCE == null) _INSTANCE = new GlobalSettings(true); }
                return _INSTANCE;
            }
        }

        // -- Used for testing only
        internal static GlobalSettings PureInstance { get { return new GlobalSettings(false); } }

        public SampleCat.ConnectionStringSetting ConnectionStringSetting { get; private set; }

        private GlobalSettings(bool loadFromDefaultFiles)
        {
            this.ConnectionStringSetting = new SampleCat.ConnectionStringSetting();

            if (loadFromDefaultFiles)
                this.ConnectionStringSetting.LoadSettingFromDefaultFiles(
                    @"Settings\", SettingCat.FileFormats.JSON
                );
        }
    }
}
