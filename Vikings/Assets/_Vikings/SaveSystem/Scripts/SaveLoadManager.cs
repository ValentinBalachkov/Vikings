using System;
using System.Collections.Generic;
using System.Linq;
using _Vikings._Scripts.Refactoring;
using TMPro;
using UnityEngine;
using Vikings.Building;
using Vikings.Map;
using Vikings.Object;
using Zenject;

public class SaveLoadManager : MonoBehaviour
{
    public static List<ISave> saves = new();

    [SerializeField] private TMP_Text _versionText;

    [SerializeField] private DateTimeData _dateTimeData;

    [SerializeField] private CharactersTaskManager charactersTaskManager;
    [SerializeField] private BuildingHomeActionController _buildingHomeAction;

    private ConfigSetting _configSetting;
    private MapFactory _mapFactory;
    private CharacterFactory _characterFactory;
    private WeaponFactory _weaponFactory;
    private MainPanelManager _mainPanelManager;

    [Inject]
    private void Init(MapFactory mapFactory, ConfigSetting configSetting, CharacterFactory characterFactory,
        WeaponFactory weaponFactory, MainPanelManager mainPanelManager)
    {
        _mapFactory = mapFactory;
        _configSetting = configSetting;
        _characterFactory = characterFactory;
        _weaponFactory = weaponFactory;
        _mainPanelManager = mainPanelManager;
    }


    private void Start()
    {
        Application.targetFrameRate = 60;
        _versionText.text = $"v{Application.version}";
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
        }
        else
        {
            _dateTimeData.dateTimeDynamicData.currentDateTime = DateTime.Now.ToString();
            _dateTimeData.Save();
            SaveAll();
        }
    }

    public void GetResources()
    {
        var time = GetTimeAfterRestart();

        if (time == 0)
        {
            return;
        }

        var eatStorage = _mapFactory.GetAllBuildings<EatStorage>().FirstOrDefault();


        var charCount = eatStorage.CurrentLevel.Value + 1;

        float wRock = GetCharactersCapacity(charCount,
            GetRouteTime(_characterFactory.CharacterManager.SpeedMove), _configSetting.resourcesData[2].CollectTime,
            _characterFactory.CharacterManager.ItemsCount,
            _weaponFactory.GetWeapon(_configSetting.weaponsData[2]).Level.Value + 1);

        float wWood = GetCharactersCapacity(charCount,
            GetRouteTime(_characterFactory.CharacterManager.SpeedMove), _configSetting.resourcesData[3].CollectTime,
            _characterFactory.CharacterManager.ItemsCount,
            _weaponFactory.GetWeapon(_configSetting.weaponsData[1]).Level.Value + 1);

        float wEat = GetCharactersCapacity(charCount,
            GetRouteTime(_characterFactory.CharacterManager.SpeedMove), _configSetting.resourcesData[1].CollectTime,
            _characterFactory.CharacterManager.ItemsCount,
            _characterFactory.CharacterManager.ItemsCount);

        var currentBuilding = _mapFactory.GetAllBuildings<AbstractBuilding>().FirstOrDefault(x =>
            x.buildingState == BuildingState.InProgress || x.buildingState == BuildingState.Crafting);

        string craftName = "";
        int level = 0;
        Sprite sprite = null;

        if (currentBuilding != null)
        {
            CalculateResourceBuilding(ref time, currentBuilding, wRock, wWood, ref craftName, ref level, ref sprite);
        }

        var storages = _mapFactory.GetAllBuildings<Storage>().Where(x => x.CheckNeededItem())
            .OrderBy(x => x.GetData().priorityToAction).ToList();

        List<float> wList = new();

        foreach (var storage in storages)
        {
            switch (storage.ResourceType)
            {
                case ResourceType.Eat:
                    wList.Add(wEat);
                    break;
                case ResourceType.Rock:
                    wList.Add(wRock);
                    break;
                case ResourceType.Wood:
                    wList.Add(wWood);
                    break;
            }
        }

        Dictionary<ResourceType, int> resourcesDict = new Dictionary<ResourceType, int>();

        if (time > 0)
        {
            CalculateResourceStorage(ref time, storages, wList.ToArray(), ref resourcesDict);
        }


        _mainPanelManager.SudoGetPanel<OfflineFarmView>().OpenWindow(resourcesDict, level, sprite);
    }

    private void CalculateResourceBuilding(ref float time, AbstractBuilding abstractBuilding, float wRock, float wWood,
        ref string buildingName, ref int level, ref Sprite sprite)
    {
        var stick = abstractBuilding.currentItems[ResourceType.Wood];
        var rock = abstractBuilding.currentItems[ResourceType.Rock];
        
        if (abstractBuilding is CraftingTable building && building.CurrentWeapon != null)
        {
             stick = abstractBuilding.currentItems[ResourceType.Wood];
             rock = abstractBuilding.currentItems[ResourceType.Rock];
            
        }
        

        List<float> timesIteration = new();


        for (int i = 0; i < 2; i++)
        {
            float w = i == 0 ? wWood : wRock;
            int currentCount = i == 0 ? stick : rock;

            ResourceType type = i == 0 ? ResourceType.Wood : ResourceType.Rock;

            var m = abstractBuilding.GetPriceForUpgrade()[type] - currentCount;

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
                abstractBuilding.ChangeCount?.Invoke(m, i == 0 ? ResourceType.Wood : ResourceType.Rock);
            }
            else
            {
                if (i > 0)
                {
                    float t = timesIteration[i - 1] - Mathf.Abs(timesIteration[i]);
                    abstractBuilding.ChangeCount?.Invoke((int)(t * w), ResourceType.Rock);
                }
                else
                {
                    abstractBuilding.ChangeCount?.Invoke((int)(Mathf.Abs(timesIteration[i]) * w), ResourceType.Wood);
                }

                return;
            }
        }


        if (abstractBuilding.abstractBuildingDynamicData.BuildingTime < time)
        {
            if (abstractBuilding is CraftingTable table && table.CurrentWeapon != null)
            {
                time -= table.CurrentWeapon.CraftingTime;
                buildingName = table.CurrentWeapon.GetWeaponData().nameText;
                level = table.CurrentWeapon.Level.Value + 1;
                sprite =  table.CurrentWeapon.GetWeaponData().icon;
                table.UpgradeWeapon();
            }
            else
            {
                abstractBuilding.Upgrade();
                time -= abstractBuilding.abstractBuildingDynamicData.BuildingTime;
                buildingName = abstractBuilding.GetData().nameText;
                level = abstractBuilding.CurrentLevel.Value;
                sprite = abstractBuilding.GetData().icon;
            }
        }
        else
        {
            time = 0;
        }
    }

    private void CalculateResourceStorage(ref float time, List<Storage> storages, float[] w,
        ref Dictionary<ResourceType, int> resourceDict)
    {
        List<float> timesIteration = new();

        for (int i = 0; i < storages.Count; i++)
        {
            int currentCount = storages[i].Count;

            var m = storages[i].MaxStorageCount - currentCount;
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
                storages[i].ChangeCount?.Invoke(m, storages[i].ResourceType);
                resourceDict.Add(storages[i].ResourceType, m);
            }
            else
            {
                if (i > 0)
                {
                    float t = timesIteration[i - 1] - Mathf.Abs(timesIteration[i]);
                    float count = t * w[i];
                    storages[i].ChangeCount?.Invoke((int)count, storages[i].ResourceType);
                    
                    if (count > storages[i].MaxStorageCount)
                    {
                        count = storages[i].MaxStorageCount;
                    }
                    
                    resourceDict.Add(storages[i].ResourceType, (int)count);
                }
                else
                {
                    float count = Mathf.Abs(timesIteration[i]) * w[i];
                    storages[i].ChangeCount?.Invoke((int)count, storages[i].ResourceType);
                    
                    if (count > storages[i].MaxStorageCount)
                    {
                        count = storages[i].MaxStorageCount;
                    }
                    
                    resourceDict.Add(storages[i].ResourceType, (int)count);
                }

                return;
            }
        }
    }

    private float GetTimeAfterRestart()
    {
        _dateTimeData.Init();
        if (_dateTimeData.dateTimeDynamicData.currentDateTime == "")
        {
            return 0;
        }

        var parsedDateTime = DateTime.Parse(_dateTimeData.dateTimeDynamicData.currentDateTime);
        DebugLogger.SendMessage(
            $"{DateTime.Now.Subtract(parsedDateTime).TotalSeconds} {_dateTimeData.dateTimeDynamicData.currentDateTime} {DateTime.Now}",
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

    private void SaveAll()
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
            _mapFactory.CreateBuilding(data, _mainPanelManager);
        }

        _mapFactory.CreateBuilding(_configSetting.craftingTable, _mainPanelManager);

        var eatStorage = _mapFactory.GetAllBuildings<EatStorage>().FirstOrDefault();
        
        _buildingHomeAction.Init(eatStorage);

        foreach (var data in _configSetting.resourcesData)
        {
            for (int i = 0; i < eatStorage.CurrentLevel.Value + 1; i++)
            {
                _mapFactory.CreateResource(i, data, charactersTaskManager.OnResourceEnable, _mainPanelManager);
            }
        }
        
        _dateTimeData.Init();
    }
}