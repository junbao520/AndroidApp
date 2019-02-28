namespace TwoPole.Chameleon3.Infrastructure.Devices.SensorProviders
{
    public interface ICarSignalParser
    {
        CarSignalInfo Parse(string[] commands);
    }
}
