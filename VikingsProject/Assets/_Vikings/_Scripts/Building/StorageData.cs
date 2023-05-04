using SecondChanceSystem.SaveSystem;
using UnityEngine;
using Vikings.Items;

namespace Vikings.Building
{
    [CreateAssetMenu(fileName = "StorageData", menuName = "Data/StorageData", order = 4)]
    public class StorageData : ScriptableObject, IData
    {
        public bool isOpen;
        public Sprite icon;
        public string nameText;
        public string description;
        public bool isDefaultOpen;
       
        public ItemData ItemType => _itemType;

        public int Count
        {
            get => _count;
            set => _count = value;
        }

        public int MaxStorageCount
        {
            get => _maxStorageCount;
            set => _maxStorageCount = value;
        }

        public StorageController StorageController => _storageController;

        public PriceToUpgrade[] PriceToUpgrade => _priceToUpgrade;

        public int CurrentLevel
        {
            get => _currentLevel;
            set => _currentLevel = value;
        }

        [SerializeField] private ItemData _itemType;

        [SerializeField] private int _count;

        [SerializeField] private int _maxStorageCount;

        [SerializeField] private int _currentLevel;
        [SerializeField] private StorageController _storageController;

        [SerializeField] private PriceToUpgrade[] _priceToUpgrade;
        public void Save()
        {
            SaveLoadSystem.SaveData(this);
        }

        public void Load()
        {
            var data = SaveLoadSystem.LoadData(this) as StorageData;
            if (data != null)
            {
                _count = data._count;
                _maxStorageCount = data._maxStorageCount;
                _currentLevel = data._currentLevel;
            }
        }
    }

}
