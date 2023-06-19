using System.Collections.Generic;
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
    

    private void Awake()
    {
        LoadAll();
        Application.targetFrameRate = 60;
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveAll();
        }
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