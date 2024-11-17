using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Relays;
using Meadow.Peripherals.Sensors.Buttons;
using YoshiPi;
using YoshiStat.Core;

namespace YoshiStat.YoshiPi;

internal class YoshiPiHardware : IYoshiStatHardware
{
    private IYoshiPiHardware _hardware;

    public IPixelDisplay Display => _hardware.Display;
    public ITouchScreen TouchScreen => _hardware.Touchscreen;
    public RotationType DisplayRotation => RotationType._270Degrees;

    public IRelay HeatRelay => _hardware.Relay1;
    public IRelay CoolRelay => _hardware.Relay2;
    public IButton TestButton1 => _hardware.Button1;
    public IButton TestButton2 => _hardware.Button2;

    public YoshiPiHardware(IYoshiPiHardware hardware)
    {
        _hardware = hardware;
    }
}
