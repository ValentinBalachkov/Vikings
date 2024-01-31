using System;

namespace Vikings.Object
{
    public interface ISaveData
    {
        public string Name { get; set; }
    }

    [Serializable]
    public abstract class AbstractBuildingDynamicData : ISaveData
    {
        public string Name { get; set; }
        public bool IsSetOnMap;
        public int[] CurrentItemsCount;
    }
    
    [Serializable]
    public class StorageDynamicData : AbstractBuildingDynamicData
    {
        public string SaveName;
        public bool IsOpen;
        public bool IsUpgrade;
        public int Count;
        public int MaxStorageCount;
        public int CurrentLevel;
        public int BuildingTime;
        
        public string Name
        {
            get => SaveName;
            set => SaveName = value;
        }
    }
    
    [Serializable]
    public class BuildingDynamicData : ISaveData
    {
        public string SaveName;
        public bool IsBuilding;
        public bool IsSetOnMap;
        public int[] CurrentItemsCount;

        public string Name
        {
            get => SaveName;
            set => SaveName = value;
        }
    }
}