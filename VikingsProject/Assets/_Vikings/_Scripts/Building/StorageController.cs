using System;
using System.Linq;
using Vikings.Items;


namespace Vikings.Building
{
    public class StorageController : AbstractBuilding
    {
        public Action<ItemData> OnChangeCountStorage;
        public Action<int, int> OnUpgradeStorage;

        
        private StorageOnMap _storageOnMap;

        public override void Init(BuildingData buildingData, StorageOnMap storageOnMap)
        {
            this.buildingData = buildingData;
            _storageOnMap = storageOnMap;
        }
        
        
        public override void ChangeStorageCount(PriceToUpgrade priceToUpgrade)
        {
            if (buildingData.StorageData.Count + priceToUpgrade.count > buildingData.StorageData.MaxStorageCount)
            {
                buildingData.StorageData.Count = buildingData.StorageData.MaxStorageCount;
            }
            else
            {
                buildingData.StorageData.Count += priceToUpgrade.count;
            }
            
            OnChangeCountStorage?.Invoke(buildingData.StorageData.ItemType);
        }

        public override void UpgradeStorage()
        {
            if (buildingData.StorageData.CurrentLevel >= 3)
            {
                return;
            }
            
            foreach (var upgradePrice in buildingData.StorageData.PriceToUpgrade)
            {
                var storage = _storageOnMap.StorageControllers.FirstOrDefault(x => x.BuildingData.StorageData.ItemType.ID == upgradePrice.itemData.ID);
                if (storage != null && upgradePrice.count <= storage.BuildingData.StorageData.Count)
                {
                    continue;
                }

                return;
            }
            
            foreach (var item in buildingData.StorageData.PriceToUpgrade)
            {
                var storage = _storageOnMap.StorageControllers.FirstOrDefault(x => x.BuildingData.StorageData.ItemType.ID == item.itemData.ID);
                if (storage != null) storage.ChangeStorageCount( new PriceToUpgrade{count = -item.count, itemData = item.itemData} );
            }
            
            buildingData.StorageData.MaxStorageCount += 10;
            buildingData.StorageData.CurrentLevel++;
            OnUpgradeStorage?.Invoke(buildingData.StorageData.MaxStorageCount, buildingData.StorageData.CurrentLevel);
        }

        public override bool IsFullStorage()
        {
            return buildingData.StorageData.Count >= buildingData.StorageData.MaxStorageCount;
        }
        
    }
}