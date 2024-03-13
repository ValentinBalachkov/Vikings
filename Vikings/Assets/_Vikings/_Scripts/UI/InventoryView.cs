using System;
using PanelManager.Scripts.Panels;
using TMPro;
using UnityEngine;
using Vikings.Building;
using Vikings.Items;

namespace Vikings.UI
{
    public class InventoryView : ViewBase
    {
        public override PanelType PanelType => PanelType.Overlay;
        public override bool RememberInHistory => false;

        //  [SerializeField] private BuildingsOnMap _buildingsOnMap;
        [SerializeField] private InventoryViewTextData[] _itemsCountText;
        [SerializeField] private StorageData[] _storagesData;


        // private List<StorageController> _storageControllers = new();

        private void Start()
        {
            _itemsCountText[0].countText.text = $" {_storagesData[0].Count}/{_storagesData[0].MaxStorageCount}";
            _itemsCountText[1].countText.text = $" {_storagesData[1].Count}/{_storagesData[1].MaxStorageCount}";
            _itemsCountText[2].countText.text = $" {_storagesData[2].Count}/{_storagesData[2].MaxStorageCount}";
        }

        // public void AddStorageController(StorageController storageController)
        // {
        //     storageController.OnChangeCountStorage += UpdateUI;
        //     _storageControllers.Add(storageController);
        // }

        public void UpdateUI(ItemData itemData)
        {
            // var storage =
            //     _storageControllers.FirstOrDefault(x => x.BuildingData.StorageData.ItemType.ID == itemData.ID);
            // if(storage == null) return;
            //
            // var item = _itemsCountText.FirstOrDefault(x => x.item.ID == itemData.ID);
            //
            // item.countText.text = $" {storage.BuildingData.StorageData.Count}/{storage.BuildingData.StorageData.MaxStorageCount}";

            _itemsCountText[0].countText.text = $" {_storagesData[0].Count}/{_storagesData[0].MaxStorageCount}";
            _itemsCountText[1].countText.text = $" {_storagesData[1].Count}/{_storagesData[1].MaxStorageCount}";
            _itemsCountText[2].countText.text = $" {_storagesData[2].Count}/{_storagesData[2].MaxStorageCount}";
        }
    }

    [Serializable]
    public class InventoryViewTextData
    {
        public TMP_Text countText;
        public ItemData item;
    }
}