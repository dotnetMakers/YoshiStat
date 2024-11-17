using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;
using Meadow.Units;
using System;

namespace YoshiStat.Core;

internal class DisplayService : IDisplayService
{
    private DisplayScreen _screen;
    private AbsoluteLayout _splashLayout;
    private AbsoluteLayout _dataLayout;

    private Label _currentHumidityLabel;
    private Label _currentTempLabel;
    private Label _timeLabel;

    private Font16x24 font16x24;

    public DisplayService(IPixelDisplay display, RotationType rotation)
    {
        _screen = new DisplayScreen(display, rotation);

        font16x24 = new Font16x24();

        LoadSplashScreen();

        LoadDataScreen();

        _screen.Controls.Add(_splashLayout, _dataLayout);
    }

    private void LoadSplashScreen()
    {
        _splashLayout = new AbsoluteLayout(_screen.Width, _screen.Height)
        {
            IsVisible = false
        };

        var image = Image.LoadFromResource("YoshiStat.Core.Resources.splash.bmp");
        var displayImage = new Picture(0, 0, _screen.Width, _screen.Height, image)
        {
            BackColor = Color.FromHex("F39E6C"),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };

        _splashLayout.Controls.Add(displayImage);

        _splashLayout.Controls.Add(new Label(110, 203, 100, 16)
        {
            Text = "Version 0.1.0",
            TextColor = Color.White,
            Font = new Font12x16(),
            HorizontalAlignment = HorizontalAlignment.Center
        });

        _screen.Controls.Add(_splashLayout);
    }

    private void LoadDataScreen()
    {
        _dataLayout = new AbsoluteLayout(_screen.Width, _screen.Height)
        {
            IsVisible = false
        };

        _dataLayout.Controls.Add(new GradientBox(
            left: 0,
            top: 0,
            width: _screen.Width,
            height: _screen.Height)
        {
            StartColor = Color.FromHex("1C3242"),
            EndColor = Color.FromHex("021323")
        });

        _currentHumidityLabel = new Label(
            5,
            41,
            _dataLayout.Width - 10,
            font16x24.Height)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            Font = font16x24,
            Text = "75%"
        };
        _dataLayout.Controls.Add(_currentHumidityLabel);

        _currentTempLabel = new Label(
            5,
            96,
            _dataLayout.Width - 10,
            font16x24.Height * 2)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            Font = font16x24,
            ScaleFactor = ScaleFactor.X2,
            Text = "-°C"
        };
        _dataLayout.Controls.Add(_currentTempLabel);

        _timeLabel = new Label(
            5,
            175,
            _dataLayout.Width - 10,
            30)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            Font = font16x24,
            Text = "11:11PM"
        };
        _dataLayout.Controls.Add(_timeLabel);

        _screen.Controls.Add(_dataLayout);
    }

    public void ShowSplashScreen()
    {
        _dataLayout.IsVisible = false;
        _splashLayout.IsVisible = true;
    }

    public void ShowDataScreen()
    {
        _splashLayout.IsVisible = false;
        _dataLayout.IsVisible = true;
    }

    public void UpdateControlState(ControlState currentState)
    {
        throw new NotImplementedException();
    }

    public void UpdateCurrentTemperature(Temperature temperature)
    {
        _currentTempLabel.Text = $"{temperature.Fahrenheit:N0}°C";
    }

    public void UpdateSetPoint(Temperature temperature)
    {
        throw new NotImplementedException();
    }
}
