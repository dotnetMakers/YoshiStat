using Meadow.Peripherals.Relays;
using YoshiStat.Core;

namespace YoshiStat;

internal class OutputService : IOutputService
{
    private IRelay _heatRelay;
    private IRelay _coolRelay;

    public OutputService(IRelay heatRelay, IRelay coolRelay)
    {
        _heatRelay = heatRelay;
        _coolRelay = coolRelay;
    }

    public void SetCoolState(bool isOn)
    {
        if (isOn) { _heatRelay.State = RelayState.Open; }

        _coolRelay.State = isOn ? RelayState.Closed : RelayState.Open;
    }

    public void SetHeatState(bool isOn)
    {
        if (isOn) { _coolRelay.State = RelayState.Open; }

        _heatRelay.State = isOn ? RelayState.Closed : RelayState.Open;
    }
}
