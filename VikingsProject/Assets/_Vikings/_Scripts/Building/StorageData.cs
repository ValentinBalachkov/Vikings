using System;
using UnityEngine;
using Vikings.Items;

namespace Vikings.Building
{
    [CreateAssetMenu(fileName = "StorageData", menuName = "Data/StorageData", order = 4)]
    public class StorageData : ScriptableObject
    {
        public Action<int> OnChangeCountStorage;
        public Action<int, int> OnUpgradeStorage;
        public ItemData ItemType => _itemType;
        public int Count => _count;
        public int MaxStorageCount => _maxStorageCount;
        public int CurrentLevel => _currentLevel;
        public StorageController StorageController => _storageController;

        [SerializeField] private ItemData _itemType;
        [SerializeField] private int _count;
        [SerializeField] private int _maxStorageCount;
        [SerializeField] private int _currentLevel;
        [SerializeField] private StorageController _storageController;


        public void ChangeStorageCount(int count)
        {
            if (_count + count > _maxStorageCount)
            {
                _count = _maxStorageCount;
            }
            else
            {
                _count += count;
            }
            
            OnChangeCountStorage?.Invoke(_count);
        }

        public void UpgradeStorage()
        {
            if (_currentLevel >= 2)
            {
                return;
            }
            _maxStorageCount *= _currentLevel;
            OnUpgradeStorage?.Invoke(_maxStorageCount, _currentLevel);
        }
    
    }

}
