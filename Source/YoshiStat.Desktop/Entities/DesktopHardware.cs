using Meadow;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using YoshiStat.Core;

namespace YoshiStat.DesktopApp;

internal class DesktopHardware : IYoshiStatHardware
{
    public IPixelDisplay Display { get; }

    public ITouchScreen TouchScreen { get; }
    public RotationType DisplayRotation => RotationType.Normal;

    public DesktopHardware(Desktop desktop)
    {
        if (desktop.Display == null)
        {
            throw new NotSupportedException();
        }

        desktop.Display.Resize(320, 240, 2);

        Display = desktop.Display;
        TouchScreen = desktop.Display as ITouchScreen;
    }
}