using SecondChanceSystem.SaveSystem;
using UnityEngine;

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
        public PriceToUpgrade[] PriceToUpgrades => _priceToUpgrades;
        public float BuildTime => _buildTime;
        public PriceToUpgrade[] currentItemsCount;
        public BuildingController BuildingController => _buildingController;

        public bool isSetOnMap;

        [SerializeField] private StorageData _storageData;
        [SerializeField] private bool _isBuild;
        [SerializeField] private PriceToUpgrade[] _priceToUpgrades;
        [SerializeField] private float _buildTime;
        [SerializeField] private string _buildingName;
        [SerializeField] private BuildingController _buildingController;


        [SerializeField] private CraftingTableController _craftingTableController;

        public void Save()
        {
            SaveLoadSystem.SaveData(this);
        }

        public void Load()
        {
            var data = SaveLoadSystem.LoadData(this) as BuildingData;
           if (data != null)
           {
               _isBuild = data._isBuild;
               currentItemsCount = data.currentItemsCount;
               isSetOnMap = data.isSetOnMap;
           }
        }
    }
}

