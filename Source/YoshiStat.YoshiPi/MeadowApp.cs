using YoshiPi;
using YoshiStat.Core;

namespace YoshiStat.YoshiPi;

internal class MeadowApp : YoshiPiApp
{
    private MainController _mainController;
    private YoshiPiHardware _hardware;

    public override Task Initialize()
    {
        // create hardware
        _hardware = new YoshiPiHardware(Hardware);

        // create sensors

        // create services

        _mainController = new MainController(_hardware);

        return base.Initialize();
    }

    public override Task Run()
    {

        return base.Run();
    }
}