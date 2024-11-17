using Meadow;
using Meadow.Foundation.Relays;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Foundation.Sensors.Hid;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Relays;
using Meadow.Peripherals.Sensors.Buttons;
using YoshiStat.Core;

namespace YoshiStat.DesktopApp;

internal class DesktopHardware : IYoshiStatHardware
{
    private Keyboard _keyboard;

    public IPixelDisplay Display { get; }

    public ITouchScreen TouchScreen { get; }
    public RotationType DisplayRotation => RotationType.Normal;

    public IRelay HeatRelay { get; }
    public IRelay CoolRelay { get; }
    public IButton TestButton1 { get; }
    public IButton TestButton2 { get; }

    public DesktopHardware(Desktop desktop)
    {
        if (desktop.Display == null)
        {
            throw new NotSupportedException();
        }

        desktop.Display.Resize(320, 240, 3);

        HeatRelay = new SimulatedRelay("heat");
        HeatRelay.OnChanged += OnRelayChanged;

        CoolRelay = new SimulatedRelay("cool");
        CoolRelay.OnChanged += OnRelayChanged;

        _keyboard = new Keyboard();

        TestButton1 = new PushButton(
            _keyboard.Pins.H);
        TestButton2 = new PushButton(
            _keyboard.Pins.C);

        Display = desktop.Display;
        TouchScreen = desktop.Display as ITouchScreen;
    }

    private void OnRelayChanged(object? sender, RelayState e)
    {
        var relay = sender as SimulatedRelay;
        Console.WriteLine($"{relay?.Name ?? "unknown"} relay is {e}");
    }
}