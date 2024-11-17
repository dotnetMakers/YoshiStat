using Meadow.Hardware;
using Meadow.Peripherals.Displays;

namespace YoshiStat.Core;

public interface IYoshiStatHardware
{
    IPixelDisplay Display { get; }

    ITouchScreen TouchScreen { get; }

    RotationType DisplayRotation { get; }
}