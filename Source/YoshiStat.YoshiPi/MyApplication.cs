using Meadow;
using YoshiPi;

namespace YoshiStat.YoshiPi;
internal sealed class MyApplication : YoshiPiApp
{
    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize");

        Resolver.Services.Add(
            new DisplayService(Hardware.Display, Hardware.Touchscreen)
            );

        return base.Initialize();
    }

    public override Task Run()
    {
        Resolver.Log.Info("Run");

        var display = Resolver.Services.Get<DisplayService>();

        display?.SetLabelText("Hello YoshiPi!");

        return base.Run();
    }

    public static async Task Main(string[] args)
    {
        await MeadowOS.Start(args);
    }
}
