using System;
using System.Collections.Generic;
using System.Linq;
using SecondChanceSystem.SaveSystem;
using UnityEngine;
using Vikings.Items;
using Vikings.UI;

namespace Vikings.Building
{
    [CreateAssetMenu(fileName = "StorageData", menuName = "Data/StorageData", order = 4)]
    public class StorageData : ScriptableObject, IData
    {
        public Action OnUpdateCount;
        public bool isOpen;
        public Sprite icon;
        public string nameText;
        public string description;
        public string required;
        public bool isDefaultOpen;
        public int priority;
        


        public ItemData ItemType => _itemType;

        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                OnUpdateCount?.Invoke();
            } 
        }

        public int MaxStorageCount
        {
            get => _maxStorageCount;
            set => _maxStorageCount = value;
        }

        public float BuildTime
        {
            get
            {
                if (_currentLevel == 0)
                {
                    return _buildTime;
                }
                return (0.5f * Mathf.Pow(_currentLevel + 1, 2)) + _buildTime;
            }
        }

        public StorageController StorageController => _storageController;

        public List<PriceToUpgrade> PriceToUpgrade
        {
            get
            {
                if (_currentLevel == 0)
                {
                    return _priceToUpgrade.ToList();
                }

                List<PriceToUpgrade> newPrice = new();
                foreach (var price in _priceToUpgrade)
                {
                    var a = price.count - 1;
                    float p = 0;
                    for (int i = 2; i <= _currentLevel + 1; i++)
                    {
                        p += (Mathf.Pow(i, 4) + ((a * i) - Mathf.Pow(i, 3)))/i;
                        a = (int)p;
                    }
                        
                    newPrice.Add(new PriceToUpgrade
                    {
                        count = (int)p,
                        itemData = price.itemData
                    });
                }

                return newPrice;
            }
        }

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
        
        [SerializeField] private float _buildTime;
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
