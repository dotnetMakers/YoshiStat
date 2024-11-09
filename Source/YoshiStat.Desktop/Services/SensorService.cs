using Meadow.Foundation.Sensors;
using Meadow.Peripherals.Sensors;
using Meadow.Units;
using ISensorService = YoshiStat.Core.ISensorService;

namespace YoshiStat;

internal class SensorService : ISensorService
{
    public event EventHandler<Temperature>? CurrentTemperatureChanged;

    private SimulatedTemperatureSensor _tempSensor;

    public SensorService()
    {
        _tempSensor = new SimulatedTemperatureSensor(
            initialTemperature: 68.Fahrenheit(),
            minimumTemperature: 66.Fahrenheit(),
            maximumTemperature: 70.Fahrenheit());

        _tempSensor.StartSimulation(SimulationBehavior.RandomWalk);
        _tempSensor.Updated += OnTempSensorUpdated;
    }

    private void OnTempSensorUpdated(object? sender, Meadow.IChangeResult<Temperature> e)
    {
        CurrentTemperatureChanged?.Invoke(this, e.New);
    }

    public Temperature? CurrentTemperature => _tempSensor.Temperature;
}
