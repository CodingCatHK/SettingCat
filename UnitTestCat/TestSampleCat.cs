using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestCat
{
    [TestClass]
    public class TestSampleCat
    {
        [TestMethod]
        public void TestLoadFromFile()
        {
            var FullPathToSettingDirectory = @"Settings\";
            var SettingInstance = GlobalSettings.Instance;
            var PureSettingInstance = GlobalSettings.PureInstance;

            PrintConnectionSetting("Pure:", PureSettingInstance);

            LoadSettingFromFile(
                FullPathToSettingDirectory,
                SettingCat.Environments.DEFAULT,
                PureSettingInstance.ConnectionStringSetting
            );
            PrintConnectionSetting("Pure (demo loaded):", PureSettingInstance);

            LoadSettingFromFile(
                FullPathToSettingDirectory,
                SettingCat.Environments.UAT,
                PureSettingInstance.ConnectionStringSetting
            );
            PrintConnectionSetting("Pure (uat loaded):", PureSettingInstance);

            LoadSettingFromFile(
                FullPathToSettingDirectory,
                SettingCat.Environments.PRODUCTION,
                PureSettingInstance.ConnectionStringSetting
            );
            PrintConnectionSetting("Pure (production loaded):", PureSettingInstance);

            System.Diagnostics.Debug.WriteLine("===============================");
            System.Diagnostics.Debug.WriteLine("The prod setting is not be loaded due to connection_string.json indicated it is in UAT env");
            PrintConnectionSetting("Normal setting instance:", SettingInstance);
        }

        [TestMethod]
        public void TestLoadFromString()
        {
            var FullPathToSettingDirectory = @"Settings\";
            var PureSettingInstance = GlobalSettings.PureInstance;
            var SettingFile = string.Format(
                @"{0}\{1}",
                FullPathToSettingDirectory,
                PureSettingInstance.ConnectionStringSetting.SettingsFilename[SettingCat.Environments.DEFAULT.ToString()]
            );
                
            using (var fr = new System.IO.StreamReader(SettingFile))
                PureSettingInstance.ConnectionStringSetting.LoadSettingFromString(
                    fr.ReadToEnd(), SettingCat.FileFormats.JSON
                );
            PrintConnectionSetting("", PureSettingInstance);
        }

        private static void PrintConnectionSetting(string header, GlobalSettings instance)
        {
            System.Diagnostics.Debug.WriteLine("===============================");
            System.Diagnostics.Debug.WriteLine(header);
            System.Diagnostics.Debug.WriteLine(string.Format(
                "--Address:{0}\n--Name:{1}\n--Username:{2}\n--Password:{3}\n\n--ConnectionString:{4}\n",
                instance.ConnectionStringSetting.DatabaseServerNetworkAddress,
                instance.ConnectionStringSetting.NameOfProjectDatabase,
                instance.ConnectionStringSetting.DatabaseUsername,
                instance.ConnectionStringSetting.DatabasePassword,
                instance.ConnectionStringSetting.GetDatabaseConnectionString(true)
            ));
        }

        private static void LoadSettingFromFile(
            string fullPathToDirectory,
            SettingCat.Environments environment,
            SettingCat.Base setting
        )
        {
            try
            {
                setting.LoadSettingFromFile(string.Format(
                    @"{0}\{1}",
                    fullPathToDirectory,
                    setting.SettingsFilename[environment.ToString()]
                ), SettingCat.FileFormats.JSON);
            }
            catch { }
        }
    }
}
