using Meadow.Units;
using System;
using System.Threading.Tasks;

namespace YoshiStat.Core;

public interface IDisplayService
{
    public event EventHandler? TestButton1Clicked;

    public event EventHandler? TestButton2Clicked;

    public Task ShowCalibrationIfRequired();

    public Task ShowSplashScreen();

    public void ShowDataScreen();

    public void UpdateCurrentTemperature(Temperature temperature);

    public void UpdateCurrentHumidity(RelativeHumidity humidity);

    public void UpdateControlState(ControlState currentState);

    public void UpdateSetPoint(Temperature temperature);
}
