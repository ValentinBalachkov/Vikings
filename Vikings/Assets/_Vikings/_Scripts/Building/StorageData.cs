using System;
using System.Collections.Generic;
using System.Linq;
using _Vikings._Scripts.Refactoring;
using _Vikings._Scripts.Refactoring.Objects;
using Vikings.SaveSystem;
using UnityEngine;
using Vikings.Items;
using Vikings.Object;

namespace Vikings.Building
{
    [CreateAssetMenu(fileName = "StorageData", menuName = "Data/StorageData", order = 4)]
    public class StorageData : ScriptableObject, IData
    {
        public Action OnUpdateCount;
        public Sprite icon;
        public Sprite iconOfflineFarm;
        public string nameText;
        public string description;
        public string required;
        public bool isDefaultOpen;
        public int priority;
        public ResourceType resourceType;
        public BuildingType buildingType;

        public BuildingView _buildingView;

        public List<BuildingVisualSprites> storageVisualSprites;

        public StorageDynamicData DynamicData;

        [SerializeField] private TaskData _taskData;

        public TaskData TaskData => _taskData;
        


        public ItemData ItemType => _itemType;

        public int Count
        {
            get => _count;
            set
            {
                _count = value > _maxStorageCount ? _maxStorageCount : value;

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
            set => _buildTime = value;
        }
        

        public List<ItemCount> PriceToUpgrade
        {
            get
            {
                if (_currentLevel == 0)
                {
                    return _priceToUpgrade.ToList();
                }

                List<ItemCount> newPrice = new();
                foreach (var price in _priceToUpgrade)
                {
                    var a = price.count - 1;
                    float p = 0;
                    for (int i = 2; i <= _currentLevel + 1; i++)
                    {
                        p += (Mathf.Pow(i, 4) + ((a * i) - Mathf.Pow(i, 3)))/i;
                        a = (int)p;
                    }
                        
                    newPrice.Add(new ItemCount
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
            set
            {
                _currentLevel = value;
                if (_currentLevel == 1 && _taskData != null)
                {
                    _taskData.AccessDone = true;
                    if (_taskData.TaskStatus == TaskStatus.InProcess)
                    {
                        TaskManager.taskChangeStatusCallback?.Invoke(_taskData, TaskStatus.TakeReward);
                    }
                }
            }
        }

        [SerializeField] private ItemData _itemType;

        [SerializeField] private int _count;

        [SerializeField] private int _maxStorageCount;

        [SerializeField] private int _currentLevel;

        [SerializeField] private ItemCount[] _priceToUpgrade;
        
        [SerializeField] private float _buildTime;
        public void Save()
        {
            //SaveLoadSystem.SaveData(DynamicData);
        }

        public void Load()
        {
            // var data = SaveLoadSystem.LoadData(DynamicData);
            // if (data != null)
            // {
            //     DynamicData = data;
            // }
        }

       
    }

    [Serializable]
    public class BuildingVisualSprites
    {
        public Sprite buildingsSprites, shadowSprites;
    }

}
