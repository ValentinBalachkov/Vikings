using System.Collections.Generic;
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
        [SerializeField] private GameObject[] _interface;
        

        public void StartCraftWeapon(int index)
        {
            _buildingsOnMap.GetCraftingTable().SetupCraftWeapon(_weaponsList[index]);
            _buildingsOnMap.UpdateCurrentBuilding(true);
            foreach (var item in _interface)
            {
                item.SetActive(false);
            }
        }
        
        private void Start()
        {
            foreach (var weapon in _weaponsList)
            {
                weapon.OnOpen += OnOpenWeapon;
            }
        }

        private void OnOpenWeapon(WeaponData weaponData)
        {
            _itemsOnMapController.AddItemToItemsList(weaponData.ItemData);
            foreach (var item in _interface)
            {
                item.SetActive(true);
            }
        }
        
    }
}

