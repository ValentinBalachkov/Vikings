using UnityEngine;

namespace Vikings.Building
{
    public abstract class AbstractBuilding : MonoBehaviour
    {
        public StorageData StorageData => storageData;
        
        protected StorageData storageData;
        public abstract void ChangeStorageCount(PriceToUpgrade count);
        public abstract void Init(StorageData storageData, StorageOnMap storageOnMap);
        public abstract bool IsFullStorage();
        public abstract void UpgradeStorage();
    }
}