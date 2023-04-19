using System;
using System.Linq;

namespace Vikings.Building
{
    public class BuildingController : AbstractBuilding
    {
        public Action<BuildingData> OnChangeCount;


        private StorageOnMap _storageOnMap;

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

        public override void Init(BuildingData buildingData, StorageOnMap storageOnMap)
        {
            this.buildingData = buildingData;
            _storageOnMap = storageOnMap;
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

        public override void UpgradeStorage()
        {
            _storageOnMap.UpgradeBuildingToStorage(buildingData);
        }
    }
}