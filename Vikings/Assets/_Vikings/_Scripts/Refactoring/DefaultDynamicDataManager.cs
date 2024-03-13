using System;
using _Vikings._Scripts.Refactoring;
using _Vikings.Refactoring.Character;
using UnityEditor;
using UnityEngine;
using Vikings.Object;
using Vikings.SaveSystem;

public class DefaultDynamicDataManager : EditorWindow
{
    public DefaultStorageData[] _defaultStoragesData;
    private SerializedObject _so;
    private SerializedProperty stringsProperty;
    
    [MenuItem("Vikings Tools/Dynamic data setting")]
    static void Init()
    {
        DefaultDynamicDataManager window = (DefaultDynamicDataManager)GetWindow(typeof(DefaultDynamicDataManager), true, "Dynamic data setting");
        
        window.Show();
    }

    private void OnEnable()
    {
        ScriptableObject target = this;
        _so = new SerializedObject(target);
        stringsProperty = _so.FindProperty("_defaultStoragesData");
        FindBuildingsData();
    }

    void OnGUI()
    {
        _so.Update();
        EditorGUILayout.PropertyField(stringsProperty, true); // True means show children
        _so.ApplyModifiedProperties(); // Remember to apply modified properties
        
        if (GUILayout.Button("Write building files"))
        {
            ClearStoragesData();
        }
    }

    private void ClearStoragesData()
    {
        foreach (var data in _defaultStoragesData)
        {
            SaveLoadSystem.SaveData(data.defaultStorageData, data.buildingData.saveKey);
        }
    }

    private void FindBuildingsData()
    {
        var buildingsData = Resources.LoadAll<BuildingData>("Building");

        _defaultStoragesData = new DefaultStorageData[buildingsData.Length];
        
        for (int i = 0; i < buildingsData.Length; i++)
        {
            _defaultStoragesData[i] = new DefaultStorageData()
            {
                buildingData = buildingsData[i],
                defaultStorageData = new StorageDynamicData()
                {
                    SaveKey = buildingsData[i].saveKey,
                    CurrentItemsCount = new[]
                    {
                        new ItemCount()
                        {
                            resourceType = ResourceType.Wood,
                            count = 0
                        },
                        
                        new ItemCount()
                        {
                            resourceType = ResourceType.Rock,
                            count = 0
                        }
                    }
                }
            };
        }
    }
}

[Serializable]
public class DefaultStorageData
{
    public BuildingData buildingData;
    public StorageDynamicData defaultStorageData;
}