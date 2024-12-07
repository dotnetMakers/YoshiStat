using Meadow.Units;
using System;

namespace YoshiStat;

public interface ISettingsService
{
    event EventHandler<Temperature>? SetpointChanged;
    event EventHandler<TimeFormat>? TimeFormatChanged;
    public event EventHandler<Temperature.UnitType>? DisplayUnitsChanged;

    public Temperature GetCurrentSetpoint();
    public void SetCurrentSetpoint(Temperature setpoint);

    public TimeFormat GetCurrentTimeFormat();
    public void SetCurrentTimeFormat(TimeFormat format);

    public Temperature.UnitType GetDisplayUnits();
    public void SetDisplayUnits(Temperature.UnitType units);
}
