using System;
using System.Collections.Generic;
using UnityEngine;
using Vikings.Building;

namespace Vikings.UI
{
    public class CraftAndBuildingMenu : MonoBehaviour
    {
        [SerializeField] private Transform _content;
        [SerializeField] private MenuElement _menuElement;

        [SerializeField] private List<StorageData> _storageDatas;
        [SerializeField] private BuildingsOnMap _buildingsOnMap;
        [SerializeField] private CraftingTableData _craftingTableData;
        [SerializeField] private BuildingData _craftingTableBuildingData;


        private void Start()
        {
            Spawn();
        }

        private void Spawn()
        {
            for (int i = 0; i < _storageDatas.Count; i++)
            {
                var item = Instantiate(_menuElement, _content);
                item.UpdateUI(_storageDatas[i].nameText, _storageDatas[i].description, 0, _storageDatas[i].icon, _storageDatas[i].PriceToUpgrade);
                var index = i;
                item.AddOnClickListener(() =>
                {
                    _buildingsOnMap.SpawnStorage(index);
                    gameObject.SetActive(false);
                });
            }
            
            var craftingTable = Instantiate(_menuElement, _content);
            craftingTable.UpdateUI(_craftingTableData.nameText, _craftingTableData.description, 0, _craftingTableData.icon, _craftingTableBuildingData.PriceToUpgrades);
            craftingTable.AddOnClickListener(() => _buildingsOnMap.SpawnStorage(3));
        }
        
        
        
    
    }
}

