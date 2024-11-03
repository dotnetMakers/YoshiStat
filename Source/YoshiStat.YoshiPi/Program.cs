using Meadow;

namespace YoshiStat.YoshiPi;

internal class Program
{
    private static async Task Main(string[] args)
    {
        await MeadowOS.Start(args);
    }
}