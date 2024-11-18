using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Foundation.Hmi;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Units;
using System;
using System.IO;
using System.Threading.Tasks;
using static Meadow.Resolver;

namespace YoshiStat.Core;

internal class DisplayService : IDisplayService
{
    public event EventHandler? TestButton1Clicked;
    public event EventHandler? TestButton2Clicked;

    private ITouchScreen _touchScreen;
    private DisplayScreen _screen;
    private AbsoluteLayout _splashLayout;
    private AbsoluteLayout _dataLayout;

    private Label _currentHumidityLabel;
    private Label _currentTempLabel;
    private Label _timeLabel;

    private Font16x24 font16x24;

    public DisplayService(IPixelDisplay display, ITouchScreen touchScreen, RotationType rotation)
    {
        _screen = new DisplayScreen(display, rotation, touchScreen);
        _touchScreen = touchScreen;

        font16x24 = new Font16x24();

        _screen = new DisplayScreen(
            display,
            rotation,
            touchScreen);

        _screen.Controls.Add(_splashLayout, _dataLayout);
    }

    private async Task CheckTouchscreenCalibration(ICalibratableTouchscreen touchscreen, DisplayScreen screen)
    {
        var calfile = new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ts.cal"));

        Log.Info($"Using calibration data at {calfile.FullName}");

        var cal = new TouchscreenCalibrationService(screen, calfile);

        var existing = cal.GetSavedCalibrationData();

        if (existing != null)
        {
            touchscreen.SetCalibrationData(existing);
        }
        else
        {
            await cal.Calibrate(true);
        }
    }

    private void LoadSplashScreen()
    {
        _splashLayout = new AbsoluteLayout(_screen.Width, _screen.Height)
        {
            IsVisible = false
        };

        var image = Image.LoadFromResource("YoshiStat.Core.Resources.splash.bmp");
        var displayImage = new Picture(0, 0, _screen.Width, _screen.Height, image);

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
            Text = "55%"
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
            Text = "-°F"
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

    public async Task ShowCalibrationIfRequired()
    {
        if (_touchScreen == null)
        {
            Log.Warn("No touch screen available");
        }
        else if (_touchScreen is ICalibratableTouchscreen cts)
        {
            await CheckTouchscreenCalibration(cts, _screen);
        }

        LoadSplashScreen();
        LoadDataScreen();
    }

    public async Task ShowSplashScreen()
    {
        _dataLayout.IsVisible = false;
        _splashLayout.IsVisible = true;

        await Task.Delay(3000);
    }

    public void ShowDataScreen()
    {
        _splashLayout.IsVisible = false;
        _dataLayout.IsVisible = true;
    }

    public void UpdateTime()
    {
        _timeLabel.Text = DateTime.Now.ToString("hh:mm tt");
    }

    private void OnCoolClicked(object sender, EventArgs e)
    {
        TestButton2Clicked?.Invoke(this, e);
    }

    private void OnHeatClicked(object sender, EventArgs e)
    {
        TestButton1Clicked?.Invoke(this, e);
    }

    public void UpdateControlState(ControlState currentState)
    {
        throw new NotImplementedException();
    }

    public void UpdateCurrentTemperature(Temperature temperature)
    {
        _currentTempLabel.Text = $"{temperature.Fahrenheit:N0}°F";
    }

    public void UpdateCurrentHumidity(RelativeHumidity humidity)
    {
        _currentHumidityLabel.Text = $"{humidity.Percent:N0}%";
    }

    public void UpdateSetPoint(Temperature temperature)
    {
        throw new NotImplementedException();
    }
}