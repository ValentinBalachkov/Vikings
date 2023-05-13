using System.Collections.Generic;
using UnityEngine;
using Vikings.Building;
using Vikings.Weapon;

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
        [SerializeField] private WeaponsOnMapController _weaponsOnMapController;

        private List<MenuElement> _menuElements = new();


        private void OnEnable()
        {
            Spawn();
        }

        private void OnDisable()
        {
            foreach (var element in _menuElements)
            {
                Destroy(element.gameObject);
            }

            _menuElements.Clear();
        }


        private void Spawn()
        {
            for (int i = 0; i < _storageDatas.Count; i++)
            {
                int k = 1;
                if (i == 0)
                {
                    k = 0;
                }

                var item = Instantiate(_menuElement, _content);
                _menuElements.Add(item);
                item.UpdateUI(_storageDatas[i].nameText, _storageDatas[i].description, _storageDatas[i].CurrentLevel + 1, _storageDatas[i].icon,
                    _storageDatas[i].PriceToUpgrade);
                item.SetButtonDescription(_storageDatas[i].CurrentLevel == 0);
                item.SetEnable((_craftingTableData.currentLevel - _storageDatas[i].CurrentLevel == k) ||
                               (_storageDatas[i].isDefaultOpen && _storageDatas[i].CurrentLevel == 0));
                var index = i;
                if (_storageDatas[i].CurrentLevel == 0)
                {
                    item.AddOnClickListener(() =>
                    {
                        _buildingsOnMap.SpawnStorage(index);
                        gameObject.SetActive(false);
                    });
                }
                else
                {
                    item.AddOnClickListener(() =>
                    {
                        _buildingsOnMap.SetStorageUpgradeState(_storageDatas[i].ItemType);
                        gameObject.SetActive(false);
                    });
                }
            }


            var craftingTable = Instantiate(_menuElement, _content);
            _menuElements.Add(craftingTable);
            craftingTable.UpdateUI(_craftingTableData.nameText, _craftingTableData.description, _craftingTableData.currentLevel + 1,
                _craftingTableData.icon, _craftingTableBuildingData.PriceToUpgrades);
            craftingTable.SetButtonDescription(_craftingTableData.currentLevel == 0);
            craftingTable.SetEnable(_weaponsOnMapController.WeaponsData[0].IsOpen);
            if (_craftingTableData.currentLevel == 0)
            {
                craftingTable.AddOnClickListener(() =>
                {
                    _buildingsOnMap.SpawnStorage(3);
                    gameObject.SetActive(false);
                });
            }
            else
            {
                craftingTable.AddOnClickListener(() =>
                {
                    _buildingsOnMap.SetCraftingTableToUpgrade();
                    gameObject.SetActive(false);
                });
            }


            for (int i = 0; i < _weaponsOnMapController.WeaponsData.Count; i++)
            {
                var weapon = Instantiate(_menuElement, _content);
                _menuElements.Add(weapon);
                weapon.UpdateUI(_weaponsOnMapController.WeaponsData[i].nameText,
                    _weaponsOnMapController.WeaponsData[i].description, 1,
                    _weaponsOnMapController.WeaponsData[i].icon,
                    _weaponsOnMapController.WeaponsData[i].PriceToBuy.ToArray());
                weapon.SetButtonDescription(!_weaponsOnMapController.WeaponsData[i].IsOpen);
                weapon.SetEnable(((i == 0 && _storageDatas[0].CurrentLevel > 0) ||
                                 (i == 1 && _craftingTableData.currentLevel == 2)) && !_weaponsOnMapController.WeaponsData[i].IsOpen);
                var index = i;
                weapon.AddOnClickListener(() =>
                {
                    _weaponsOnMapController.StartCraftWeapon(index);
                    gameObject.SetActive(false);
                });
            }
        }
    }
}