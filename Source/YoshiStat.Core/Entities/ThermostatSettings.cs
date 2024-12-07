using Meadow.Units;
using System.Text.Json.Serialization;

namespace YoshiStat.Core;

public class ThermostatSettings
{
    public double SetPointF { get; set; }
    public TimeFormat TimeFormat { get; set; }
    public Temperature.UnitType DisplayUnits { get; set; }

    [JsonIgnore]
    public static ThermostatSettings Default
    {
        get
        {
            return new ThermostatSettings
            {
                SetPointF = 66,
                TimeFormat = TimeFormat.Hours_24,
                DisplayUnits = Temperature.UnitType.Fahrenheit
            };
        }
    }
}
