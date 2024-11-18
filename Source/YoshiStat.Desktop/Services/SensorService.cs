using Meadow;
using Meadow.Foundation.Sensors;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Atmospheric;
using Meadow.Units;

namespace YoshiStat.DesktopApp;

internal class SensorService : Core.ISensorService
{
    private ITemperatureSensor _temperatureSensor;

    private IHumiditySensor _humiditySensor;

    public event EventHandler<Temperature>? CurrentTemperatureChanged;

    public event EventHandler<RelativeHumidity>? CurrentHumidityChanged;

    public Temperature? CurrentTemperature => throw new NotImplementedException();

    public RelativeHumidity? CurrentHumidity => throw new NotImplementedException();

    public SensorService()
    {
        var temperatureSensor = new SimulatedTemperatureSensor(
            initialTemperature: 68.Fahrenheit(),
            minimumTemperature: 66.Fahrenheit(),
            maximumTemperature: 70.Fahrenheit());
        temperatureSensor.StartSimulation(SimulationBehavior.RandomWalk);

        _temperatureSensor = temperatureSensor;
        _temperatureSensor.Updated += TemperatureSensorUpdated;

        var humiditySensor = new SimulatedHumiditySensor();

        _humiditySensor = humiditySensor;
        _humiditySensor.Updated += HumiditySensorUpdated;
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