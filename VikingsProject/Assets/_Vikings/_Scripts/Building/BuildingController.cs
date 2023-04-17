using System.Linq;
using UnityEngine;

namespace Vikings.Building
{
    public class BuildingController : AbstractBuilding
    {
        [SerializeField] private BuildingData _buildingData;
        private StorageOnMap _storageOnMap;

        public override void ChangeStorageCount(PriceToUpgrade price)
        {
            var item = _buildingData.PriceToUpgrades.FirstOrDefault(x => x.itemData.ID == price.itemData.ID);
            var currentItem = _buildingData.currentItemsCount.FirstOrDefault(x => x.itemData.ID == price.itemData.ID);
            if (price.count + currentItem.count <= item.count)
            {
                currentItem.count += price.count;
            }
            else
            {
                currentItem.count = item.count;
            }
        }

        public override void Init(StorageData storageData, StorageOnMap storageOnMap)
        {
            this.storageData = storageData;
            _storageOnMap = storageOnMap;
        }

        public override bool IsFullStorage()
        {
            for (int i = 0; i < _buildingData.PriceToUpgrades.Length; i++)
            {
                if (_buildingData.PriceToUpgrades[i].count > _buildingData.currentItemsCount[i].count)
                {
                    return false;
                }
            }
            _buildingData.IsBuild = true;
            return true;
        }

        public override void UpgradeStorage()
        {
            
        }
    }
}