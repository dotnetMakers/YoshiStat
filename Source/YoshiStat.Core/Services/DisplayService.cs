using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;
using Meadow.Units;
using System;
using YoshiStat.Core;

namespace YoshiStat;

internal class DisplayService : IDisplayService
{
    private IPixelDisplay _display;
    private DisplayScreen _screen;

    private AbsoluteLayout _homeLayout;
    private Label _currentTempLabel;

    public DisplayService(IPixelDisplay display, RotationType rotation)
    {
        _display = display;

        if (_display is IColorInvertableDisplay cid)
        {
            cid.InvertDisplayColor(true);
        }

        _screen = new DisplayScreen(
            display,
            rotation);

        CreateLayouts();
    }

    private void CreateLayouts()
    {
        _homeLayout = new AbsoluteLayout(_screen.Width, _screen.Height);

        _currentTempLabel = new Label(
            5, 5, _homeLayout.Width - 10, 30)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Font = new Font16x24(),
            Text = "--"
        };

        _homeLayout.Controls.Add(_currentTempLabel);

        _screen.Controls.Add(_homeLayout);
    }

    public void UpdateControlState(ControlState currentState)
    {
        throw new NotImplementedException();
    }

    public void UpdateCurrentTemperature(Temperature temperature)
    {
        _currentTempLabel.Text = $"{temperature.Fahrenheit:N1}";
    }

    public void UpdateSetPoint(Temperature temperature)
    {
        throw new NotImplementedException();
    }
}
