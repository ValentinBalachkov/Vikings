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

        public override void Init(StorageData storageData, StorageOnMap storageOnMap)
        {
            base.storageData = storageData;
            _storageOnMap = storageOnMap;
        }
        
        
        public override void ChangeStorageCount(PriceToUpgrade priceToUpgrade)
        {
            if (storageData.Count + priceToUpgrade.count > storageData.MaxStorageCount)
            {
                storageData.Count = storageData.MaxStorageCount;
            }
            else
            {
                storageData.Count += priceToUpgrade.count;
            }
            
            OnChangeCountStorage?.Invoke(storageData.ItemType);
        }

        public override void UpgradeStorage()
        {
            if (storageData.CurrentLevel >= 3)
            {
                return;
            }
            
            foreach (var upgradePrice in storageData.PriceToUpgrade)
            {
                var storage = _storageOnMap.StorageControllers.FirstOrDefault(x => x.StorageData.ItemType.ID == upgradePrice.itemData.ID);
                if (storage != null && upgradePrice.count <= storage.StorageData.Count)
                {
                    continue;
                }

                return;
            }
            
            foreach (var item in storageData.PriceToUpgrade)
            {
                var storage = _storageOnMap.StorageControllers.FirstOrDefault(x => x.StorageData.ItemType.ID == item.itemData.ID);
                if (storage != null) storage.ChangeStorageCount( new PriceToUpgrade{count = -item.count, itemData = item.itemData} );
            }
            
            storageData.MaxStorageCount += 10;
            storageData.CurrentLevel++;
            OnUpgradeStorage?.Invoke(storageData.MaxStorageCount, storageData.CurrentLevel);
        }

        public override bool IsFullStorage()
        {
            return storageData.Count >= storageData.MaxStorageCount;
        }
        
    }
}