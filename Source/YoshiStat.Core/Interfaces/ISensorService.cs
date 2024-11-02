using Meadow.Units;

namespace YoshiStat.Core;

public interface ISensorService
{
    public Temperature CurrentTemperature { get; }
}
