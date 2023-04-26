using System;
using System.Collections.Generic;
using System.Linq;

namespace Vikings.Building
{
    public class BuildingController : AbstractBuilding
    {
        public Action<BuildingData> OnChangeCount;
        
        public override void ChangeStorageCount(PriceToUpgrade price)
        {
            var item = buildingData.PriceToUpgrades.FirstOrDefault(x => x.itemData.ID == price.itemData.ID);
            var currentItem = buildingData.currentItemsCount.FirstOrDefault(x => x.itemData.ID == price.itemData.ID);
            if (price.count + currentItem.count <= item.count)
            {
                currentItem.count += price.count;
            }
            else
            {
                currentItem.count = item.count;
            }
            
            OnChangeCount?.Invoke(buildingData);

            if (IsFullStorage())
            {
                UpgradeStorage();
            }
        }

        public override void Init(BuildingData buildingData)
        {
            this.buildingData = buildingData;
        }

        public override bool IsFullStorage()
        {
            for (int i = 0; i < buildingData.PriceToUpgrades.Length; i++)
            {
                if (buildingData.PriceToUpgrades[i].count > buildingData.currentItemsCount[i].count)
                {
                    return false;
                }
            }
            buildingData.IsBuild = true;
            
            return true;
        }

        public override PriceToUpgrade[] GetCurrentPriceToUpgrades()
        {
            List<PriceToUpgrade> price = new();
            for (int i = 0; i < buildingData.PriceToUpgrades.Length; i++)
            {
                if (buildingData.PriceToUpgrades[i].count > buildingData.currentItemsCount[i].count)
                {
                    price.Add( buildingData.currentItemsCount[i]);
                }
            }

            return price.ToArray();
        }
        
        public override void UpgradeStorage() { }
        public override void SetUpgradeState() { }
    }
}