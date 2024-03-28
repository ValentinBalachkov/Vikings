using System;
using _Vikings._Scripts.Refactoring;
using _Vikings.Refactoring.Character;

namespace Vikings.Object
{

    [Serializable]
    public class AbstractBuildingDynamicData 
    {
        public string SaveKey;
        public ItemCount[] CurrentItemsCount;
        public int BuildingTime;
        public BuildingState State;
        public int CurrentLevel;
    }
    
    [Serializable]
    public class StorageDynamicData : AbstractBuildingDynamicData
    {
        public int Count;
        public int MaxStorageCount;
        public ResourceType ResourceType;
    }
    
    [Serializable]
    public class CraftingTableDynamicData : AbstractBuildingDynamicData
    {
        public ItemCount[] CurrentItemsCountWeapon;
    }
    
    [Serializable]
    public class CharacterDynamicData
    {
        public string SaveKey;
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
    
    [Serializable]
    public class TaskDynamicData
    {
        public string SaveKey;
        public TaskStatus taskStatus;
        public bool accessDone;
    }
    
    [Serializable]
    public class DateTimeDynamicData
    {
        public string SaveKey;
        public string currentDateTime;
    }
}