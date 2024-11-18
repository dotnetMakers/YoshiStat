using Meadow.Units;
using System;

namespace YoshiStat.Core;

public interface ISensorService
{
    event EventHandler<Temperature>? CurrentTemperatureChanged;


    event EventHandler<RelativeHumidity>? CurrentHumidityChanged;

    public Temperature? CurrentTemperature { get; }

    public RelativeHumidity? CurrentHumidity { get; }
}