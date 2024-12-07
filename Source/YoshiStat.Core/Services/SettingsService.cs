using Meadow.Units;
using System;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace YoshiStat.Core;

public class SettingsService : ISettingsService
{
    public event EventHandler<Temperature>? SetpointChanged;
    public event EventHandler<TimeFormat>? TimeFormatChanged;
    public event EventHandler<Temperature.UnitType>? DisplayUnitsChanged;

    private readonly string _settingsPath;
    private const string SettingsFileName = "settings.json";

    private ThermostatSettings _settings;


    public SettingsService()
    {
        _settingsPath =
            Path.Combine(
                Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                SettingsFileName);

        Initialize();
    }

    private void Load()
    {
        try
        {
            if (!File.Exists(_settingsPath))
            {
                _settings = ThermostatSettings.Default;
                Save();
                return;
            }

            var json = File.ReadAllText(_settingsPath);

            _settings = JsonSerializer.Deserialize<ThermostatSettings>(json)
                ?? ThermostatSettings.Default;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            _settings = ThermostatSettings.Default;
        }
    }

    private void Save()
    {
        try
        {
            var json = JsonSerializer.Serialize(_settings);
            File.WriteAllText(_settingsPath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void Initialize()
    {
        Load();
    }

    public Temperature GetCurrentSetpoint()
    {
        return _settings.SetPointF.Fahrenheit();
    }

    public void SetCurrentSetpoint(Temperature setpoint)
    {
        if (setpoint.Fahrenheit == _settings.SetPointF) return;

        _settings.SetPointF = setpoint.Fahrenheit;
        Save();
        SetpointChanged?.Invoke(this, setpoint);
    }

    public TimeFormat GetCurrentTimeFormat()
    {
        return _settings.TimeFormat;
    }

    public void SetCurrentTimeFormat(TimeFormat format)
    {
        if (format == _settings.TimeFormat) return;

        _settings.TimeFormat = format;
        Save();
        TimeFormatChanged?.Invoke(this, format);
    }


    public Temperature.UnitType GetDisplayUnits()
    {
        return _settings.DisplayUnits;
    }

    public void SetDisplayUnits(Temperature.UnitType units)
    {
        if (units == _settings.DisplayUnits) return;

        _settings.DisplayUnits = units;
        Save();
        DisplayUnitsChanged?.Invoke(this, units);
    }

}
