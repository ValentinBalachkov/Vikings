using System.Collections.Generic;
using UnityEngine;

public static class AppMetricaEvents
{
    public static void SendLevelFinishedEvent()
    {
        int currentLevel = PlayerPrefs.GetInt("current_level_index", 0);

        var paramDict = new Dictionary<string, object>
                        {
                            { "level_number", currentLevel }
                        };

        AppMetrica.Instance.ReportEvent("level_finish", paramDict);
    }

    public static void SendLevelStartedEvent()
    {
        int currentLevel = PlayerPrefs.GetInt("current_level_index", 1);

        var paramDict = new Dictionary<string, object>
                        {
                            { "level_number", currentLevel }
                        };

        AppMetrica.Instance.ReportEvent("level_start", paramDict); 
    }
}