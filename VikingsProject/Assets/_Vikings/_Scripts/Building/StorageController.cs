using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vikings.Items;


namespace Vikings.Building
{
    public class StorageController : AbstractBuilding, IGetItem
    {
        public Action<ItemData> OnChangeCountStorage;
        public Action<int, int> OnUpgradeStorage;
        public int Priority { get; set; }

        private List<PriceToUpgrade> _currentItemsForUpgrade = new();
        [SerializeField] private CollectingResourceView _collectingResourceView;
       


        public override void Init(BuildingData buildingData, bool isSaveInit = false)
        {
            Priority = 0;
            this.buildingData = buildingData;
            if (!isSaveInit)
            {
                buildingData.StorageData.CurrentLevel++;
            }
        }

        public override void SetUpgradeState()
        {
            _currentItemsForUpgrade.Clear();
            
            foreach (var item in buildingData.StorageData.PriceToUpgrade)
            {
                _currentItemsForUpgrade.Add(new PriceToUpgrade
                {
                    count = 0,
                    itemData = item.itemData
                });
                
            }

            _collectingResourceView.Setup(buildingData.StorageData.nameText);
            isUpgradeState = true;
        }

        public bool IsAvailableToGetItem()
        {
            return buildingData.StorageData.Count >= buildingData.StorageData.ItemType.DropCount;
        }

        public override void ChangeStorageCount(PriceToUpgrade priceToUpgrade)
        {
            if (!isUpgradeState)
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
                return;
            }

            var item = _currentItemsForUpgrade.FirstOrDefault(x => x.itemData.ID == priceToUpgrade.itemData.ID);
            var defaultItem = buildingData.StorageData.PriceToUpgrade.FirstOrDefault(x => x.itemData.ID == priceToUpgrade.itemData.ID);

            if (item.count + priceToUpgrade.count >= defaultItem.count)
            {
                item.count = defaultItem.count;
            }
            else
            {
                item.count += priceToUpgrade.count;
            }
            
            _collectingResourceView.UpdateView(_currentItemsForUpgrade.ToArray(), buildingData.StorageData.PriceToUpgrade.ToArray());
        }

        public override void UpgradeStorage()
        {
            buildingData.StorageData.MaxStorageCount += 10;
            isUpgradeState = false;
            _collectingResourceView.gameObject.SetActive(false);
            buildingData.StorageData.CurrentLevel++;
        }

        public override PriceToUpgrade[] GetCurrentPriceToUpgrades()
        {
            if (!isUpgradeState)
            {
                PriceToUpgrade price = new PriceToUpgrade()
                {
                    count = buildingData.StorageData.Count,
                    itemData = buildingData.StorageData.ItemType
                };

                PriceToUpgrade[] priceToUpgrades = { price };
                return priceToUpgrades;
            }

            return _currentItemsForUpgrade.ToArray();
        }

        public override bool IsFullStorage()
        {
            if (!isUpgradeState)
            {
                bool isFullStorage = buildingData.StorageData.Count >= buildingData.StorageData.MaxStorageCount;
                return isFullStorage;
            }

            for (int i = 0; i < buildingData.StorageData.PriceToUpgrade.Length; i++)
            {
                if (buildingData.StorageData.PriceToUpgrade[i].count > _currentItemsForUpgrade[i].count)
                {
                    return false;
                }
            }
            _collectingResourceView.gameObject.SetActive(false);
            return true;
        }

        public Transform GetItemPosition()
        {
            return transform;
        }

        public void TakeItem()
        {
            buildingData.StorageData.Count -= buildingData.StorageData.ItemType.DropCount;
            OnChangeCountStorage?.Invoke(buildingData.StorageData.ItemType);
        }

        public ItemData GetItemData()
        {
            return buildingData.StorageData.ItemType;
        }
    }
}