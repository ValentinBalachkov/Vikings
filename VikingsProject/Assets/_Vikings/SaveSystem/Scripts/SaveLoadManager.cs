using System.Collections.Generic;
using UnityEngine;
using Vikings.Building;
using Vikings.Items;
using Vikings.Weapon;

public class SaveLoadManager : MonoBehaviour
{
    [SerializeField] private List<StorageData> _storageData = new();
    [SerializeField] private List<BuildingData> _buildingData = new();
    [SerializeField] private List<ItemData> _itemData = new();
    [SerializeField] private List<WeaponData> _weaponData = new();
    [SerializeField] private List<CraftingTableData> _craftingTableData = new();

    private void Awake()
    {
        LoadAll();
    }

    private void OnApplicationQuit()
    {
        SaveAll();
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
    }
}