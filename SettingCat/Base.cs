using System;

namespace SettingCat
{
    public abstract class Base
    {
        public abstract System.Collections.Generic.Dictionary<string, string> SettingsFilename { get; protected set; }

        [System.ComponentModel.DefaultValue("DEMO")] public string Environment;

        public Base()
        {
            InitializePropertyDefaultValues(this);
        }

        public Base LoadSettingFromString(string settingString, FileFormats stringFormat)
        {
            var Converter = GetConverter(null, stringFormat);
            if (Converter == null) throw new NotImplementedException();
            Converter.PopulateStringToSettingInstance(this, settingString);

            return this;
        }

        #region Load Setting From File
        public Base LoadSettingFromFile(string fullFilename, FileFormats expectingFormat)
        {
            if (string.IsNullOrEmpty(fullFilename)) return this;
            try { if (!System.IO.File.Exists(fullFilename)) return this; } catch { return this; }

            var Converter = GetConverter(fullFilename, expectingFormat);
            if (Converter == null) throw new NotImplementedException();
            Converter.PopulateStringToSettingInstance(this);

            return this;
        }

        private void LoadSettingFromDefaultFile(
            string fullPathToDirectory,
            Environments environment,
            FileFormats expectingFormat
        )
        {
            if (this.SettingsFilename == null) return;
            if (this.SettingsFilename.Count <= 0) return;

            var Environment = environment.ToString();
            if (!this.SettingsFilename.ContainsKey(Environment)) return;
            this.LoadSettingFromFile(
                string.Format(@"{0}\{1}", fullPathToDirectory, this.SettingsFilename[Environment]),
                expectingFormat
            );
        }

        public Base LoadSettingFromDefaultFiles(string fullPathToDirectory, FileFormats expectingFormat)
        {
            if (this.SettingsFilename == null || this.SettingsFilename.Count <= 0) return this;
            // -- Has to load at least the demo setting file
            this.LoadSettingFromDefaultFile(fullPathToDirectory, Environments.DEFAULT, expectingFormat);

            var IsLoadProduction = this.IsProductionEnvironment();
            var IsLoadUAT = IsLoadProduction || this.IsUATEnvironment();

            if (IsLoadUAT) this.LoadSettingFromDefaultFile(fullPathToDirectory, Environments.UAT, expectingFormat);
            if (IsLoadProduction) this.LoadSettingFromDefaultFile(fullPathToDirectory, Environments.PRODUCTION, expectingFormat);
            return this;
        }
        #endregion

        #region Running Environment
        public bool IsUATEnvironment() { return this.IsThisEnvironment(Environments.UAT); }
        public bool IsProductionEnvironment() { return this.IsThisEnvironment(Environments.PRODUCTION); }

        private bool IsThisEnvironment(Environments environment)
        {
            return this.IsThisEnvironment(environment.ToString());
        }

        public bool IsThisEnvironment(string environment)
        {
            return (this.Environment ?? "").Trim().ToUpper().Equals(
                (environment ?? "").Trim().ToUpper()
            );
        }
        #endregion

        private static void InitializePropertyDefaultValues(object obj)
        {
            foreach (System.Reflection.FieldInfo FieldInfo in obj.GetType().GetFields())
            {
                foreach (Attribute Attribute in FieldInfo.GetCustomAttributes(true))
                {
                    if (Attribute is System.ComponentModel.DefaultValueAttribute)
                    {
                        System.ComponentModel.DefaultValueAttribute DefaultValueAttribute =
                            (System.ComponentModel.DefaultValueAttribute)Attribute;

                        if (DefaultValueAttribute != null) FieldInfo.SetValue(obj, DefaultValueAttribute.Value);
                    }
                }
            }
        }

        private static Utils.BaseFileUtil GetConverter(string fullFilename, FileFormats expectingFormat)
        {
            Utils.BaseFileUtil Converter = null;
            switch (expectingFormat)
            {
                case FileFormats.XML: break;
                case FileFormats.JSON: Converter = new Utils.JSON(fullFilename); break;
            }
            return Converter;
        }
    }

    public enum Environments
    {
        DEFAULT,
        UAT,
        PRODUCTION
    }

    public enum FileFormats
    {
        JSON,
        XML
    }
}
