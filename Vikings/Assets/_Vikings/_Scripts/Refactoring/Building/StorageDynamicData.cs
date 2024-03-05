using System;
using _Vikings._Scripts.Refactoring;
using _Vikings.Refactoring.Character;

namespace Vikings.Object
{

    [Serializable]
    public abstract class AbstractBuildingDynamicData 
    {
        public ItemCount[] CurrentItemsCount;
    }
    
    [Serializable]
    public class StorageDynamicData : AbstractBuildingDynamicData
    {
        public string SaveKey;
        public int Count;
        public int MaxStorageCount;
        public int CurrentLevel;
        public int BuildingTime;
        public BuildingState State;
        public ResourceType ResourceType;
    }
}