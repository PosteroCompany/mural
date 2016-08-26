using System;

namespace PosteroOrg.Mural
{
    public class ConfigurationManager
    {
        public static object Get(string key, object _default = null)
        {
            var value = Environment.GetEnvironmentVariable(key);
            if (value == null)
            {
                value = System.Configuration.ConfigurationManager.AppSettings[key];
            }
            return value != null ? value : _default;
        }
    }
}