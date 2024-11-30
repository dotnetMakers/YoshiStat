using Meadow.Units;
using System;

namespace YoshiStat.Core;

public class SettingsService : ISettingsService
{
    public event EventHandler<Temperature>? SetpointChanged;

    private Temperature _currentSetpoint;

    public SettingsService()
    {
        Initialize();
    }

    private void Initialize()
    {
        _currentSetpoint = 66.Fahrenheit();
    }

    public Temperature GetCurrentSetpoint()
    {
        return _currentSetpoint;
    }

    public void SetCurrentSetpoint(Temperature setpoint)
    {
        if (setpoint == _currentSetpoint) return;

        _currentSetpoint = setpoint;
        SetpointChanged?.Invoke(this, _currentSetpoint);
    }

}
