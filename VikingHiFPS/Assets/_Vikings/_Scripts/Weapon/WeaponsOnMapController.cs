using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vikings.Building;
using Vikings.Items;

namespace Vikings.Weapon
{
    public class WeaponsOnMapController : MonoBehaviour
    {
        public List<WeaponData> WeaponsData => _weaponsList;
        [SerializeField] private ItemsOnMapController _itemsOnMapController;
        [SerializeField] private List<WeaponData> _weaponsList = new();
        [SerializeField] private BuildingsOnMap _buildingsOnMap;
        [SerializeField] private MenuButtonsManager _menuButtonsManager;
        
        [SerializeField] private CraftingTableData _craftingTableData;
        [SerializeField] private CraftingTableData _craftingTableDataDefault;


        private void Start()
        {
            foreach (var weapon in _weaponsList)
            {
                weapon.OnOpen += OnOpenWeapon;
            }
        }

        public void CheckForCraft()
        {
            int weaponId = 0;
            
            if (_craftingTableData.currentItemsCount.Count > 0)
            {
                weaponId = _craftingTableData.currentWeaponId;
            }
            else if( _craftingTableDataDefault.currentItemsCount.Count > 0)
            {
                weaponId = _craftingTableDataDefault.currentWeaponId;
            }

            if (_craftingTableData.currentItemsCount.Count > 0 || _craftingTableDataDefault.currentItemsCount.Count > 0)
            {
                _buildingsOnMap.GetCraftingTable().SetupCraftWeapon(_weaponsList.FirstOrDefault(x => x.id == weaponId));
                _buildingsOnMap.UpdateCurrentBuilding(true);
                _menuButtonsManager.EnableButtons(false);
            }
        }

        public void StartCraftWeapon(int index)
        {
            _buildingsOnMap.GetCraftingTable().SetupCraftWeapon(_weaponsList[index]);
            _buildingsOnMap.UpdateCurrentBuilding(true);
            _menuButtonsManager.EnableButtons(false);
        }

        private void OnOpenWeapon(WeaponData weaponData)
        {
            _itemsOnMapController.AddItemToItemsList(weaponData.ItemData);
            _menuButtonsManager.EnableButtons(true);
        }
        
    }
}

