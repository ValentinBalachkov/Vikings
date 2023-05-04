using System.IO;
using UnityEngine;

namespace SecondChanceSystem.SaveSystem
{
    public static class SaveLoadSystem
    {
        /// <summary>
        /// Метод сохранения ScriptableObject в Json
        /// </summary>
        /// <param name="saveData">ScriptableObject, который нужно сохранить</param>
        public static void SaveData(ScriptableObject saveData)
        {
            string saveFileName = saveData.name + "Data.json";
            string savePath = GetDataPath(saveFileName);
            string json = JsonUtility.ToJson(saveData);
            File.WriteAllText(savePath, json);
        }
        /// <summary>
        /// Метод загрузки ScriptableObject из Json
        /// </summary>
        /// <param name="loadData">ScriptableObject, который нужно загрузить</param>
        /// <returns>Загруженный экземпляр SO</returns>
        public static ScriptableObject LoadData(ScriptableObject loadData)
        {
            string loadFileName = loadData.name + "Data.json";
            string loadPath = GetDataPath(loadFileName);

            if (!File.Exists(loadPath))
            {
                //Debug.Log($"{loadFileName} - LoadFromFile -> FileNotFound!");
                return null;
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
            SavePath = Path.Combine(Application.dataPath + "/SecondChanceSystem/Save", saveFileName);
#endif
            return SavePath;
        }

    }
}