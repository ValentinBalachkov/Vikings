using System;
using UnityEngine;

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
    }
}