using Meadow.Units;

namespace YoshiStat.Core;

public interface IDisplayService
{
    public void ShowSplashScreen();

    public void ShowDataScreen();

    public void UpdateCurrentTemperature(Temperature temperature);

    public void UpdateControlState(ControlState currentState);

    public void UpdateSetPoint(Temperature temperature);
}
