﻿using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Vikings.Building;
using Vikings.Weapon;

namespace Vikings.Chanacter
{
    public class InventoryController : MonoBehaviour
    {
        public Action OnCollect;
        public IGetItem CurrentItem => _currentItem;
        [SerializeField] private CharactersConfig _charactersConfig;
        [SerializeField] private WeaponData[] _weaponsData;

        [SerializeField] private AudioSource _audioSourceTree;
        [SerializeField] private AudioSource _audioSourceStone;
        [SerializeField] private AudioSource _audioSourceEat;
        [SerializeField] private AudioSource _audioSourceDefaultItem;
        
        
        private IGetItem _currentItem;


        private int _count;

        public void SetItem(IGetItem itemData)
        {
            _currentItem = itemData;
        }

        public void CollectItem()
        {
            var weapon = _weaponsData.FirstOrDefault(x => x.ItemData.ID == _currentItem.GetItemData().ID);
            if (weapon != null)
            {
                if (weapon.IsOpen && _currentItem.GetItemData().DropCount > 1)
                {
                    StartCoroutine(CollectItemsCoroutine(weapon));
                    return;
                }
            }
            
            StartCoroutine(CollectItemsCoroutine());
        }
        
        public PriceToUpgrade SetItemToStorage()
        {
            var price = new PriceToUpgrade
            {
                count = _count,
                itemData = _currentItem.GetItemData()
            };
            _currentItem = null;
            _count = 0;
            return price;
        }

        private IEnumerator CollectItemsCoroutine()
        {
            var item = _currentItem.GetItemData();
            var currentAudioSource = _currentItem.GetItemData().ID == 1 ? _audioSourceEat : _audioSourceDefaultItem;
            for (int i = 0; i < item.CollectTime; i++)
            {
                currentAudioSource.Play();
                yield return new WaitForSeconds(1f);
            }
            
            //yield return new WaitForSeconds(item.CollectTime);
            _count = _charactersConfig.ItemsCount < item.DropCount ? _charactersConfig.ItemsCount : item.DropCount;
            _currentItem.TakeItem();
            OnCollect?.Invoke();
        }
        
        private IEnumerator CollectItemsCoroutine(WeaponData weaponData)
        {
            var item = _currentItem.GetItemData();
            
            for (int i = 0; i < item.CollectTime; i++)
            {
                yield return new WaitForSeconds(1f);

                if (weaponData.ItemData.ID == 3)
                {
                    _audioSourceTree.Play();
                }
                else if (weaponData.ItemData.ID == 2)
                {
                    _audioSourceStone.Play();
                }
                
                if (_charactersConfig.ItemsCount > _count && _count < item.DropCount)
                {
                    _count += weaponData.level;
                }
                else
                {
                    if (_charactersConfig.ItemsCount <= _count)
                    {
                        _count = _charactersConfig.ItemsCount;
                    }
                    else if(_count >= item.DropCount)
                    {
                        _count = item.DropCount;
                    }
                    
                    break;
                }
            }
            
            _currentItem.TakeItem();
            OnCollect?.Invoke();
        }
    }
}