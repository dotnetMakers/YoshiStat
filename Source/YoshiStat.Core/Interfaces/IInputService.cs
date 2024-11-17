using System;

namespace YoshiStat;

public interface IInputService
{
    public event EventHandler? OnIncrementRequested;
    public event EventHandler? OnDecrementRequested;
    public event EventHandler? OnAcceptRequested;
    public event EventHandler? OnCancelRequested;

    public event EventHandler? OnTestButton1Clicked;
    public event EventHandler? OnTestButton2Clicked;

}
