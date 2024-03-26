using _Vikings._Scripts.Refactoring;
using UnityEngine;
using Vikings.Object;
using Vikings.SaveSystem;

namespace Vikings.Building
{
    [CreateAssetMenu(fileName = "DateTimeData", menuName = "Data/DateTimeData")]
    public class DateTimeData : ScriptableObject, ISave
    {
        public string saveKey;
        public int timeConst;
        public DateTimeDynamicData dateTimeDynamicData;


        public void Save()
        {
            SaveLoadSystem.SaveData(dateTimeDynamicData, saveKey);
        }

        public void Init()
        {
            dateTimeDynamicData = new();
            dateTimeDynamicData = SaveLoadSystem.LoadData(dateTimeDynamicData, saveKey);
        }
    }
}