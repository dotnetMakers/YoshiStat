using Meadow;
using System.Threading.Tasks;

namespace YoshiStat.Core;

public class MainController
{
    private IYoshiStatHardware _hardware;
    private IDisplayService _displayService;
    private ISensorService _sensorService;
    private IOutputService _outputService;

    public MainController(IYoshiStatHardware hardware)
    {
        _hardware = hardware;
    }

    private async Task Initialize()
    {
        // create services
        _displayService = new DisplayService(
            _hardware.Display,
            _hardware.TouchScreen,
            _hardware.DisplayRotation);

        await _displayService.ShowCalibrationIfRequired();
        await _displayService.ShowSplashScreen();
        _displayService.ShowDataScreen();

        _sensorService = Resolver.Services.Get<ISensorService>()
            ?? throw new System.Exception("ISensorService not registered");

        _outputService = new OutputService(_hardware.HeatRelay, _hardware.CoolRelay);

        _sensorService.CurrentTemperatureChanged += OnCurrentTemperatureChanged;

        InitializeEvents();
    }

    private void InitializeEvents()
    {
        _hardware.TestButton1.Clicked += OnTestButton1Clicked;
        _hardware.TestButton2.Clicked += OnTestButton2Clicked;

        _displayService.TestButton1Clicked += OnTestButton1Clicked;
        _displayService.TestButton2Clicked += OnTestButton2Clicked;
    }

    private void OnTestButton1Clicked(object sender, System.EventArgs e)
    {
        _outputService.SetHeatState(true);
    }

    private void OnTestButton2Clicked(object sender, System.EventArgs e)
    {
        _outputService.SetCoolState(true);
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