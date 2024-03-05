using System.IO;
using UnityEngine;
using Vikings.Object;

namespace Vikings.SaveSystem
{
    public static class SaveLoadSystem
    {
        public static void SaveData<T>(T dynamicData, string dataKey)
        {
            string saveFileName = dataKey + "Data.json";
            string savePath = GetDataPath(saveFileName);
            string json = JsonUtility.ToJson(dynamicData);
            File.WriteAllText(savePath, json);
        }
        
        public static T LoadData<T>(T dynamicData, string dataKey)
        {
            string loadFileName = dataKey + "Data.json";
            string loadPath = GetDataPath(loadFileName);

            if (!File.Exists(loadPath))
            {
                DebugLogger.SendMessage($"Data not fount from path {loadPath}", Color.red);
                return dynamicData;
            }

            string json = File.ReadAllText(loadPath);
            JsonUtility.FromJsonOverwrite(json, dynamicData);
            return dynamicData;
        }

        private static string GetDataPath(string saveFileName)
        {
            string SavePath;
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        SavePath = Path.Combine(Application.persistentDataPath, saveFileName);
#else
            SavePath = Path.Combine(Application.dataPath + "/_Vikings/Save", saveFileName);
#endif
            return SavePath;
        }

    }
}