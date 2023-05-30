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
        public bool EnableToGet { get; set; }


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

            for (int i = 0; i < buildingData.StorageData.PriceToUpgrade.Count; i++)
            {
                buildingData.currentItemsCount[i].count = 0;
            }

            CollectingResourceView.Instance.Setup(buildingData.StorageData.nameText, buildingData.currentItemsCount,
                buildingData.StorageData.PriceToUpgrade.ToArray(), transform);
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

            var item = buildingData.currentItemsCount.FirstOrDefault(x => x.itemData.ID == priceToUpgrade.itemData.ID);
            var defaultItem =
                buildingData.StorageData.PriceToUpgrade.FirstOrDefault(x =>
                    x.itemData.ID == priceToUpgrade.itemData.ID);

            if (item.count + priceToUpgrade.count >= defaultItem.count)
            {
                item.count = defaultItem.count;
            }
            else
            {
                item.count += priceToUpgrade.count;
            }

            CollectingResourceView.Instance.UpdateView(buildingData.currentItemsCount,
                buildingData.StorageData.PriceToUpgrade.ToArray());
        }

        public override void UpgradeStorage()
        {
            isUpgradeState = false;

            buildingData.StorageData.CurrentLevel++;
            buildingData.StorageData.MaxStorageCount = (int)(Mathf.Pow(buildingData.StorageData.CurrentLevel, 5f)
                                                             + Mathf.Pow(buildingData.StorageData.CurrentLevel, buildingData.StorageData.CurrentLevel - 1)
                                                             + 20);
            CollectingResourceView.Instance.gameObject.SetActive(false);
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
            
            List<PriceToUpgrade> priceUpgrade = new();
            for (int i = 0; i < buildingData.StorageData.PriceToUpgrade.Count; i++)
            {
                if (buildingData.StorageData.PriceToUpgrade[i].count > buildingData.currentItemsCount[i].count)
                {
                    priceUpgrade.Add(buildingData.currentItemsCount[i]);
                }
            }

            return priceUpgrade.ToArray();
        }

        public override bool IsFullStorage()
        {
            if (!isUpgradeState)
            {
                bool isFullStorage = buildingData.StorageData.Count >= buildingData.StorageData.MaxStorageCount;
                return isFullStorage;
            }

            for (int i = 0; i < buildingData.StorageData.PriceToUpgrade.Count; i++)
            {
                if (buildingData.StorageData.PriceToUpgrade[i].count > buildingData.currentItemsCount[i].count)
                {
                    return false;
                }
            }

            CollectingResourceView.Instance.gameObject.SetActive(false);
            return true;
        }

        public Transform GetItemPosition()
        {
            return transform;
        }

        public void TakeItem()
        {
            buildingData.StorageData.Count -= buildingData.StorageData.ItemType.DropCount;
            EnableToGet = true;
            OnChangeCountStorage?.Invoke(buildingData.StorageData.ItemType);
        }

        public ItemData GetItemData()
        {
            return buildingData.StorageData.ItemType;
        }
    }
}