using UnityEngine;

namespace Vikings.Building
{
    public abstract class AbstractBuilding : MonoBehaviour
    {
        public BuildingData BuildingData => buildingData;
        
        protected BuildingData buildingData;
        public abstract void ChangeStorageCount(PriceToUpgrade count);
        public abstract void Init(BuildingData buildingData);
        public abstract bool IsFullStorage();
        public abstract void UpgradeStorage();

        public abstract PriceToUpgrade[] GetCurrentPriceToUpgrades();
        
        public bool isUpgradeState;
    }
}