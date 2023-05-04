using System.Collections.Generic;
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
        
        public List<PriceToUpgrade> currentItemsCount = new();
        public List<PriceToUpgrade> priceToUpgradeCraftingTable = new();
        public int craftingTime;

        public void Setup(List<PriceToUpgrade> price, int time)
        {
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
            SaveLoadSystem.SaveData(this);
        }

        public void Load()
        {
            var data = SaveLoadSystem.LoadData(this) as CraftingTableData;
            if (data != null)
            {
                currentLevel = data.currentLevel;
                currentItemsCount = data.currentItemsCount;
                priceToUpgradeCraftingTable = data.priceToUpgradeCraftingTable;
                craftingTime = data.craftingTime;
            }
        }
    }

}
