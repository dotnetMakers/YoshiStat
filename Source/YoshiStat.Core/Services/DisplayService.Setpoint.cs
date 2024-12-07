using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Units;
using System.Threading.Tasks;

namespace YoshiStat.Core;

internal partial class DisplayService : IDisplayService
{
    private AbsoluteLayout _setpointLayout;
    private Button _setpointCancelButton;
    private Button _setpointApplyButton;
    private Label _currentSetpointLabel;
    private Button _incrementSetpointButton;
    private Button _decrementSetpointButton;
    private Temperature _currentSetpoint;
    private Temperature _temporarySetpoint;

    private void LoadSetpointScreen()
    {
        _setpointLayout = new AbsoluteLayout(_screen.Width, _screen.Height)
        {
            IsVisible = false
        };

        var setpointTitle = new Label(0, 0, _setpointLayout.Width, 30)
        {
            Text = "Set Point",
            HorizontalAlignment = HorizontalAlignment.Center
        };

        _currentSetpointLabel = new Label(
            5,
            96,
            200,
            _font16x24.Height * 2)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            Font = _font16x24,
            ScaleFactor = ScaleFactor.X2,
            Text = $"{_currentSetpoint.Fahrenheit:N0}°F"
        };

        _incrementSetpointButton = new Button(
            _setpointLayout.Right - 35,
            _currentSetpointLabel.Top,
            30,
            20
            )
        {
            Text = "+"
        };
        _incrementSetpointButton.Clicked += OnIncrementSetpointButtonClicked;

        _decrementSetpointButton = new Button(
            _setpointLayout.Right - 35,
            _currentSetpointLabel.Bottom - 20,
            30,
            20
            )
        {
            Text = "-"
        };
        _decrementSetpointButton.Clicked += OnDecrementSetpointButtonClicked;

        _setpointCancelButton = new Button(
            5,
            _setpointLayout.Height - 35,
            100,
            30)
        {
            Text = "cancel"
        };
        _setpointCancelButton.Clicked += OnSetpointCancelButtonClicked; ;

        _setpointApplyButton = new Button(
            _setpointLayout.Right - 105,
            _setpointLayout.Height - 35,
            100,
            30)
        {
            Text = "apply"
        };
        _setpointApplyButton.Clicked += OnSetpointApplyButtonClicked;

        _setpointLayout.Controls.Add(
            setpointTitle,
            _currentSetpointLabel,
            _incrementSetpointButton,
            _decrementSetpointButton,
            _setpointCancelButton,
            _setpointApplyButton);

        _screen.Controls.Add(_setpointLayout);
    }

    private void OnDecrementSetpointButtonClicked(object sender, System.EventArgs e)
    {
        _temporarySetpoint = (_temporarySetpoint.Fahrenheit - 1).Fahrenheit();
        UpdateSetpointLabel();
    }

    private void OnIncrementSetpointButtonClicked(object sender, System.EventArgs e)
    {
        _temporarySetpoint = (_temporarySetpoint.Fahrenheit + 1).Fahrenheit();
        UpdateSetpointLabel();
    }

    private void UpdateSetpointLabel()
    {
        _currentSetpointLabel.Text = $"{_temporarySetpoint.Fahrenheit:N0}°F";
    }

    private void OnSetpointApplyButtonClicked(object sender, System.EventArgs e)
    {
        _settings.SetCurrentSetpoint(_temporarySetpoint);
        ShowHomeScreen();
    }

    private void OnSetpointCancelButtonClicked(object sender, System.EventArgs e)
    {
        ShowHomeScreen();
    }

    public async Task ShowSetpointScreen(Temperature currentSetpoint)
    {
        _temporarySetpoint = currentSetpoint;
        UpdateSetpointLabel();

        _homeLayout.IsVisible = false;
        _splashLayout.IsVisible = false;
        _appSettingsLayout.IsVisible = false;
        _setpointLayout.IsVisible = true;
    }
}