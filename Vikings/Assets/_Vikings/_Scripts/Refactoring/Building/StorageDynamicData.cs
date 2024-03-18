using System;
using _Vikings._Scripts.Refactoring;
using _Vikings.Refactoring.Character;

namespace Vikings.Object
{

    [Serializable]
    public class AbstractBuildingDynamicData 
    {
        public ItemCount[] CurrentItemsCount;
        public string SaveKey;
    }
    
    [Serializable]
    public class StorageDynamicData : AbstractBuildingDynamicData
    {
        public int Count;
        public int MaxStorageCount;
        public int CurrentLevel;
        public int BuildingTime;
        public BuildingState State;
        public ResourceType ResourceType;
    }
    
    [Serializable]
    public class CraftingTableDynamicData : AbstractBuildingDynamicData
    {
        public int CurrentLevel;
        public int BuildingTime;
        public BuildingState State;
        public ItemCount[] CurrentItemsCountWeapon;
    }
    
    [Serializable]
    public class CharacterDynamicData
    {
        public string SaveKey;
        public int charactersCount;
        public int speedMoveLevel;
        public int itemsCountLevel;
        public int speedWorkLevel;
    }
    
    [Serializable]
    public class WeaponDynamicData
    {
        public string SaveKey;
        public int Level;
        public bool IsSetOnCraftingTable;
    }
}