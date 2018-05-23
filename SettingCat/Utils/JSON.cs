using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SettingCat.Utils
{
    internal class JSON : BaseFileUtil
    {
        public JSON(string fullFilename) : base(fullFilename) { }

        public override BaseFileUtil PopulateStringToSettingInstance(object instance)
        {
            if (string.IsNullOrEmpty(base.FileContent)) return this;

            var Settings = new JsonSerializerSettings() { ObjectCreationHandling = ObjectCreationHandling.Replace };
            Settings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
            JsonConvert.PopulateObject(base.FileContent, instance, Settings);

            return this;
        }

        public override BaseFileUtil PopulateStringToSettingInstance(object instance, string settings)
        {
            var Settings = new JsonSerializerSettings() { ObjectCreationHandling = ObjectCreationHandling.Replace };
            Settings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
            JsonConvert.PopulateObject(settings, instance, Settings);

            return this;
        }
    }
}
