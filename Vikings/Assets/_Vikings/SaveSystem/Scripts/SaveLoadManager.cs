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
            GetResources();
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

        int wRock = GetCharactersCapacity(_charactersConfig.charactersCount,
            GetRouteTime((int)_charactersConfig.SpeedMove), (int)_itemData[2].CollectTime, _charactersConfig.ItemsCount,
            _weaponData.FirstOrDefault(x => x.id == 2).level);

        int wWood = GetCharactersCapacity(_charactersConfig.charactersCount,
            GetRouteTime((int)_charactersConfig.SpeedMove), (int)_itemData[3].CollectTime, _charactersConfig.ItemsCount,
            _weaponData.FirstOrDefault(x => x.id == 3).level);

        int wEat = GetCharactersCapacity(_charactersConfig.charactersCount,
            GetRouteTime((int)_charactersConfig.SpeedMove), (int)_itemData[1].CollectTime, _charactersConfig.ItemsCount,
            _weaponData.FirstOrDefault(x => x.id == 1).level);

        var currentBuilding = _buildingData.FirstOrDefault(x => x.isSetOnMap && !x.IsBuild);

        if (currentBuilding != null)
        {
        }
    }

    private void CalculateResourceBuilding(ref int time, BuildingData buildingData, int wRock, int wWood)
    {
        var stick = buildingData.currentItemsCount[0];
        var rock = buildingData.currentItemsCount[1];

        List<int> timesIteration = new();
        

        for (int i = 0; i < 2; i++)
        {
            int w = i == 0 ? wWood : wRock;
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
            timesIteration.Add(time);

            if (time > 0)
            {
                buildingData.currentItemsCount[i].count += m;
            }
            else
            {
                if (i > 0)
                {
                    int t = timesIteration[i - 1] - Mathf.Abs(timesIteration[i]);
                    buildingData.currentItemsCount[i].count += t * w;
                }
                else
                {
                    
                }
                
               
            }
        }
    }

    private int GetTimeAfterRestart()
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
        return (int)DateTime.Now.Subtract(parsedDateTime).TotalSeconds;
    }

    private int GetRouteTime(int moveSpeed)
    {
        return TIME_CONST / moveSpeed;
    }

    private int GetCharactersCapacity(int charactersCount, int routeTime, int workTime, int backspaceVolume,
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
    }
}