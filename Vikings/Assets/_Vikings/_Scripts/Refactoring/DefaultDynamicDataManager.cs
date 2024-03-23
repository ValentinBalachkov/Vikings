using System;
using System.Linq;
using _Vikings._Scripts.Refactoring;
using _Vikings.Refactoring.Character;
using UnityEditor;
using UnityEngine;
using Vikings.Chanacter;
using Vikings.Object;
using Vikings.SaveSystem;

#if UNITY_EDITOR
public class DefaultDynamicDataManager : EditorWindow
{
    public ConfigSetting _configSetting;
    public DefaultStorageData[] _defaultStoragesData;
    public DefaultWeaponData[] _defaultWeaponsData;
    public DefaultCraftingTableData _defaultCraftingTableData;
    public DefaultCharacterData _defaultCharacterData;
    private SerializedObject _so;
    private SerializedProperty _storageDataProperty;
    private SerializedProperty _weaponsDataProperty;
    private SerializedProperty _craftingTableDataProperty;
    private SerializedProperty _configProperty;
    private SerializedProperty _charactersProperty;
    private Vector2 scrollPosition;

    private static bool isSetup;

    [MenuItem("Vikings Tools/Dynamic data setting")]
    static void Init()
    {
        DefaultDynamicDataManager window =
            (DefaultDynamicDataManager)GetWindow(typeof(DefaultDynamicDataManager), true, "Dynamic data setting");

        window.Show();
        isSetup = false;
    }

    private void OnEnable()
    {
        ScriptableObject target = this;
        _so = new SerializedObject(target);
        _configProperty = _so.FindProperty("_configSetting");
        _storageDataProperty = _so.FindProperty("_defaultStoragesData");
        _weaponsDataProperty = _so.FindProperty("_defaultWeaponsData");
        _charactersProperty = _so.FindProperty("_defaultCharacterData");
        _craftingTableDataProperty = _so.FindProperty("_defaultCraftingTableData");
    }

    private void Update()
    {
        if (_configSetting == null)
        {
            return;
        }

        if (!isSetup)
        {
            FindBuildingsData();
            isSetup = true;
        }
    }

    void OnGUI()
    {
        scrollPosition = GUILayout.BeginScrollView(
            scrollPosition, GUILayout.Width(500), GUILayout.Height(750));

        _so.Update();
        
        if (_configSetting == null)
        {
            EditorGUILayout.PropertyField(_configProperty, true);
        }
        else
        {
            EditorGUILayout.PropertyField(_storageDataProperty, true);
            EditorGUILayout.PropertyField(_craftingTableDataProperty, true);
            EditorGUILayout.PropertyField(_weaponsDataProperty, true);
            EditorGUILayout.PropertyField(_charactersProperty, true);
        }

        _so.ApplyModifiedProperties();

        GUILayout.EndScrollView();


        if (GUILayout.Button("Write building files"))
        {
            SaveStoragesData();
        }
    }

    private void SaveStoragesData()
    {
        foreach (var data in _defaultStoragesData)
        {
            SaveLoadSystem.SaveData(data.defaultBuildingData, data.buildingData.saveKey);
        }
        
        SaveLoadSystem.SaveData(_defaultCraftingTableData.defaultBuildingData, _defaultCraftingTableData.buildingData.saveKey);

        foreach (var data in _defaultWeaponsData)
        {
            SaveLoadSystem.SaveData(data.weaponDynamicData, data.weaponData.saveKey);
        }
        
        SaveLoadSystem.SaveData(_defaultCharacterData.characterDynamicData, _defaultCharacterData.charactersConfig.saveKey);
    }

    private void FindBuildingsData()
    {
        var buildingsData = _configSetting.buildingsData;
        var weaponsData = _configSetting.weaponsData.Where(x => x.saveKey != "WeaponHand").ToList();

        var data = _configSetting.craftingTable;

        _defaultCharacterData = new DefaultCharacterData()
        {
            charactersConfig = _configSetting.charactersConfig,
            characterDynamicData = new CharacterDynamicData()
            {
                charactersCount = 1,
                itemsCountLevel = 1,
                SaveKey = _configSetting.charactersConfig.saveKey,
                speedWorkLevel = 1,
                speedMoveLevel = 1
            }
        };
        
        _defaultCraftingTableData = new DefaultCraftingTableData()
        {
            buildingData = data,
            defaultBuildingData = new CraftingTableDynamicData()
            {
                SaveKey = data.saveKey,
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
                },
                CurrentItemsCountWeapon = new[]
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

        _defaultStoragesData = new DefaultStorageData[buildingsData.Count];

        for (int i = 0; i < buildingsData.Count; i++)
        {
            _defaultStoragesData[i] = new DefaultStorageData()
            {
                buildingData = buildingsData[i],
                defaultBuildingData = new StorageDynamicData()
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

        _defaultWeaponsData = new DefaultWeaponData[weaponsData.Count];

        for (int i = 0; i < weaponsData.Count; i++)
        {
            _defaultWeaponsData[i] = new DefaultWeaponData()
            {
                weaponData = weaponsData[i],
                weaponDynamicData = new WeaponDynamicData()
                {
                    SaveKey = weaponsData[i].saveKey,
                    IsSetOnCraftingTable = false,
                    Level = 0
                }
            };
        }
    }
}
#endif


[Serializable]
public class DefaultStorageData
{
    public BuildingData buildingData;
    public StorageDynamicData defaultBuildingData;
}

[Serializable]
public class DefaultCraftingTableData
{
    public BuildingData buildingData;
    public CraftingTableDynamicData defaultBuildingData;
}

[Serializable]
public class DefaultWeaponData
{
    public WeaponData weaponData;
    public WeaponDynamicData weaponDynamicData;
}

[Serializable]
public class DefaultCharacterData
{
    public CharactersConfig charactersConfig;
    public CharacterDynamicData characterDynamicData;
}