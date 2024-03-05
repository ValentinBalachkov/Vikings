using System;
using System.Collections.Generic;
using _Vikings._Scripts.Refactoring;
using UniRx;

namespace Vikings.Object
{
    public abstract class AbstractBuilding : AbstractObject
    {
        public ReactiveProperty<int> CurrentLevel = new();

        public Action<int, ResourceType> ChangeCount;

        protected BuildingState buildingState;

        public Dictionary<ResourceType, int> priceToUpgrades = new();
        
        public Dictionary<ResourceType, int> currentItems = new();
        public abstract void Upgrade();

        public AbstractBuildingDynamicData abstractBuildingDynamicData;

        public virtual void ChangeState(BuildingState state)
        {
            buildingState = state;
        }

        public abstract Dictionary<ResourceType, int> GetNeededItemsCount();

        public abstract void SetData(BuildingData buildingData);
    }

    public enum BuildingState
    {
        NotSet = 0,
        InProgress = 1,
        Ready = 2
    }
}