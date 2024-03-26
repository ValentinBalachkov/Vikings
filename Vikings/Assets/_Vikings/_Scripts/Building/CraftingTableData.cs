using System.Collections.Generic;
using System.Linq;
using Vikings.SaveSystem;
using UnityEngine;

namespace Vikings.Building
{
    [CreateAssetMenu(fileName = "CraftingTableData", menuName = "Data/CraftingTableData", order = 6)]
    public class CraftingTableData : ScriptableObject, IData
    {
        public bool isOpen;
        public int currentLevel
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
        private int _currentLevel;
        public Sprite icon;
        public Sprite iconOfflineFarm;
        public string nameText;
        public string description;
        public string required;
        public int currentWeaponId;
        public int tableBuildingTime;
        public int priority;
        public bool isUpgrade;

        //[SerializeField] private BuildingData _buildingData;
        [SerializeField] private TaskData _taskData;
        
        public List<ItemCount> currentItemsCount = new();
        public List<ItemCount> priceToUpgradeCraftingTable = new();

        public List<ItemCount> currentItemsPriceToUpgrade = new();
        public List<ItemCount> PriceToUpgrade
        {
            get
            {
                // if (currentLevel == 0)
                // {
                //     return _buildingData.PriceToUpgrades.ToList();
                // }

                List<ItemCount> newPrice = new();
                // foreach (var price in _buildingData.PriceToUpgrades)
                // {
                //     var a = price.count - 1;
                //     float p = 0;
                //     for (int i = 2; i <= currentLevel + 1; i++)
                //     {
                //         p += (Mathf.Pow(i, 4) + ((a * i) - Mathf.Pow(i, 3)))/i;
                //         a = (int)p;
                //     }
                //         
                //     newPrice.Add(new ItemCount
                //     {
                //         count = (int)p,
                //         itemData = price.itemData
                //     });
                // }

                return newPrice;
            }
        }

        public float TableBuildingTime
        {
            get
            {
                if (currentLevel == 0)
                {
                    return tableBuildingTime;
                }
                return (0.5f * Mathf.Pow(currentLevel + 1, 2)) + tableBuildingTime;
            }
        }
        

        public int craftingTime;
        private int _weaponLevel;

        private ItemCount[] _currentItemsCountArray;
        private ItemCount[] _priceToUpgradeCraftingTableArray;

        public void Setup(List<ItemCount> price, int time, int weaponLevel, int weaponId)
        {
            currentWeaponId = weaponId;
            _weaponLevel = weaponLevel;
            
            priceToUpgradeCraftingTable.Clear();
            currentItemsCount.Clear();
            
            foreach (var item in price)
            {
                priceToUpgradeCraftingTable.Add(new ItemCount()
                {
                    count = item.count,
                    itemData = item.itemData
                });
            }
            
            foreach (var item in price)
            {
                currentItemsCount.Add(new ItemCount()
                {
                    count = 0,
                    itemData = item.itemData
                });
            }
            craftingTime = time;
        }

        public void Clear()
        {
            currentItemsCount.Clear();
            craftingTime = 0;
        }

        public void Save()
        {
            // _currentItemsCountArray = currentItemsCount.ToArray();
            // _priceToUpgradeCraftingTableArray = priceToUpgradeCraftingTable.ToArray();
            // SaveLoadSystem.SaveData(this);
        }

        public void Load()
        {
            // var data = SaveLoadSystem.LoadData(this) as CraftingTableData;
            // if (data != null)
            // {
            //     _currentLevel = data._currentLevel;
            //     isUpgrade = data.isUpgrade;
            //     if (data._currentItemsCountArray != null)
            //     {
            //         currentItemsCount.Clear();
            //         priceToUpgradeCraftingTable.Clear();
            //         currentItemsCount = data._currentItemsCountArray.ToList();
            //         priceToUpgradeCraftingTable = data._priceToUpgradeCraftingTableArray.ToList();
            //     }
            //     craftingTime = data.craftingTime;
            // }
        }
    }

}
