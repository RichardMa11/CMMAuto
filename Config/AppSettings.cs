using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using log4net;

namespace CMMAuto.Config
{
    public class AppSettings
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(AppSettings));
        private static string filePath = @"AppSettings.ini";
        private static List<string> configs = _CheckExist();

        // check AppSettings.ini exist
        private static List<string> _CheckExist()
        {
            if (File.Exists(filePath))
            {
                return File.ReadAllLines(filePath, Encoding.Default).ToList();
            }
            else
            {
                Directory.CreateDirectory(filePath);
                return new List<string>(0);
            }
        }

        public static string ListAll()
        {
            return "";
        }

        public static string ReadSysValue(string key)
        {
            return ConfigurationManager.AppSettings[key] ?? "";
        }

        public static void WriteSysValue(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings[key] == null)
            {
                log.Info($"add new key: { key }, value: { value }");
                config.AppSettings.Settings.Add(key, value);
            }
            else
            {
                log.Info($"update key: { key }, value: { value }");
                config.AppSettings.Settings[key].Value = value;
            }
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        public static string ReadValue(string section, string key)
        {
            string value = string.Empty;
            for (int i = 0; i < configs.Count; i++)
            {
                var config = configs[i].Trim();
                if (config.StartsWith("[") && config.EndsWith("]"))
                {
                    var curSection = config.Substring(1, config.Length - 2);
                    if (curSection == section)
                    {
                        for (int j = i + 1; j < configs.Count; j++)
                        {
                            if (configs[j].Trim().StartsWith("[")) break;
                            var curConfig = configs[j].Split(new char[] { '=' }, 2);
                            if (curConfig.Length != 2) continue;
                            var curKey = curConfig[0].Trim();
                            if (curKey == key)
                            {
                                value = curConfig[1].TrimStart(' ', '"', '\'').TrimEnd(' ', '"', '\'');
                                break;
                            }
                        }
                    }
                }
            }
            return value;
        }

        public static void WriteValue(string section, string key, string value)
        {
            bool isSectionFound = false;
            for (int i = 0; i < configs.Count - 1; i++)
            {
                var config = configs[i].Trim();
                if (config.StartsWith("[") && config.EndsWith("]"))
                {
                    var curSection = config.Substring(1, config.Length - 2);
                    if (curSection == section)
                    {
                        isSectionFound = true;
                        var isConfigFound = false;
                        for (int j = i + 1; j < configs.Count; j++)
                        {
                            if (configs[j].Trim().StartsWith("[")) break;
                            var curConfig = configs[j].Split(new char[] { '=' }, 2);
                            if (curConfig.Length != 2) continue;
                            var curKey = curConfig[0].Trim();
                            if (curKey == key)
                            {
                                isConfigFound = true;
                                configs[j] = $"{key}={value}";
                                break;
                            }
                        }
                        if (!isConfigFound)
                        {
                            configs.Insert(i + 1, $"{key}={value}");
                        }
                    }
                }
            }

            if (!isSectionFound)
            {
                configs.Add($"[{section}]");
                configs.Add($"{key}={value}");
            }

            File.WriteAllLines(filePath, configs, Encoding.Default);
        }
    }
}
