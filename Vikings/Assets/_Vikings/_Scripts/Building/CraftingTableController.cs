using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vikings.Weapon;

namespace Vikings.Building
{
    public class CraftingTableController : AbstractBuilding
    {
        public GameObject Model => _model;
        public Action<ItemCount[], ItemCount[]> OnChangeCount;
        public CraftingTableData CraftingTableData => _craftingTableData;
        [SerializeField] private Sprite[] _buildingsSprites, _shadowsRenders;
        [SerializeField] private SpriteRenderer _spriteBuilding, _shadowRender;
        
        [SerializeField] private AudioSource _audioSourceToStorage;
        
        [SerializeField] private CraftingTableData _craftingTableData;
        [SerializeField] private GameObject _model;
        

        private WeaponData _currentWeapon;
        private bool _isUpgradeWeapon;

        public void SetupCraftWeapon(WeaponData weaponData, bool isUpgrade)
        {
            _isUpgradeWeapon = isUpgrade;
            _currentWeapon = weaponData;
            _craftingTableData.Setup(_currentWeapon.PriceToBuy, (int)_currentWeapon.CraftingTime, _currentWeapon.level, weaponData.id);
            CollectingResourceView.Instance.Setup(weaponData.nameText, _craftingTableData.currentItemsCount.ToArray(), _craftingTableData.priceToUpgradeCraftingTable.ToArray(), transform);
        }
        
        public override void SetUpgradeState()
        {
            for (int i = 0; i < _craftingTableData.PriceToUpgrade.Count; i++)
            {
                _craftingTableData.currentItemsPriceToUpgrade[i].count = 0;
            }

            CollectingResourceView.Instance.Setup(_craftingTableData.nameText,_craftingTableData.currentItemsPriceToUpgrade.ToArray(), 
                _craftingTableData.PriceToUpgrade.ToArray(), transform);

            _craftingTableData.isUpgrade = true;
        }

        public override bool GetUpgradeState()
        {
            return _craftingTableData.isUpgrade;
        }

        public override void SetUpgradeState(bool isUpgrade)
        {
            _craftingTableData.isUpgrade = isUpgrade;
        }

        public override void ChangeStorageCount(ItemCount price)
        {
            _audioSourceToStorage.Play();
            if (_craftingTableData.isUpgrade)
            {
                var itemUpg = _craftingTableData.currentItemsPriceToUpgrade.FirstOrDefault(x => x.itemData.ID == price.itemData.ID);
                var defaultItemUpg = _craftingTableData.PriceToUpgrade.FirstOrDefault(x => x.itemData.ID == price.itemData.ID);

                if (itemUpg.count + price.count >= defaultItemUpg.count)
                {
                    itemUpg.count = defaultItemUpg.count;
                }
                else
                {
                    itemUpg.count += price.count;
                }
                CollectingResourceView.Instance.UpdateView(_craftingTableData.currentItemsPriceToUpgrade.ToArray(), _craftingTableData.PriceToUpgrade.ToArray());
                return;
            }
            
            var item = _currentWeapon.PriceToBuy.FirstOrDefault(x => x.itemData.ID == price.itemData.ID);
            var currentItem = _craftingTableData.currentItemsCount.FirstOrDefault(x => x.itemData.ID == price.itemData.ID);
            if (price.count + currentItem.count <= item.count)
            {
                currentItem.count += price.count;
            }
            else
            {
                currentItem.count = item.count;
            }
            
            CollectingResourceView.Instance.UpdateView(_craftingTableData.currentItemsCount.ToArray(), _currentWeapon.PriceToBuy.ToArray());
            OnChangeCount?.Invoke(_currentWeapon.PriceToBuy.ToArray(), _craftingTableData.currentItemsCount.ToArray());

        }

        public void OpenCurrentWeapon()
        {
            if (_isUpgradeWeapon)
            {
                _currentWeapon.level++;
                _isUpgradeWeapon = false;
            }

            else if (!_craftingTableData.isUpgrade)
            {
                _currentWeapon.level = 1;
                _currentWeapon.IsOpen = true;
                _currentWeapon.ItemData.IsOpen = true;
            }
            else
            {
                UpgradeStorage();
            }
            
            _craftingTableData.Clear();
        }

        public override void Init(BuildingData buildingData , bool isSaveInit = false)
        {
            if (!isSaveInit)
            {
                _craftingTableData.currentLevel++;
            }
            
            SetupSprite(_craftingTableData.currentLevel);
        }

        public override bool IsFullStorage()
        {
            if (_craftingTableData.isUpgrade)
            {
                for (int i = 0; i < _craftingTableData.PriceToUpgrade.Count; i++)
                {
                    if ( _craftingTableData.PriceToUpgrade[i].count > _craftingTableData.currentItemsPriceToUpgrade[i].count)
                    {
                        return false;
                    }
                }
            
                CollectingResourceView.Instance.gameObject.SetActive(false);
                return true;
            }
            
            if (_currentWeapon == null || _craftingTableData.currentItemsCount.Count == 0)
            {
                return true;
            }
            
            for (int i = 0; i < _currentWeapon.PriceToBuy.Count; i++)
            {
                if (_currentWeapon.PriceToBuy[i].count > _craftingTableData.currentItemsCount[i].count)
                {
                    return false;
                }
            }
            CollectingResourceView.Instance.gameObject.SetActive(false);
            return true;
        }

        public override void UpgradeStorage()
        {
            _craftingTableData.currentLevel++;
            SetupSprite(_craftingTableData.currentLevel);
            _craftingTableData.isUpgrade = false;
        }

        public override ItemCount[] GetCurrentPriceToUpgrades()
        {
            if (_craftingTableData.isUpgrade)
            {
                List<ItemCount> priceUpgrade = new();
                for (int i = 0; i < _craftingTableData.PriceToUpgrade.Count; i++)
                {
                    if (_craftingTableData.PriceToUpgrade[i].count > _craftingTableData.currentItemsPriceToUpgrade[i].count)
                    {
                        priceUpgrade.Add(_craftingTableData.currentItemsPriceToUpgrade[i]);
                    }
                }
                
                return priceUpgrade.ToArray();
            }
            
            List<ItemCount> price = new();
            for (int i = 0; i < _currentWeapon.PriceToBuy.Count; i++)
            {
                if (_currentWeapon.PriceToBuy[i].count > _craftingTableData.currentItemsCount[i].count)
                {
                    price.Add(_craftingTableData.currentItemsCount[i]);
                }
            }

            return price.ToArray();
        }

        private void SetupSprite(int lvl)
        {
            _spriteBuilding.sprite = lvl switch
            {
                1 => _buildingsSprites[0],
                2 => _buildingsSprites[1],
                3 => _buildingsSprites[2],
                _ => _buildingsSprites[2]
            };
        }
    }
}