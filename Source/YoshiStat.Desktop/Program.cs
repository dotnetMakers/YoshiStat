﻿using Meadow;

namespace YoshiStat.DesktopApp;

internal class Program
{
    private static async Task Main(string[] args)
    {
        await MeadowOS.Start(args);
    }
}