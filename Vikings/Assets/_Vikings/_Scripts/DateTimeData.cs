using System;
using SecondChanceSystem.SaveSystem;
using UnityEngine;

namespace Vikings.Building
{
    [CreateAssetMenu(fileName = "DateTimeData", menuName = "Data/DateTimeData")]
    public class DateTimeData : ScriptableObject, IData
    {
        public string  currentDateTime;
        public int cheatTime;
        public int timeConst;
        
        public void Save()
        {
            //SaveLoadSystem.SaveData(this);
        }

        public void Load()
        {
            // var data = SaveLoadSystem.LoadData(this) as DateTimeData;
            // if (data != null)
            // {
            //     currentDateTime = data.currentDateTime;
            //     cheatTime = data.cheatTime;
            //     timeConst = data.timeConst;
            // }
        }
    }
}