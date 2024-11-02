using Meadow;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using YoshiStat.Core;

namespace YoshiStat;

internal class DesktopHardware : IYoshiStatHardware
{
    public IPixelDisplay Display { get; }
    public ITouchScreen TouchScreen { get; }

    public DesktopHardware(Desktop desktop)
    {
        if (desktop.Display == null) throw new NotSupportedException();

        desktop.Display.Resize(320, 240, 3);

        Display = desktop.Display;
        TouchScreen = desktop.Display as ITouchScreen;
    }


}
