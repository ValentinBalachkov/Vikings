using UnityEngine;

namespace Vikings.Building
{
    [CreateAssetMenu(fileName = "BuildingData", menuName = "Data/BuildingData", order = 5)]
    public class BuildingData : ScriptableObject
    {
        public StorageData StorageData => _storageData;

        public bool IsBuild
        {
            get => _isBuild;
            set
            {
                _isBuild = value;
                SaveData();
            }
        }

        public CraftingTableController CraftingTableController => _craftingTableController;
        public PriceToUpgrade[] PriceToUpgrades => _priceToUpgrades;
        public float BuildTime => _buildTime;
        public PriceToUpgrade[] currentItemsCount;
        public BuildingController BuildingController => _buildingController;

        [SerializeField] private StorageData _storageData;
        [SerializeField] private bool _isBuild;
        [SerializeField] private PriceToUpgrade[] _priceToUpgrades;
        [SerializeField] private float _buildTime;
        [SerializeField] private string _buildingName;
        [SerializeField] private BuildingController _buildingController;


        [SerializeField] private CraftingTableController _craftingTableController;

        public void LoadData()
        {
            if (!PlayerPrefs.HasKey(_buildingName)) return;
            var value = PlayerPrefs.GetInt(_buildingName);
            if (value == 1)
            {
                _isBuild = true;
            }
        }

        private void SaveData()
        {
            int value = _isBuild ? 1 : 0;
            PlayerPrefs.SetInt(_buildingName, value);
        }
    }
}

