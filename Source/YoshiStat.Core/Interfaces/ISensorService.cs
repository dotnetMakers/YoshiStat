using Meadow.Units;
using System;

namespace YoshiStat.Core;

public interface ISensorService
{
    event EventHandler<Temperature>? CurrentTemperatureChanged;

    public Temperature? CurrentTemperature { get; }
}
