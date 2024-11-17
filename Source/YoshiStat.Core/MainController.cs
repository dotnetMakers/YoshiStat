using Meadow;
using System.Threading.Tasks;

namespace YoshiStat.Core;

public class MainController
{
    private IYoshiStatHardware _hardware;
    private IDisplayService _displayService;
    private ISensorService _sensorService;

    public MainController(IYoshiStatHardware hardware)
    {
        _hardware = hardware;
    }

    private async Task Initialize()
    {
        _displayService = new DisplayService(_hardware.Display, _hardware.DisplayRotation);
        _displayService.ShowSplashScreen();
        await Task.Delay(3000);
        _displayService.ShowDataScreen();

        _sensorService = Resolver.Services.Get<ISensorService>()
            ?? throw new System.Exception("ISensorService not registered");

        _sensorService.CurrentTemperatureChanged += OnCurrentTemperatureChanged;
        _sensorService.CurrentHumidityChanged += OnCurrentHumidityChanged;
    }

    private void OnCurrentTemperatureChanged(object sender, Meadow.Units.Temperature e)
    {
        _displayService.UpdateCurrentTemperature(e);
    }

    private void OnCurrentHumidityChanged(object sender, Meadow.Units.RelativeHumidity e)
    {
        _displayService.UpdateCurrentHumidity(e);
    }

    public async Task Run()
    {
        await Initialize();

        while (true)
        {
            await Task.Delay(1000);
        }
    }
}