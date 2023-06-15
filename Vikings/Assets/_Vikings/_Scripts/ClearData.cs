using UnityEditor;
using UnityEngine;
using Vikings.Building;
using Vikings.Chanacter;
using Vikings.Items;
using Vikings.Weapon;

public class ClearData : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("VikingsTools/ClearAllData")]
    public static void ClearAllData()
    {
        var buildingsData = Resources.LoadAll<BuildingData>("Building");
        
        foreach (var data in buildingsData)
        {
            foreach (var item in data.currentItemsCount)
            {
                item.count = 0;
            }

            data.isSetOnMap = false;
            data.IsBuild = false;
        }
        
        var storagesData = Resources.LoadAll<StorageData>("Building/Storages");
        
        foreach (var data in storagesData)
        {
            data.Count = 0;
            data.CurrentLevel = 0;
            data.isOpen = false;
        }
        
        var craftingTablesData = Resources.LoadAll<CraftingTableData>("Building/Storages");
        
        foreach (var data in craftingTablesData)
        {
            data.currentLevel = 0;
            data.isOpen = false;
        }
        
        var itemsData = Resources.LoadAll<ItemData>("ItemData");
        
        foreach (var data in itemsData)
        {
            if (data.DropCount > 1)
            {
                data.IsOpen = false;
            }
        }
        
        var weaponsData = Resources.LoadAll<WeaponData>("WeaponData");
        
        foreach (var data in weaponsData)
        {
            data.level = 0;
            data.IsOpen = false;
        }
        
        var characterConfig =  Resources.Load<CharactersConfig>("Character/CharactersConfig");

        characterConfig.ClearData();
    }
#endif
    
}
