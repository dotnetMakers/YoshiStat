using Meadow.Foundation.Sensors;
using Meadow.Peripherals.Sensors;
using Meadow.Units;
using ISensorService = YoshiStat.Core.ISensorService;

namespace YoshiStat.YoshiPi;

internal class SensorService : ISensorService
{
    public event EventHandler<Temperature>? CurrentTemperatureChanged;

    public Temperature? CurrentTemperature => throw new NotImplementedException();

    private ITemperatureSensor _tempSensor;

    public SensorService()
    {
        var tempSensor = new SimulatedTemperatureSensor(
            initialTemperature: 68.Fahrenheit(),
            minimumTemperature: 66.Fahrenheit(),
            maximumTemperature: 70.Fahrenheit());

        tempSensor.StartSimulation(SimulationBehavior.RandomWalk);

        _tempSensor = tempSensor;
        _tempSensor.Updated += OnTempSensorUpdated;
    }

    private void OnTempSensorUpdated(object? sender, Meadow.IChangeResult<Temperature> e)
    {
        CurrentTemperatureChanged?.Invoke(this, e.New);
    }
}