using System;
using _Vikings.WeaponObject;
using UnityEngine;
using Vikings.Object;

namespace _Vikings._Scripts.Refactoring
{
    public class EatStorage : Storage
    {
        public event Action OnHomeBuilding;
        public override void Upgrade()
        {
            base.Upgrade();
            OnHomeBuilding?.Invoke();
        }

        public void SudoChangeCount(int value)
        {
            if (MaxStorageCount < value + _storageDynamicData.Count)
            {
                _storageDynamicData.Count = MaxStorageCount;
            }
            else if(value + _storageDynamicData.Count < 0)
            {
                _storageDynamicData.Count = 0;
            }
            else
            {
                _storageDynamicData.Count += value;
            }
           
            _inventoryView.UpdateUI(_storageDynamicData.Count, MaxStorageCount, ResourceType);
            
            if (value < 0)
            {
                DebugLogger.SendMessage($"{StorageNeedItem.Method.Name}", Color.blue);
                StorageNeedItem?.Invoke(this);
            }
            
        }

        public override (bool, int, Sprite) IsEnableToBuild<T1, T2>(T1 arg1, T2 arg2)
        {
            var craftingTable = arg1 as AbstractBuilding;
            var weapon = arg2 as Weapon;

            Sprite sprite = _storageData.requiredSprite;

            int level = craftingTable.CurrentLevel.Value + 1;
            
            bool isEnable;

            if (weapon.Level.Value == 0)
            {
                sprite = weapon.GetWeaponData().icon;
                isEnable = CurrentLevel.Value == 0 && weapon.Level.Value > 0;
                level = weapon.Level.Value + 1;
                return (isEnable, level, sprite);
            }
            
            if (CurrentLevel.Value >= 5)
            {
                isEnable = false;
            }
            else
            {
                isEnable = CurrentLevel.Value == 0 && weapon.Level.Value > 0 || craftingTable.CurrentLevel.Value - CurrentLevel.Value >= 1 && CurrentLevel.Value < 5;
            }
            
            return (isEnable, level, sprite);
        }
    }
}