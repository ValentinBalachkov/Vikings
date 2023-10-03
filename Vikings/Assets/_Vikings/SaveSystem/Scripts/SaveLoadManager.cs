using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Vikings.Building;
using Vikings.Chanacter;
using Vikings.Items;
using Vikings.Weapon;

public class SaveLoadManager : MonoBehaviour
{
    [SerializeField] private List<StorageData> _storageData = new();
    [SerializeField] private List<BuildingData> _buildingData = new();
    [SerializeField] private List<ItemData> _itemData = new();
    [SerializeField] private List<WeaponData> _weaponData = new();
    [SerializeField] private List<CraftingTableData> _craftingTableData = new();
    [SerializeField] private CharactersConfig _charactersConfig;
    [SerializeField] private TMP_Text _versionText;

    [SerializeField] private DateTimeData _dateTimeData;
    [SerializeField] private OfflineFarmView _offlineFarmView;

    private const int TIME_CONST = 10;


    private void Awake()
    {
        LoadAll();
        Application.targetFrameRate = 60;
        _versionText.text = $"v{Application.version}";
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
            _dateTimeData.currentDateTime = DateTime.Now.ToString();
            _dateTimeData.Save();
        }
    }

    private void GetResources()
    {
        var time = GetTimeAfterRestart();

        if (time == 0)
        {
            return;
        }

        var charCount = _charactersConfig.charactersCount + 1;

        float wRock = GetCharactersCapacity(charCount,
            GetRouteTime(_charactersConfig.SpeedMove), _itemData[2].CollectTime, _charactersConfig.ItemsCount,
            _weaponData.FirstOrDefault(x => x.id == 1).level + 1);

        float wWood = GetCharactersCapacity(charCount,
            GetRouteTime(_charactersConfig.SpeedMove), _itemData[3].CollectTime, _charactersConfig.ItemsCount,
            _weaponData.FirstOrDefault(x => x.id == 0).level + 1);

        float wEat = GetCharactersCapacity(charCount,
            GetRouteTime(_charactersConfig.SpeedMove), _itemData[1].CollectTime, _charactersConfig.ItemsCount,
            _charactersConfig.ItemsCount);
        
        var currentBuilding = _buildingData.FirstOrDefault(x => x.isSetOnMap && !x.IsBuild);

        string craftName = "";
        int level = 0;

        if (currentBuilding != null)
        {
            CalculateResourceBuilding(ref time, currentBuilding, wRock, wWood, ref craftName, ref level);
        }

        var storagesData = _storageData
            .Where(x => x.CurrentLevel > 0 && x.Count < x.MaxStorageCount)
            .OrderBy(x => x.priority)
            .ToArray();

        List<float> wList = new();

        for (int i = 0; i < storagesData.Length; i++)
        {
            switch (storagesData[i].ItemType.ID)
            {
                case 1:
                    wList.Add(wEat);
                    break;
                case 2:
                    wList.Add(wRock);
                    break;
                case 3:
                    wList.Add(wWood);
                    break;
            }
        } 
        
        Dictionary<int, int> resourcesDict = new Dictionary<int, int>();

        if (time > 0)
        {
            CalculateResourceStorage(ref time, storagesData, wList.ToArray(), ref resourcesDict);
        }
        
        _offlineFarmView.OpenWindow(resourcesDict, craftName, level);
    }

    private void CalculateResourceBuilding(ref float time, BuildingData buildingData, float wRock, float wWood, ref string buildingName, ref int level)
    {
        var stick = buildingData.currentItemsCount[0];
        var rock = buildingData.currentItemsCount[1];

        List<float> timesIteration = new();


        for (int i = 0; i < 2; i++)
        {
            float w = i == 0 ? wWood : wRock;
            int currentCount = i == 0 ? stick.count : rock.count;

            var m = buildingData.PriceToUpgrades[i].count - currentCount;
            
            
            
            if (i > 0)
            {
                time = timesIteration[i - 1] - (m / w);
            }
            else
            {
                time -= (m / w);
            }
            
            DebugLogger.SendMessage($"Building: iteration(i) - {i}, w = {w}, T[{i}] = {time}", Color.green);

            timesIteration.Add(time);

            if (time > 0)
            {
                buildingData.currentItemsCount[i].count += m;
            }
            else
            {
                if (i > 0)
                {
                    float t = timesIteration[i - 1] - Mathf.Abs(timesIteration[i]);
                    buildingData.currentItemsCount[i].count += (int)(t * w);
                }
                else
                {
                    buildingData.currentItemsCount[i].count += (int)(Mathf.Abs(timesIteration[i]) * w);
                }

                return;
            }
        }

        if (buildingData.StorageData != null)
        {
            if (buildingData.StorageData.BuildTime < time)
            {
                buildingData.isSetOnMap = true;
                buildingData.IsBuild = true;
                time -= (int)buildingData.StorageData.BuildTime;
                buildingName = buildingData.StorageData.nameText;
                level = buildingData.StorageData.CurrentLevel;
            }
            else
            {
                buildingData.StorageData.BuildTime -= time;
                time = 0;
            }
        }
        else
        {
            if (buildingData.craftingTableCrateTime < time)
            {
                buildingData.isSetOnMap = true;
                buildingData.IsBuild = true;
                time -= buildingData.craftingTableCrateTime;
                buildingName = buildingData.CraftingTableController.CraftingTableData.nameText;
                level = buildingData.CraftingTableController.CraftingTableData.currentLevel;
            }
            else
            {
                buildingData.StorageData.BuildTime -= time;
                time = 0;
            }
        }
    }

    private void CalculateResourceStorage(ref float time, StorageData[] storagesData, float[] w, ref Dictionary<int,int> resourceDict)
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
            
            DebugLogger.SendMessage($"Storages: iteration(i) - {i}, w = {w}, T[{i}] = {time}", Color.green);

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
        return TIME_CONST / moveSpeed;
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

    private void SaveAll()
    {
        foreach (var data in _storageData)
        {
            data.Save();
        }

        foreach (var data in _buildingData)
        {
            data.Save();
        }

        foreach (var data in _itemData)
        {
            data.Save();
        }

        foreach (var data in _weaponData)
        {
            data.Save();
        }

        foreach (var data in _craftingTableData)
        {
            data.Save();
        }

        _charactersConfig.Save();
    }

    private void LoadAll()
    {
        foreach (var data in _storageData)
        {
            data.Load();
        }

        foreach (var data in _buildingData)
        {
            data.Load();
        }

        foreach (var data in _itemData)
        {
            data.Load();
        }

        foreach (var data in _weaponData)
        {
            data.Load();
        }

        foreach (var data in _craftingTableData)
        {
            data.Load();
        }

        _charactersConfig.Load();
        
        GetResources();
    }
}