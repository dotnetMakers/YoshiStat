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

    private void Initialize()
    {
        // create services
        _displayService = new DisplayService(_hardware.Display, _hardware.DisplayRotation);

        _sensorService = Resolver.Services.Get<ISensorService>()
            ?? throw new System.Exception("ISensorService not registered");

        _sensorService.CurrentTemperatureChanged += OnCurrentTemperatureChanged;
    }

    private void OnCurrentTemperatureChanged(object sender, Meadow.Units.Temperature e)
    {
        _displayService.UpdateCurrentTemperature(e);
    }

    public async Task Run()
    {
        Initialize();

        while (true)
        {
            await Task.Delay(1000);
        }
    }
}