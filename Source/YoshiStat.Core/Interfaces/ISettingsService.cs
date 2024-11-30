using Meadow.Units;
using System;

namespace YoshiStat;

public interface ISettingsService
{
    event EventHandler<Temperature>? SetpointChanged;

    public Temperature GetCurrentSetpoint();
    public void SetCurrentSetpoint(Temperature setpoint);
}
