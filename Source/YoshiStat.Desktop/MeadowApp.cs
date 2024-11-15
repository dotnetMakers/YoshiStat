using Meadow;
using Meadow.Foundation.Displays;
using YoshiStat.Core;

namespace YoshiStat.DesktopApp;

internal class MeadowApp : App<Desktop>
{
    private IYoshiStatHardware _hardware;
    private MainController _mainController;

    public override Task Initialize()
    {
        Resolver.Log.Info("Creating Desktop hardware...");
        _hardware = new DesktopHardware(Device);

        //Resolver.Log.Info("Creating sensor service...");
        Resolver.Services.Create<SensorService, ISensorService>();

        Resolver.Log.Info("Creating Main Controller...");
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
