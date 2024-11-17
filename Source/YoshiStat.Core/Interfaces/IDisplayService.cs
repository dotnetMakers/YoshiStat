using Meadow.Units;
using System;

namespace YoshiStat.Core;

public interface IDisplayService
{
    public event EventHandler? TestButton1Clicked;
    public event EventHandler? TestButton2Clicked;

    public void UpdateCurrentTemperature(Temperature temperature);
    public void UpdateControlState(ControlState currentState);
    public void UpdateSetPoint(Temperature temperature);
}
