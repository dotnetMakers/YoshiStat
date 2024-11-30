using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using System.Threading.Tasks;

namespace YoshiStat.Core;

internal partial class DisplayService : IDisplayService
{
    private AbsoluteLayout _appSettingsLayout;
    private Button _settingsCancelButton;
    private Button _settingsApplyButton;

    private void LoadAppSettingsScreen()
    {
        _appSettingsLayout = new AbsoluteLayout(_screen.Width, _screen.Height)
        {
            IsVisible = false
        };

        var settingsTitle = new Label(0, 0, _appSettingsLayout.Width, 30)
        {
            Text = "Settings",
            HorizontalAlignment = HorizontalAlignment.Center
        };

        _settingsCancelButton = new Button(
            5,
            _appSettingsLayout.Height - 35,
            100,
            30)
        {
            Text = "cancel"
        };
        _settingsCancelButton.Clicked += OnSettingsCancelButtonClicked;

        _settingsApplyButton = new Button(
            _appSettingsLayout.Right - 105,
            _appSettingsLayout.Height - 35,
            100,
            30)
        {
            Text = "apply"
        };
        _settingsApplyButton.Clicked += OnSettingsApplyButtonClicked;

        _appSettingsLayout.Controls.Add(
            settingsTitle,
            _settingsCancelButton,
            _settingsApplyButton);

        _screen.Controls.Add(_appSettingsLayout);
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