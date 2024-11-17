using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using YoshiPi;
using YoshiStat.Core;

namespace YoshiStat.YoshiPi;

internal class YoshiPiHardware : IYoshiStatHardware
{
    private IYoshiPiHardware _hardware;

    public IPixelDisplay Display => _hardware.Display;

    public ITouchScreen TouchScreen => _hardware.Touchscreen;

    public RotationType DisplayRotation => RotationType._270Degrees;

    public YoshiPiHardware(IYoshiPiHardware hardware)
    {
        _hardware = hardware;

        if (_hardware.Display is IColorInvertableDisplay cid)
        {
            cid.InvertDisplayColor(true);
        }
    }
}