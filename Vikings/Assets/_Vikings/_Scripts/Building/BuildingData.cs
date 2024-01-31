using SecondChanceSystem.SaveSystem;
using UnityEngine;
using Vikings.Object;

namespace Vikings.Building
{
    [CreateAssetMenu(fileName = "BuildingData", menuName = "Data/BuildingData", order = 5)]
    public class BuildingData : ScriptableObject, IData
    {
        public StorageData StorageData => _storageData;

        public bool IsBuild
        {
            get => _isBuild;
            set => _isBuild = value;
        }

        public CraftingTableController CraftingTableController => _craftingTableController;
        public ItemCount[] PriceToUpgrades => _priceToUpgrades;
        
        public ItemCount[] currentItemsCount;
        public BuildingController BuildingController => _buildingController;

        public int craftingTableCrateTime;

        public bool isSetOnMap;

        public BuildingDynamicData BuildingDynamicData;

        [SerializeField] private StorageData _storageData;
        [SerializeField] private bool _isBuild;
        [SerializeField] private ItemCount[] _priceToUpgrades;
        
        [SerializeField] private string _buildingName;
        [SerializeField] private BuildingController _buildingController;


        [SerializeField] private CraftingTableController _craftingTableController;

        public void Save()
        {
            BuildingDynamicData.CurrentItemsCount = new int[currentItemsCount.Length];
            
            for (int i = 0; i < BuildingDynamicData.CurrentItemsCount.Length; i++)
            {
                BuildingDynamicData.CurrentItemsCount[i] = currentItemsCount[i].count;
            }
            
            SaveLoadSystem.SaveData(BuildingDynamicData);
        }

        public void Load()
        {
            var data = SaveLoadSystem.LoadData(BuildingDynamicData);
           if (data != null)
           {
               BuildingDynamicData = data;

               for (int i = 0; i < BuildingDynamicData.CurrentItemsCount.Length; i++)
               {
                   currentItemsCount[i].count = BuildingDynamicData.CurrentItemsCount[i];
               }
           }
        }
    }
}

