﻿using UnityEngine;

namespace Vikings.Building
{
    public abstract class AbstractBuilding : MonoBehaviour
    {
        public BuildingData BuildingData => buildingData;
        
        protected BuildingData buildingData;
        public abstract void ChangeStorageCount(ItemCount count);
        public abstract void Init(BuildingData buildingData, bool isSaveInit = false);
        public abstract bool IsFullStorage();
        public abstract void UpgradeStorage();

        public abstract ItemCount[] GetCurrentPriceToUpgrades();
        
        //public bool isUpgradeState;
        
        public abstract void SetUpgradeState();

        public abstract bool GetUpgradeState();

        public abstract void SetUpgradeState(bool isUpgrade);
    }
}