using System.Collections.Generic;
using System.Linq;
using SecondChanceSystem.SaveSystem;
using UnityEngine;

namespace Vikings.Building
{
    [CreateAssetMenu(fileName = "CraftingTableData", menuName = "Data/CraftingTableData", order = 6)]
    public class CraftingTableData : ScriptableObject, IData
    {
        public bool isOpen;
        public int currentLevel;
        public Sprite icon;
        public string nameText;
        public string description;
        public string required;
        public int currentWeaponId;
        public int tableBuildingTime;
        
        public List<PriceToUpgrade> currentItemsCount = new();
        public List<PriceToUpgrade> priceToUpgradeCraftingTable = new();
        
        public List<PriceToUpgrade> PriceToUpgradeCraftingTable
        {
            get
            {
                if (currentLevel == 0)
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
                        p += Mathf.Pow(i, 4) + ((a * i) - Mathf.Pow(i, 3));
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

        public float WeaponCraftingTime
        {
            get
            {
                if (_weaponLevel == 0)
                {
                    return _craftingTime;
                }
                return (0.5f * Mathf.Pow(_weaponLevel + 1, 2)) + _craftingTime;
            }
        }

        private int _craftingTime;
        private int _weaponLevel;

        private PriceToUpgrade[] _currentItemsCountArray;
        private PriceToUpgrade[] _priceToUpgradeCraftingTableArray;

        public void Setup(List<PriceToUpgrade> price, int time, int weaponLevel, int weaponId)
        {
            currentWeaponId = weaponId;
            _weaponLevel = weaponLevel;
            
            foreach (var item in price)
            {
                priceToUpgradeCraftingTable.Add(new PriceToUpgrade()
                {
                    count = item.count,
                    itemData = item.itemData
                });
            }
            
            foreach (var item in price)
            {
                currentItemsCount.Add(new PriceToUpgrade()
                {
                    count = 0,
                    itemData = item.itemData
                });
            }
            _craftingTime = time;
        }

        public void Clear()
        {
            currentItemsCount.Clear();
            _craftingTime = 0;
        }

        public void Save()
        {
            _currentItemsCountArray = currentItemsCount.ToArray();
            _priceToUpgradeCraftingTableArray = priceToUpgradeCraftingTable.ToArray();
            SaveLoadSystem.SaveData(this);
        }

        public void Load()
        {
            var data = SaveLoadSystem.LoadData(this) as CraftingTableData;
            if (data != null)
            {
                currentLevel = data.currentLevel;
                if (data._currentItemsCountArray != null)
                {
                    currentItemsCount = data._currentItemsCountArray.ToList();
                    priceToUpgradeCraftingTable = data._priceToUpgradeCraftingTableArray.ToList();
                }
                _craftingTime = data._craftingTime;
            }
        }
    }

}
