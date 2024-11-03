using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using YoshiPi;
using YoshiStat.Core;

namespace YoshiStat.YoshiPi;

internal class YoshiPiHardware : IYoshiStatHardware
{
    public IPixelDisplay Display { get; }

    public ITouchScreen TouchScreen { get; }

    public YoshiPiHardware(IYoshiPiHardware yoshipiHardware)
    {
        if (yoshipiHardware.Display == null)
        {
            throw new NotSupportedException();
        }

        Display = yoshipiHardware.Display;
        TouchScreen = yoshipiHardware.Touchscreen;
    }
}