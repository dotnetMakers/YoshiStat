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

internal partial class DisplayService : IDisplayService
{
    public event EventHandler? TestButton1Clicked;
    public event EventHandler? TestButton2Clicked;

    private ISettingsService _settings;
    private TimeService _timeService;

    private ITouchScreen _touchScreen;
    private DisplayScreen _screen;
    private AbsoluteLayout _splashLayout;
    private AbsoluteLayout _homeLayout;

    private Label _currentHumidityLabel;
    private Label _currentTempLabel;
    private Label _timeLabel;
    private Label _stateLabel;
    private Button _showAppSettingsButton;

    private Font16x24 _font16x24;
    private IFont _pageTitleFont;

    private Temperature? _lastTemperature;

    public DisplayService(
        IPixelDisplay display,
        ITouchScreen touchScreen,
        RotationType rotation,
        ISettingsService settings,
        TimeService timeService)
    {
        _timeService = timeService;
        _settings = settings;

        _screen = new DisplayScreen(display, rotation, touchScreen);
        _touchScreen = touchScreen;

        _font16x24 = new Font16x24();
        _pageTitleFont = new Font16x24();

        _screen = new DisplayScreen(
            display,
            rotation,
            touchScreen);

        _screen.Controls.Add(_splashLayout, _homeLayout);
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

    private void LoadHomeScreen()
    {
        _homeLayout = new AbsoluteLayout(_screen.Width, _screen.Height)
        {
            IsVisible = false
        };

        _homeLayout.Controls.Add(new GradientBox(
            left: 0,
            top: 0,
            width: _homeLayout.Width,
            height: _homeLayout.Height)
        {
            StartColor = Color.FromHex("1C3242"),
            EndColor = Color.FromHex("021323")
        });

        _currentHumidityLabel = new Label(
            5,
            41,
            _homeLayout.Width - 10,
            _font16x24.Height)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            Font = _font16x24,
            Text = "55%"
        };

        _currentTempLabel = new Label(
            5,
            96,
            _homeLayout.Width - 10,
            _font16x24.Height * 2)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            Font = _font16x24,
            ScaleFactor = ScaleFactor.X2,
            Text = "-°F"
        };
        _currentTempLabel.Clicked += OnCurrentTempLabelClicked;

        _timeLabel = new Label(
            5,
            175,
            _homeLayout.Width - 10,
            30)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            Font = _font16x24,
            Text = "11:11PM"
        };

        _stateLabel = new Label(
            _homeLayout.Width - 35,
            5,
            30,
            30)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            Font = _font16x24,
            Text = "-"
        };

        _showAppSettingsButton = new Button(
            5,
            _homeLayout.Height - 35,
            100,
            30)
        {
            Text = "settings"
        };
        _showAppSettingsButton.Clicked += OnShowAppSettingsButtonClicked;

        _homeLayout.Controls.Add(_currentHumidityLabel);
        _homeLayout.Controls.Add(_currentTempLabel);
        _homeLayout.Controls.Add(_timeLabel);
        _homeLayout.Controls.Add(_stateLabel);
        _homeLayout.Controls.Add(_showAppSettingsButton);

        _screen.Controls.Add(_homeLayout);
    }

    private void OnCurrentTempLabelClicked(object sender, EventArgs e)
    {
        _ = ShowSetpointScreen(_settings.GetCurrentSetpoint());
    }

    private void OnShowAppSettingsButtonClicked(object sender, EventArgs e)
    {
        _ = ShowAppSettingsScreen();
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

        LoadAllLayouts();
    }

    private void LoadAllLayouts()
    {
        LoadSplashScreen();
        LoadHomeScreen();
        LoadSetpointScreen();
        LoadAppSettingsScreen();

        _settings.DisplayUnitsChanged += (s, e) =>
        {
            if (_lastTemperature != null)
            {
                UpdateCurrentTemperature(_lastTemperature.Value);
            }
        };
    }

    public async Task ShowSplashScreen()
    {
        _homeLayout.IsVisible = false;
        _splashLayout.IsVisible = true;
        _appSettingsLayout.IsVisible = false;
        _setpointLayout.IsVisible = false;

        await Task.Delay(3000);
    }

    public void ShowHomeScreen()
    {
        _splashLayout.IsVisible = false;
        _homeLayout.IsVisible = true;
        _setpointLayout.IsVisible = false;
        _appSettingsLayout.IsVisible = false;
        UpdateTime();
    }

    public void UpdateTime()
    {
        _timeLabel.Text = _timeService.GetFormattedLocalTime();
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
        _stateLabel.Text = currentState switch
        {
            ControlState.Heating => "H",
            ControlState.Cooling => "C",
            _ => "-"
        };
    }

    public void UpdateCurrentTemperature(Temperature temperature)
    {
        _lastTemperature = temperature;

        switch (_settings.GetDisplayUnits())
        {
            case Temperature.UnitType.Fahrenheit:
                _currentTempLabel.Text = $"{temperature.Fahrenheit:N1}°F";
                break;
            default:
                _currentTempLabel.Text = $"{temperature.Celsius:N1}°C";
                break;
        }
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