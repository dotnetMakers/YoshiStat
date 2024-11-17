using System;

namespace YoshiStat.Core;

public interface IInputService
{
    public event EventHandler? OnIncrementRequested;

    public event EventHandler? OnDecrementRequested;

    public event EventHandler? OnAcceptRequested;

    public event EventHandler? OnCancelRequested;
}