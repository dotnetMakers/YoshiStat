using Meadow;
using YoshiPi;
using YoshiStat.Core;

namespace YoshiStat.YoshiPi;

internal class MeadowApp : YoshiPiApp
{
    private MainController _mainController;
    private IYoshiStatHardware _hardware;

    public override Task Initialize()
    {
        Resolver.Log.Info("Creating sensor service...");
        Resolver.Services.Create<SensorService, Core.ISensorService>();

        Resolver.Log.Info("Creating YoshiPi hardware...");
        _hardware = new YoshiPiHardware(Hardware);

        Resolver.Log.Info("Creating Main Controller...");
        _mainController = new MainController(_hardware);

        return base.Initialize();
    }

    public override Task Run()
    {
        Resolver.Log.Info("Run");

        return _mainController.Run();
    }
}