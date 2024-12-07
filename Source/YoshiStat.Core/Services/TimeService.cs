using System;

namespace YoshiStat.Core;

public class TimeService
{
    private ISettingsService _settingsService;

    public TimeService(ISettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    public string GetFormattedLocalTime()
    {
        if (_settingsService.GetCurrentTimeFormat() == TimeFormat.Hours_12)
        {
            return DateTime.Now.ToString("h:mm tt");
        }

        return DateTime.Now.ToString("HH:mm");
    }
}
