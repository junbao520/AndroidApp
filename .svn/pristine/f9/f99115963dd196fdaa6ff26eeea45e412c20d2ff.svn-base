//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;
//using TwoPole.Chameleon3.Domain;
//using TwoPole.Chameleon3.Foundation.Providers;

//namespace TwoPole.Chameleon3.Infrastructure.Services
//{
//    public class ConfigurationProvider : IConfigurationProvider
//    {
//        private const string SettingGroupName = "Setting";
//        private readonly IRepository<Setting> _settingRepository;

//        public ConfigurationProvider(IRepository<Setting> settingRepository)
//        {
//            _settingRepository = settingRepository;
//        }

//        public TSettings GetSettings<TSettings>()
//            where TSettings : class, ISettings, new()
//        {
//            var settingKey = typeof(TSettings).Name;
//            var settingItems = this.GetSettingItems(settingKey);
//            var settings = GetSettingsCore<TSettings>(null, settingItems, settingKey);
//            return settings;
//        }

//        public void SaveSettings<TSettings>(TSettings settings)
//            where TSettings : class, ISettings, new()
//        {
//            if (settings == null)
//                throw new ArgumentNullException("settings");

//            var settingKey = typeof(TSettings).Name;
//            var settingItems = this.GetSettingItems(settingKey);
//            SaveSettingsCore(settings, settingItems, settingKey);
//            _settingRepository.Flush();
//        }

//        public void DeleteSettings<TSettings>()
//            where TSettings : class, ISettings, new()
//        {
//            var key = typeof(TSettings).Name + ".";
//            var settings = _settingRepository.Table.Where(d => d.Key.StartsWith(key) && d.GroupName == SettingGroupName);
//            foreach (var setting in settings)
//                _settingRepository.Delete(setting);

//            _settingRepository.Flush();
//        }

//        private TSettings GetSettingsCore<TSettings>(TSettings settings, IList<Setting> settingItems, string parentKey)
//           where TSettings : class, ISettings, new()
//        {
//            settings = settings ?? new TSettings();
//            var properties = from prop in typeof(TSettings).GetProperties(BindingFlags.Public | BindingFlags.Instance)
//                             where prop.CanWrite && prop.CanRead
//                             where prop.GetGetMethod(true).GetParameters().Length == 0
//                             where settingItems.Any(d => d.Key.StartsWith(parentKey + ".", StringComparison.OrdinalIgnoreCase))
//                             select prop;
//            foreach (var property in properties)
//            {
//                string key = parentKey + "." + property.Name;
//                if (typeof(ISettings).IsAssignableFrom(property.PropertyType))
//                {
//                    var method = typeof(ConfigurationProvider).GetMethod("GetSettingsCore", BindingFlags.Instance | BindingFlags.NonPublic).MakeGenericMethod(property.PropertyType);
//                    var value = method.Invoke(this, new object[] { property.GetValue(settings), settingItems, key });
//                    property.SetValue(settings, value);
//                }
//                else
//                {
//                    var setting = settingItems.FirstOrDefault(d => string.Equals(d.Key, key, StringComparison.OrdinalIgnoreCase));
//                    if (setting != null)
//                    {
//                        var converter = TypeDescriptor.GetConverter(property.PropertyType);
//                        if (converter.CanConvertFrom(typeof(string)) && converter.IsValid(setting.Value))
//                        {
//                            var value = converter.ConvertFromInvariantString(setting.Value);
//                            property.SetValue(settings, value);
//                        }
//                    }
//                }
//            }

//            return settings;
//        }

//        private void SaveSettingsCore<TSettings>(TSettings settings, IList<Setting> settingItems, string parentKey)
//            where TSettings : class, ISettings, new()
//        {
//            var properties = from prop in typeof(TSettings).GetProperties(BindingFlags.Public | BindingFlags.Instance)
//                             where prop.CanWrite && prop.CanRead
//                             where prop.GetGetMethod(true).GetParameters().Length == 0
//                             select prop;

//            foreach (var prop in properties)
//            {
//                string key = parentKey + "." + prop.Name;
//                if (typeof(ISettings).IsAssignableFrom(prop.PropertyType))
//                {
//                    var method = typeof(ConfigurationProvider).GetMethod("SaveSettingsCore", BindingFlags.Instance | BindingFlags.NonPublic).MakeGenericMethod(prop.PropertyType);
//                    method.Invoke(this, new object[] { prop.GetValue(settings), settingItems, key });
//                }
//                else
//                {
//                    var converter = TypeDescriptor.GetConverter(prop.PropertyType);
//                    if (converter.CanConvertFrom(typeof(string)))
//                    {
//                        dynamic value = prop.GetValue(settings, null);
//                        var valueStr = value == null ? string.Empty : converter.ConvertToInvariantString(value);
//                        var settingItem = settingItems.FirstOrDefault(d => string.Equals(d.Key, key, StringComparison.OrdinalIgnoreCase));
//                        if (settingItem == null)
//                        {
//                            settingItem = new Setting();
//                            settingItem.GroupName = SettingGroupName;
//                            settingItem.Key = key;
//                            settingItem.Value = valueStr;
//                            _settingRepository.Create(settingItem);
//                        }
//                        else
//                        {
//                            settingItem.Value = valueStr;
//                            _settingRepository.Update(settingItem);
//                        }
//                    }
//                }
//            }
//        }

//        private IList<Setting> GetSettingItems(string key)
//        {
//            key = key + ".";
//            var settings = _settingRepository.Table.Where(s => s.Key.StartsWith(key) && s.GroupName == SettingGroupName).ToList();
//            return settings;
//        }
//    }
//}
