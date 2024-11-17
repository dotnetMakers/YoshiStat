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
using YoshiStat.Core;
using static Meadow.Resolver;

namespace YoshiStat;

internal class DisplayService : IDisplayService
{
    public event EventHandler? TestButton1Clicked;
    public event EventHandler? TestButton2Clicked;

    private IPixelDisplay _display;
    private DisplayScreen _screen;

    private AbsoluteLayout _homeLayout;
    private Label _currentTempLabel;

    private Button _heatButton;
    private Button _coolButton;

    public DisplayService(IPixelDisplay display, ITouchScreen touchScreen, RotationType rotation)
    {
        _display = display;

        if (_display is IColorInvertableDisplay cid)
        {
            cid.InvertDisplayColor(true);
        }

        _screen = new DisplayScreen(
            display,
            rotation,
            touchScreen);

        if (touchScreen == null)
        {
            Log.Warn("No touch screen available");
        }
        else if (touchScreen is ICalibratableTouchscreen cts)
        {
            CheckTouchscreenCalibration(cts, _screen).Wait();
        }

        CreateLayouts();
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
    private void CreateLayouts()
    {
        _homeLayout = new AbsoluteLayout(_screen.Width, _screen.Height)
        {
            BackgroundColor = Color.Black
        };

        _currentTempLabel = new Label(
            5, 5, _homeLayout.Width - 10, 30)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Font = new Font16x24(),
            Text = "--"
        };

        _heatButton = new Button(5, _currentTempLabel.Bottom + 5, _homeLayout.Width - 10, 30)
        {
            Text = "HEAT"
        };
        _heatButton.IsEnabled = true;
        _heatButton.Clicked += OnHeatClicked;

        _coolButton = new Button(5, _heatButton.Bottom + 5, _homeLayout.Width - 10, 30)
        {
            Text = "COOL"
        };
        _coolButton.Clicked += OnCoolClicked;

        _homeLayout.Controls.Add(
            _currentTempLabel,
            _heatButton,
            _coolButton);

        _screen.Controls.Add(_homeLayout);
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
        _currentTempLabel.Text = $"{temperature.Fahrenheit:N1}";
    }

    public void UpdateSetPoint(Temperature temperature)
    {
        throw new NotImplementedException();
    }
}
