using System;
using UnityEngine;

namespace Vikings.Building
{
    public class ResourcesAfterRestartManager : MonoBehaviour
    {
        [SerializeField] private DateTimeData _dateTimeData;

        private const int TIME_CONST = 10;
        private const int TIME_WORK_ANIM = 10;

        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
            {
                _dateTimeData.Load();
                if (_dateTimeData.currentDateTime == "")
                {
                    return;
                }
                var parsedDateTime = DateTime.Parse(_dateTimeData.currentDateTime);
                var time = DateTime.Now.Subtract(parsedDateTime);
                DebugLogger.SendMessage($"{time.TotalSeconds} {_dateTimeData.currentDateTime} {DateTime.Now}", Color.green);
            }
            else
            {
                _dateTimeData.currentDateTime = DateTime.Now.ToString();
                _dateTimeData.Save();
            }
        }

        private int GetRouteTime(int moveSpeed)
        {
            return TIME_CONST / moveSpeed;
        }

        private int GetCharactersCapacity(int charactersCount, int routeTime, int workTime, int backspaceVolume, int weaponsPower)
        {
            if (backspaceVolume >= weaponsPower)
            {
                return charactersCount / ((routeTime / backspaceVolume) + (workTime / backspaceVolume));
            }

            return charactersCount / ((routeTime / backspaceVolume) + (workTime / weaponsPower));
        }
    }
}