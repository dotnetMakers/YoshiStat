using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Relays;
using Meadow.Peripherals.Sensors.Buttons;

namespace YoshiStat.Core;

public interface IYoshiStatHardware
{
    IPixelDisplay Display { get; }

    ITouchScreen TouchScreen { get; }

    RotationType DisplayRotation { get; }

    IRelay HeatRelay { get; }

    IRelay CoolRelay { get; }

    IButton TestButton1 { get; }

    IButton TestButton2 { get; }
}