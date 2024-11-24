﻿using Meadow;
using Meadow.Units;
using System;
using System.Threading.Tasks;

namespace YoshiStat.Core;

public class MainController
{
    private IYoshiStatHardware _hardware;
    private IDisplayService _displayService;
    private ISensorService _sensorService;
    private IOutputService _outputService;
    private ControlState _currentState;
    public DateTimeOffset? _lastHeatTime;
    public DateTimeOffset? _lastCoolTime;

    // TODO: initialize from config
    public TimeSpan ChangeoverTime { get; set; } = TimeSpan.FromHours(2);
    public Temperature SetPoint { get; set; } = 66.Fahrenheit();
    public Temperature Deadband { get; set; } = 1.Fahrenheit();

    public MainController(IYoshiStatHardware hardware)
    {
        _hardware = hardware;
    }

    private async Task Initialize()
    {
        // create services
        _displayService = new DisplayService(
            _hardware.Display,
            _hardware.TouchScreen,
            _hardware.DisplayRotation);

        await _displayService.ShowCalibrationIfRequired();
        await _displayService.ShowSplashScreen();
        _displayService.ShowDataScreen();

        _sensorService = Resolver.Services.Get<ISensorService>()
            ?? throw new System.Exception("ISensorService not registered");

        _outputService = new OutputService(_hardware.HeatRelay, _hardware.CoolRelay);

        _sensorService.CurrentTemperatureChanged += OnCurrentTemperatureChanged;

        if (_sensorService.CurrentTemperature != null)
        {
            _displayService.UpdateCurrentTemperature(_sensorService.CurrentTemperature.Value);
        }

        InitializeEvents();
    }

    private void InitializeEvents()
    {
        _hardware.TestButton1.Clicked += OnTestButton1Clicked;
        _hardware.TestButton2.Clicked += OnTestButton2Clicked;

        _displayService.TestButton1Clicked += OnTestButton1Clicked;
        _displayService.TestButton2Clicked += OnTestButton2Clicked;
    }

    private void OnTestButton1Clicked(object sender, System.EventArgs e)
    {
        _outputService.SetHeatState(true);
    }

    private void OnTestButton2Clicked(object sender, System.EventArgs e)
    {
        _outputService.SetCoolState(true);
        _sensorService.CurrentHumidityChanged += OnCurrentHumidityChanged;
    }

    private void OnCurrentTemperatureChanged(object sender, Meadow.Units.Temperature e)
    {
        _displayService.UpdateCurrentTemperature(e);
    }

    private void OnCurrentHumidityChanged(object sender, Meadow.Units.RelativeHumidity e)
    {
        _displayService.UpdateCurrentHumidity(e);
    }

    public ControlState CurrentControlState
    {
        get => _currentState;
        private set
        {
            _currentState = value;
            _displayService.UpdateControlState(CurrentControlState);
            switch (CurrentControlState)
            {
                case ControlState.Heating:
                    _outputService.SetHeatState(true);
                    break;
                case ControlState.Cooling:
                    _outputService.SetCoolState(true);
                    break;
                default:
                    _outputService.SetHeatState(false);
                    _outputService.SetCoolState(false);
                    break;

            }
        }
    }

    public async Task Run()
    {
        await Initialize();

        var i = 0;

        while (true)
        {
            await Task.Delay(1000);

            // state control algorithm
            var currentTemp = _sensorService.CurrentTemperature;
            if (currentTemp == null)
            {
                continue;
            }

            switch (CurrentControlState)
            {
                case ControlState.Idle:
                    if (currentTemp < SetPoint)
                    {
                        if (_lastCoolTime == null ||
                            _lastCoolTime < DateTimeOffset.UtcNow - ChangeoverTime)
                        {
                            CurrentControlState = ControlState.Heating;
                        }
                    }
                    else if (currentTemp > SetPoint)
                    {
                        if (_lastHeatTime == null ||
                            _lastHeatTime < DateTimeOffset.UtcNow - ChangeoverTime)
                        {
                            CurrentControlState = ControlState.Cooling;
                        }
                    }
                    break;
                case ControlState.Heating:
                    if (currentTemp.Value.Fahrenheit >= SetPoint.Fahrenheit + Deadband.Fahrenheit)
                    {
                        CurrentControlState = ControlState.Idle;
                        _lastHeatTime = DateTimeOffset.UtcNow;
                    }
                    break;
                case ControlState.Cooling:
                    if (currentTemp.Value.Fahrenheit <= SetPoint.Fahrenheit - Deadband.Fahrenheit)
                    {
                        CurrentControlState = ControlState.Idle;
                        _lastCoolTime = DateTimeOffset.UtcNow;
                    }
                    break;

            }

        }
    }
}