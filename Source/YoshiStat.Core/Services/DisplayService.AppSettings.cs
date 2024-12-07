using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Units;
using System.Threading.Tasks;

namespace YoshiStat.Core;

internal partial class DisplayService : IDisplayService
{
    private AbsoluteLayout _appSettingsLayout;
    private Button _settingsCancelButton;
    private Button _settingsApplyButton;

    private IFont _settingsItemFont;

    private Label _timeFormatLabel;
    private Button _timeFormatButton;
    private Label _displayUnitsLabel;
    private Button _displayUnitsButton;

    private void LoadAppSettingsScreen()
    {
        _settingsItemFont = new Font12x16();

        _appSettingsLayout = new AbsoluteLayout(_screen.Width, _screen.Height)
        {
            IsVisible = false
        };

        _appSettingsLayout.Controls.Add(new GradientBox(
            left: 0,
            top: 0,
            width: _appSettingsLayout.Width,
            height: _appSettingsLayout.Height)
        {
            StartColor = Color.FromHex("1C3242"),
            EndColor = Color.FromHex("021323")
        });

        var settingsTitle = new Label(0, 0, _appSettingsLayout.Width, 30)
        {
            Text = "Settings",
            HorizontalAlignment = HorizontalAlignment.Center,
            Font = _pageTitleFont
        };

        _timeFormatLabel = new Label(
            5,
            settingsTitle.Bottom + 5,
            250,
            30)
        {
            Font = _settingsItemFont,
            Text = "clock format"
        };
        _timeFormatButton = new Button(
            _appSettingsLayout.Right - 45,
            _timeFormatLabel.Top,
            30,
            30
            )
        {
            Text = "<>"
        };
        _timeFormatButton.Clicked += OnTimeFormatButtonClicked;

        _displayUnitsLabel = new Label(
            5,
            _timeFormatLabel.Bottom + 5,
            250,
            30)
        {
            Font = _settingsItemFont,
            Text = "display units"
        };
        _displayUnitsButton = new Button(
            _appSettingsLayout.Right - 45,
            _displayUnitsLabel.Top,
            30,
            30
            )
        {
            Text = "<>"
        };
        _displayUnitsButton.Clicked += OnDisplayUnitsButtonClicked;



        _settingsCancelButton = new Button(
            5,
            _appSettingsLayout.Height - 35,
            100,
            30)
        {
            Text = "cancel",
        };
        _settingsCancelButton.Clicked += OnSettingsCancelButtonClicked;

        _settingsApplyButton = new Button(
            _appSettingsLayout.Right - 105,
            _appSettingsLayout.Height - 35,
            100,
            30)
        {
            Text = "ok"
        };
        _settingsApplyButton.Clicked += OnSettingsApplyButtonClicked;

        _appSettingsLayout.Controls.Add(
            settingsTitle,
            _settingsApplyButton,
            _timeFormatLabel,
            _timeFormatButton,
            _displayUnitsLabel,
            _displayUnitsButton);

        _screen.Controls.Add(_appSettingsLayout);

        _settings.TimeFormatChanged += (s, e) => UpdateSettingsLabels();
        _settings.DisplayUnitsChanged += (s, e) => UpdateSettingsLabels();

        UpdateSettingsLabels();
    }

    private void UpdateSettingsLabels()
    {
        _timeFormatLabel.Text = _settings.GetCurrentTimeFormat() switch
        {
            TimeFormat.Hours_12 => "Show 12-Hour Time",
            _ => "Show 24-hour Time"
        };
        _displayUnitsLabel.Text = _settings.GetDisplayUnits() switch
        {
            Meadow.Units.Temperature.UnitType.Celsius => "Show Temp in C",
            _ => "Show Temp in F"
        };
    }

    private void OnDisplayUnitsButtonClicked(object sender, System.EventArgs e)
    {
        var changeSetting = _settings.GetDisplayUnits() switch
        {
            Meadow.Units.Temperature.UnitType.Celsius => Temperature.UnitType.Fahrenheit,
            _ => Temperature.UnitType.Celsius
        };

        _settings.SetDisplayUnits(changeSetting);
    }

    private void OnTimeFormatButtonClicked(object sender, System.EventArgs e)
    {
        var changeSetting = _settings.GetCurrentTimeFormat() switch
        {
            TimeFormat.Hours_12 => TimeFormat.Hours_24,
            _ => TimeFormat.Hours_12
        };

        _settings.SetCurrentTimeFormat(changeSetting);
    }

    private void OnSettingsApplyButtonClicked(object sender, System.EventArgs e)
    {
        ShowHomeScreen();
    }

    private void OnSettingsCancelButtonClicked(object sender, System.EventArgs e)
    {
        ShowHomeScreen();
    }

    public async Task ShowAppSettingsScreen()
    {
        _homeLayout.IsVisible = false;
        _splashLayout.IsVisible = false;
        _setpointLayout.IsVisible = false;
        _appSettingsLayout.IsVisible = true;
    }
}