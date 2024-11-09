using Meadow;
using Meadow.Foundation.Displays;
using YoshiStat.Core;

namespace YoshiStat;

internal class YoshiStatApp : App<Desktop>
{
    private MainController _mainController;
    private DesktopHardware _hardware;

    public override Task Initialize()
    {
        // create hardware
        _hardware = new DesktopHardware(Device);

        // create sensors

        // create services
        Resolver.Services.Create<SensorService, ISensorService>();

        _mainController = new MainController(_hardware);

        return base.Initialize();
    }

    public override Task Run()
    {
        // this must be spawned in a worker because the UI needs the main thread
        _ = _mainController.Run();

        ExecutePlatformDisplayRunner();

        return base.Run();
    }

    private void ExecutePlatformDisplayRunner()
    {
        if (Device.Display is SilkDisplay silkDisplay)
        {
            silkDisplay.Run();
        }
    }
}
