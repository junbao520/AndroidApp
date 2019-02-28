namespace TwoPole.Chameleon3.Foundation.Providers
{
    public interface IConfigurationProvider
    {
        TSettings GetSettings<TSettings>()
            where TSettings : class, ISettings, new();

        void SaveSettings<TSettings>(TSettings settings)
            where TSettings : class, ISettings, new();

        void DeleteSettings<TSettings>()
            where TSettings : class, ISettings, new();
    }
}
