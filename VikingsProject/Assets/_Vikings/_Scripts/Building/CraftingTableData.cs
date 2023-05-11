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
        public int currentWeaponId;
        
        public List<PriceToUpgrade> currentItemsCount = new();
        public List<PriceToUpgrade> priceToUpgradeCraftingTable = new();
        public int craftingTime;
        
        private PriceToUpgrade[] _currentItemsCountArray;
        private PriceToUpgrade[] _priceToUpgradeCraftingTableArray;

        public void Setup(List<PriceToUpgrade> price, int time, int weaponId)
        {
            currentWeaponId = weaponId;
            
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
            craftingTime = time;
        }

        public void Clear()
        {
            currentItemsCount.Clear();
            craftingTime = 0;
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
                craftingTime = data.craftingTime;
            }
        }
    }

}
