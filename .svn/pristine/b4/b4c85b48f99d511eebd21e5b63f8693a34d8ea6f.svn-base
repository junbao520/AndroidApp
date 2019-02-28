//using System;
//using DCommon.Caching;
//using DCommon.Events;
//using DCommon.Utility;
//using TwoPole.Chameleon3.Foundation.Providers;

//namespace TwoPole.Chameleon3.Infrastructure.Services
//{
//    public class SettingService : ISettingService
//    {
//        private readonly ICacheSet _cache;
//        private readonly IEventPublisher _eventPublisher;
//        private readonly IConfigurationProvider _configurationProvider;

//        public SettingService(ICacheSet cache,
//            IEventPublisher eventPublisher,
//            IConfigurationProvider configurationProvider)
//        {
//            _cache = cache;
//            _eventPublisher = eventPublisher;
//            _configurationProvider = configurationProvider;
//        }

//        public TSettings GetSettings<TSettings>() where TSettings : class, ISettings, new()
//        {
//            var cacheKey = CacheKeys.GetSettingCacheKey<TSettings>();
//            var setting = _cache.Get(cacheKey, _configurationProvider.GetSettings<TSettings>);
//            return setting;
//        }

//        public void SaveSettings<TSettings>(TSettings settings) where TSettings : class, ISettings, new()
//        {
//            Guard.Against<ArgumentNullException>(settings == null, "settings");

//            _configurationProvider.SaveSettings(settings);
//            _eventPublisher.EntityUpdated<ISettings>(settings);
//        }
//    }
//}
