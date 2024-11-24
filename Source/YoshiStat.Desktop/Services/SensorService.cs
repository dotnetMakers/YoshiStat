using Meadow;
using Meadow.Foundation.Sensors;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Atmospheric;
using Meadow.Units;

namespace YoshiStat.DesktopApp;

internal class SensorService : Core.ISensorService
{
    public event EventHandler<Temperature>? CurrentTemperatureChanged;
    public event EventHandler<RelativeHumidity>? CurrentHumidityChanged;

    private SimulatedTemperatureSensor _temperatureSensor;
    private IHumiditySensor _humiditySensor;

    public Temperature? CurrentTemperature => _temperatureSensor.Temperature;
    public RelativeHumidity? CurrentHumidity => _humiditySensor.Humidity;
    public bool SimulateTemperatureChanges { get; set; } = false;

    public SensorService(DesktopHardware hardware)
    {
        _temperatureSensor = new SimulatedTemperatureSensor(
            initialTemperature: 68.Fahrenheit(),
            minimumTemperature: 66.Fahrenheit(),
            maximumTemperature: 70.Fahrenheit());

        if (SimulateTemperatureChanges)
        {
            _temperatureSensor.StartSimulation(SimulationBehavior.RandomWalk);
        }
        else
        {
            var tempUpButton = new PushButton(
                hardware.Keyboard.Pins.Up);
            tempUpButton.Clicked += OnTempUpButtonClicked;

            var tempDownButton = new PushButton(
                hardware.Keyboard.Pins.Down);
            tempDownButton.Clicked += OnTempDownButtonClicked;
        }

        _temperatureSensor.Updated += TemperatureSensorUpdated;

        var humiditySensor = new SimulatedHumiditySensor();

        _humiditySensor = humiditySensor;
        _humiditySensor.Updated += HumiditySensorUpdated;
    }

    private void OnTempUpButtonClicked(object? sender, EventArgs e)
    {
        var current = _temperatureSensor.Temperature ?? 65.Fahrenheit();
        var newTemp = (current.Fahrenheit + 0.1).Fahrenheit();
        _temperatureSensor.Temperature = newTemp;
    }

    private void OnTempDownButtonClicked(object? sender, EventArgs e)
    {
        var current = _temperatureSensor.Temperature ?? 65.Fahrenheit();
        var newTemp = (current.Fahrenheit - 0.1).Fahrenheit();
        _temperatureSensor.Temperature = newTemp;
    }

    private void TemperatureSensorUpdated(object? sender, IChangeResult<Temperature> e)
    {
        CurrentTemperatureChanged?.Invoke(this, e.New);
    }

    private void HumiditySensorUpdated(object? sender, IChangeResult<RelativeHumidity> e)
    {
        CurrentHumidityChanged?.Invoke(this, e.New);
    }
}