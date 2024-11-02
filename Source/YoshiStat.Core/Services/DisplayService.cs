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

    public DisplayService(IPixelDisplay display)
    {
        _display = display;
        _screen = new DisplayScreen(
            display);

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
            Text = "75"
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
        throw new NotImplementedException();
    }

    public void UpdateSetPoint(Temperature temperature)
    {
        throw new NotImplementedException();
    }
}
