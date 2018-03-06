using System;
using System.Collections.Generic;
using System.Text;

namespace SettingCat.Utils
{
    internal abstract class BaseFileUtil
    {
        public System.IO.FileInfo FileInfoSetting { get; private set; }
        public string FileContent { get; private set; }

        public BaseFileUtil(string fullFilename)
        {
            this.FileInfoSetting = null;
            this.FileContent = null;

            if (string.IsNullOrEmpty(fullFilename)) return;
            try
            {
                if (System.IO.File.Exists(fullFilename))
                    this.FileInfoSetting = new System.IO.FileInfo(fullFilename);
            }
            catch { }

            if (this.FileInfoSetting != null)
                if (this.FileInfoSetting.Exists)
                    using (var fs = this.FileInfoSetting.OpenRead())
                    using (var sr = new System.IO.StreamReader(fs))
                        this.FileContent = sr.ReadToEnd();
        }

        public abstract BaseFileUtil PopulateStringToSettingInstance(object instance);
        public abstract BaseFileUtil PopulateStringToSettingInstance(object instance, string settings);
    }
}
