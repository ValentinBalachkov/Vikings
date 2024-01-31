using System.IO;
using UnityEngine;
using Vikings.Object;

namespace SecondChanceSystem.SaveSystem
{
    public static class SaveLoadSystem
    {
        public static void SaveData<T>(T saveData) where T : ISaveData
        {
            string saveFileName = saveData.Name + "Data.json";
            string savePath = GetDataPath(saveFileName);
            string json = JsonUtility.ToJson(saveData);
            File.WriteAllText(savePath, json);
        }
        
        public static T LoadData<T>(T loadData) where T : ISaveData
        {
            string loadFileName = loadData.Name + "Data.json";
            string loadPath = GetDataPath(loadFileName);

            if (!File.Exists(loadPath))
            {
                return loadData;
            }
            else
            {
                string json = File.ReadAllText(loadPath);
                JsonUtility.FromJsonOverwrite(json, loadData);
                return loadData;
            }
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