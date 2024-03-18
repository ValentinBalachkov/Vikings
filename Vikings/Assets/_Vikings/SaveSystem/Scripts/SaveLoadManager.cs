using System;
using System.Collections.Generic;
using System.Linq;
using _Vikings._Scripts.Refactoring;
using TMPro;
using UnityEngine;
using Vikings.Building;
using Vikings.Map;
using Zenject;

public class SaveLoadManager : MonoBehaviour
{
    public static List<ISave> saves = new();

    [SerializeField] private TMP_Text _versionText;

    [SerializeField] private DateTimeData _dateTimeData;
    [SerializeField] private OfflineFarmView _offlineFarmView;

    [SerializeField] private TMP_InputField _timeCheatIF;
    [SerializeField] private TMP_InputField _constCheatIF;

    private ConfigSetting _configSetting;
    private MapFactory _mapFactory;

    private const int TIME_CONST = 10;

    public void CheatTimeSave()
    {
        int time;

        if (Int32.TryParse(_timeCheatIF.text, out time))
        {
            _dateTimeData.cheatTime = time;
            _dateTimeData.Save();
            DebugLogger.SendMessage($"Save Time {_dateTimeData.cheatTime} sec", Color.green);
            return;
        }

        DebugLogger.SendMessage("Incorrect Input", Color.red);
    }

    public void CheatConstSave()
    {
        int time;

        if (Int32.TryParse(_constCheatIF.text, out time))
        {
            _dateTimeData.timeConst = time;
            _dateTimeData.Save();
            DebugLogger.SendMessage($"Save Const {_dateTimeData.timeConst}", Color.green);
            return;
        }

        DebugLogger.SendMessage("Incorrect Input", Color.red);
    }

    [Inject]
    private void Init(MapFactory mapFactory, ConfigSetting configSetting)
    {
        _mapFactory = mapFactory;
        _configSetting = configSetting;
    }


    private void Start()
    {
        //_taskManager.SubscribeChangeStatusEvent();
        Application.targetFrameRate = 60;
//        _versionText.text = $"v{Application.version}";
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveAll();
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
        }
        else
        {
            // _dateTimeData.currentDateTime = DateTime.Now.ToString();
            // _dateTimeData.Save();
        }
    }

    // private void GetResources()
    // {
    //     DebugLogger.SendMessage(
    //         $"Current const: {_dateTimeData.timeConst} \n Current loaded time: {_dateTimeData.cheatTime}sec",
    //         Color.yellow);
    //
    //     //var time = GetTimeAfterRestart();
    //     var time = (float)_dateTimeData.cheatTime;
    //
    //     if (time == 0)
    //     {
    //         return;
    //     }
    //
    //     var charCount = _charactersConfig.charactersCount + 1;
    //
    //     float wRock = GetCharactersCapacity(charCount,
    //         GetRouteTime(_charactersConfig.SpeedMove), _itemData[2].CollectTime, _charactersConfig.ItemsCount,
    //         _weaponData.FirstOrDefault(x => x.id == 1).level + 1);
    //
    //     float wWood = GetCharactersCapacity(charCount,
    //         GetRouteTime(_charactersConfig.SpeedMove), _itemData[3].CollectTime, _charactersConfig.ItemsCount,
    //         _weaponData.FirstOrDefault(x => x.id == 0).level + 1);
    //
    //     float wEat = GetCharactersCapacity(charCount,
    //         GetRouteTime(_charactersConfig.SpeedMove), _itemData[1].CollectTime, _charactersConfig.ItemsCount,
    //         _charactersConfig.ItemsCount);
    //
    //     // var currentBuilding = _buildingData.FirstOrDefault(x => x.isSetOnMap && !x.IsBuild);
    //     //
    //     // var currentBuildingUpgrade =
    //     //     _buildingData.FirstOrDefault(x => x.StorageData != null && x.StorageData.DynamicData.IsUpgrade);
    //
    //     string craftName = "";
    //     int level = 0;
    //     Sprite sprite = null;
    //
    //     // if (_craftingTableData[0].isUpgrade)
    //     // {
    //     //     CalculateResourceBuilding(ref time, _buildingData[0], wRock, wWood, ref craftName, ref level,
    //     //         ref sprite);
    //     // }
    //     else if (currentBuildingUpgrade != null)
    //     {
    //         CalculateResourceBuilding(ref time, currentBuildingUpgrade, wRock, wWood, ref craftName, ref level,
    //             ref sprite);
    //     }
    //     else if (currentBuilding != null)
    //     {
    //         CalculateResourceBuilding(ref time, currentBuilding, wRock, wWood, ref craftName, ref level, ref sprite);
    //     }
    //     else if (_craftingTableData[0].currentItemsCount.Count > 0)
    //     {
    //         int weaponId = _craftingTableData[0].currentWeaponId;
    //         var weapon = _weaponData.FirstOrDefault(x => x.id == weaponId);
    //         CalculateResourceCraftingTable(ref time, _craftingTableData[0], weapon, wRock, wWood, ref craftName,
    //             ref level, ref sprite);
    //     }
    //     else if (_craftingTableData[1].currentItemsCount.Count > 0)
    //     {
    //         int weaponId = _craftingTableData[1].currentWeaponId;
    //         var weapon = _weaponData.FirstOrDefault(x => x.id == weaponId);
    //         CalculateResourceCraftingTable(ref time, _craftingTableData[1], weapon, wRock, wWood, ref craftName,
    //             ref level, ref sprite);
    //     }
    //
    //     var storagesData = _storageData
    //         .Where(x => x.CurrentLevel > 0 && x.Count < x.MaxStorageCount)
    //         .OrderBy(x => x.priority)
    //         .ToArray();
    //
    //     List<float> wList = new();
    //
    //     for (int i = 0; i < storagesData.Length; i++)
    //     {
    //         switch (storagesData[i].ItemType.ID)
    //         {
    //             case 1:
    //                 wList.Add(wEat);
    //                 break;
    //             case 2:
    //                 wList.Add(wRock);
    //                 break;
    //             case 3:
    //                 wList.Add(wWood);
    //                 break;
    //         }
    //     }
    //
    //     Dictionary<int, int> resourcesDict = new Dictionary<int, int>();
    //
    //     if (time > 0)
    //     {
    //         CalculateResourceStorage(ref time, storagesData, wList.ToArray(), ref resourcesDict);
    //     }
    //
    //
    //     _offlineFarmView.OpenWindow(resourcesDict, craftName, level, sprite);
    // }
    //
    // private void CalculateResourceBuilding(ref float time, BuildingData buildingData, float wRock, float wWood,
    //     ref string buildingName, ref int level, ref Sprite sprite)
    // {
    //     if (_craftingTableData[0].isUpgrade)
    //     {
    //         var stick = _craftingTableData[0].PriceToUpgrade[0];
    //         var rock = _craftingTableData[0].PriceToUpgrade[0];
    //
    //         List<float> timesIteration = new();
    //
    //
    //         for (int i = 0; i < 2; i++)
    //         {
    //             float w = i == 0 ? wWood : wRock;
    //             int currentCount = i == 0 ? stick.count : rock.count;
    //
    //             var m = _craftingTableData[0].currentItemsPriceToUpgrade[i].count - currentCount;
    //
    //
    //             if (i > 0)
    //             {
    //                 time = timesIteration[i - 1] - (m / w);
    //             }
    //             else
    //             {
    //                 time -= (m / w);
    //             }
    //
    //             DebugLogger.SendMessage($"Building: iteration(i) - {i}, w = {w}, T[{i}] = {time}", Color.green);
    //
    //             timesIteration.Add(time);
    //
    //             if (time > 0)
    //             {
    //                 _craftingTableData[0].currentItemsPriceToUpgrade[i].count += m;
    //             }
    //             else
    //             {
    //                 if (i > 0)
    //                 {
    //                     float t = timesIteration[i - 1] - Mathf.Abs(timesIteration[i]);
    //                     _craftingTableData[0].currentItemsPriceToUpgrade[i].count += (int)(t * w);
    //                 }
    //                 else
    //                 {
    //                     _craftingTableData[0].currentItemsPriceToUpgrade[i].count +=
    //                         (int)(Mathf.Abs(timesIteration[i]) * w);
    //                 }
    //
    //                 return;
    //             }
    //         }
    //     }
    //     else
    //     {
    //         var stick = buildingData.currentItemsCount[0];
    //         var rock = buildingData.currentItemsCount[1];
    //
    //         List<float> timesIteration = new();
    //
    //
    //         for (int i = 0; i < 2; i++)
    //         {
    //             float w = i == 0 ? wWood : wRock;
    //             int currentCount = i == 0 ? stick.count : rock.count;
    //
    //             var m = buildingData.PriceToUpgrades[i].count - currentCount;
    //
    //
    //             if (i > 0)
    //             {
    //                 time = timesIteration[i - 1] - (m / w);
    //             }
    //             else
    //             {
    //                 time -= (m / w);
    //             }
    //
    //             DebugLogger.SendMessage($"Building: iteration(i) - {i}, w = {w}, T[{i}] = {time}", Color.green);
    //
    //             timesIteration.Add(time);
    //
    //             if (time > 0)
    //             {
    //                 buildingData.currentItemsCount[i].count += m;
    //             }
    //             else
    //             {
    //                 if (i > 0)
    //                 {
    //                     float t = timesIteration[i - 1] - Mathf.Abs(timesIteration[i]);
    //                     buildingData.currentItemsCount[i].count += (int)(t * w);
    //                 }
    //                 else
    //                 {
    //                     buildingData.currentItemsCount[i].count += (int)(Mathf.Abs(timesIteration[i]) * w);
    //                 }
    //
    //                 return;
    //             }
    //         }
    //     }
    //
    //     if (buildingData.StorageData != null)
    //     {
    //         if (buildingData.StorageData.BuildTime < time)
    //         {
    //             if (buildingData.StorageData.DynamicData.IsUpgrade)
    //             {
    //                 buildingData.StorageData.DynamicData.IsUpgrade = false;
    //             }
    //             else
    //             {
    //                 buildingData.isSetOnMap = true;
    //                 buildingData.IsBuild = true;
    //             }
    //
    //             buildingData.StorageData.CurrentLevel++;
    //             time -= (int)buildingData.StorageData.BuildTime;
    //             buildingName = buildingData.StorageData.nameText;
    //             level = buildingData.StorageData.CurrentLevel;
    //             sprite = buildingData.StorageData.iconOfflineFarm;
    //             if (buildingData.StorageData.ItemType.ID == 1 && _charactersConfig.houseLevel < 5)
    //             {
    //                 _charactersConfig.charactersCount++;
    //             }
    //         }
    //         else
    //         {
    //             //buildingData.StorageData.BuildTime -= time;
    //             time = 0;
    //         }
    //     }
    //     else
    //     {
    //         if (buildingData.craftingTableCrateTime < time)
    //         {
    //             if (_craftingTableData[0].isUpgrade)
    //             {
    //                 _craftingTableData[0].isUpgrade = false;
    //             }
    //             else
    //             {
    //                 buildingData.isSetOnMap = true;
    //                 buildingData.IsBuild = true;
    //             }
    //
    //             _craftingTableData[0].currentLevel++;
    //             time -= buildingData.craftingTableCrateTime;
    //             buildingName = _craftingTableData[0].nameText;
    //             level = _craftingTableData[0].currentLevel;
    //             sprite = _craftingTableData[0].iconOfflineFarm;
    //         }
    //         else
    //         {
    //             //_craftingTableData[0].tableBuildingTime -= (int)time;
    //             time = 0;
    //         }
    //     }
    // }

    private void CalculateResourceCraftingTable(ref float time, CraftingTableData craftingTableData,
        WeaponData weaponData, float wRock,
        float wWood, ref string buildingName, ref int level, ref Sprite sprite)
    {
        var stick = craftingTableData.currentItemsCount[0];
        var rock = craftingTableData.currentItemsCount[1];

        List<float> timesIteration = new();


        for (int i = 0; i < 2; i++)
        {
            float w = i == 0 ? wWood : wRock;
            int currentCount = i == 0 ? stick.count : rock.count;

            var m = craftingTableData.priceToUpgradeCraftingTable[i].count - currentCount;


            if (i > 0)
            {
                time = timesIteration[i - 1] - (m / w);
            }
            else
            {
                time -= (m / w);
            }

            DebugLogger.SendMessage($"CraftingTable: iteration(i) - {i}, w = {w}, T[{i}] = {time}", Color.green);

            timesIteration.Add(time);

            if (time > 0)
            {
                craftingTableData.currentItemsCount[i].count += m;
            }
            else
            {
                if (i > 0)
                {
                    float t = timesIteration[i - 1] - Mathf.Abs(timesIteration[i]);
                    craftingTableData.currentItemsCount[i].count += (int)(t * w);
                }
                else
                {
                    craftingTableData.currentItemsCount[i].count += (int)(Mathf.Abs(timesIteration[i]) * w);
                }

                return;
            }
        }


        // if (craftingTableData.craftingTime < time)
        // {
        //     time -= craftingTableData.craftingTime;
        //     weaponData.level++;
        //     weaponData.IsOpen = true;
        //     var items = _itemData.Where(x => x.ID == weaponData.ItemData.ID).ToList();
        //     foreach (var item in items)
        //     {
        //         item.IsOpen = true;
        //     }
        //
        //     craftingTableData.Clear();
        //     buildingName = weaponData.nameText;
        //     level = weaponData.level;
        //     sprite = weaponData.iconOfflineFarm;
        // }
        // else
        // {
        //     craftingTableData.craftingTime -= (int)time;
        //     time = 0;
        // }
    }

    private void CalculateResourceStorage(ref float time, StorageData[] storagesData, float[] w,
        ref Dictionary<int, int> resourceDict)
    {
        List<float> timesIteration = new();

        for (int i = 0; i < storagesData.Length; i++)
        {
            int currentCount = storagesData[i].Count;

            var m = storagesData[i].MaxStorageCount - currentCount;
            if (i > 0)
            {
                time = timesIteration[i - 1] - (m / w[i]);
            }
            else
            {
                time -= (m / w[i]);
            }

            DebugLogger.SendMessage($"Storages: iteration(i) - {i}, w = {w[i]}, T[{i}] = {time}", Color.green);

            timesIteration.Add(time);

            if (time > 0)
            {
                storagesData[i].Count += m;
                resourceDict.Add(storagesData[i].ItemType.ID, m);
            }
            else
            {
                if (i > 0)
                {
                    float t = timesIteration[i - 1] - Mathf.Abs(timesIteration[i]);
                    float count = t * w[i];
                    storagesData[i].Count += (int)count;
                    resourceDict.Add(storagesData[i].ItemType.ID, (int)count);
                }
                else
                {
                    float count = Mathf.Abs(timesIteration[i]) * w[i];
                    storagesData[i].Count += (int)count;
                    resourceDict.Add(storagesData[i].ItemType.ID, (int)count);
                }

                return;
            }
        }
    }

    public void ClearAllCheatData()
    {
        _dateTimeData.timeConst = TIME_CONST;
        _dateTimeData.cheatTime = 0;
        _dateTimeData.Save();
    }

    private float GetTimeAfterRestart()
    {
        _dateTimeData.Load();
        if (_dateTimeData.currentDateTime == "")
        {
            return 0;
        }

        var parsedDateTime = DateTime.Parse(_dateTimeData.currentDateTime);
        DebugLogger.SendMessage(
            $"{DateTime.Now.Subtract(parsedDateTime).TotalSeconds} {_dateTimeData.currentDateTime} {DateTime.Now}",
            Color.green);
        return (float)DateTime.Now.Subtract(parsedDateTime).TotalSeconds;
    }

    private float GetRouteTime(float moveSpeed)
    {
        //return TIME_CONST / moveSpeed;
        return _dateTimeData.timeConst / moveSpeed;
    }

    private float GetCharactersCapacity(int charactersCount, float routeTime, float workTime, int backspaceVolume,
        int weaponsPower)
    {
        if (backspaceVolume >= weaponsPower)
        {
            return charactersCount / ((routeTime / backspaceVolume) + (workTime / backspaceVolume));
        }

        return charactersCount / ((routeTime / backspaceVolume) + (workTime / weaponsPower));
    }

    public void SaveAll()
    {
        foreach (var save in saves)
        {
            save.Save();
        }
    }

    public void LoadAll()
    {
        var buildings = _configSetting.buildingsData;

        foreach (var data in buildings)
        {
            _mapFactory.CreateBuilding(data);
        }

        _mapFactory.CreateBuilding(_configSetting.craftingTable);

        var eatStorage = _mapFactory.GetAllBuildings<EatStorage>().FirstOrDefault();

        foreach (var data in _configSetting.resourcesData)
        {
            for (int i = 0; i < eatStorage.CurrentLevel.Value + 1; i++)
            {
                _mapFactory.CreateResource(i, data);
            }
        }


        //GetResources();
    }
}