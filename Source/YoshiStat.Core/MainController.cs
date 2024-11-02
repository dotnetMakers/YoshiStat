using System.Threading.Tasks;

namespace YoshiStat.Core;

public class MainController
{
    private IYoshiStatHardware _hardware;

    public MainController(IYoshiStatHardware hardware)
    {
        _hardware = hardware;
    }

    private void Initialize()
    {
        // create services
        var displayService = new DisplayService(_hardware.Display);
    }

    public async Task Run()
    {
        Initialize();

        while (true)
        {
            await Task.Delay(1000);
        }
    }
}