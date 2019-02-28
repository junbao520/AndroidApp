using System;
using TwoPole.Chameleon3.Foundation.Providers;

namespace TwoPole.Chameleon3.Infrastructure
{
    public static class CacheKeys
    {
        public static readonly string SettingPattern = "Settings_";
        public static readonly string AllExamItemSettings = "All_ExamItemSettings";
        public static readonly string AllLightExamItems = "All_LightExamItems";
        public static readonly string AllSettings = "All_Settings";
        public static readonly string GlobalSettings = "GlobalSettings";
        public static readonly string AllMapLines = "All_MapLines";
        public static readonly string RoadVisionSettings = "Road_Vision_Settings";
        public static string GetSettingCacheKey<TSettings>() where TSettings : ISettings
        {
            return GetSettingCacheKey(typeof(TSettings));
        }

        public static string GetMapCacheKey(int mapId)
        {
            return string.Format("Map_{0}", mapId);
        }

        public static string GetSettingCacheKey(Type type)
        {
            return CacheKeys.SettingPattern + type.FullName;
        }
    }
}
