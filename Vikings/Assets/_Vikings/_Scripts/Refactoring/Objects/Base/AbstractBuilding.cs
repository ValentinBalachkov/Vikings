using System;
using System.Collections.Generic;
using _Vikings._Scripts.Refactoring;
using UniRx;
using Vikings.Items;

namespace Vikings.Object
{
    public abstract class AbstractBuilding : AbstractObject
    {
        public ReactiveProperty<int> CurrentLevel = new();

        public Action<int, ItemData> ChangeCount;

        protected BuildingState buildingState;

        protected Dictionary<ItemData, int> priceToUpgrades;
        
        protected Dictionary<ItemData, int> currentItems;
        public abstract void Upgrade();

        public AbstractBuildingDynamicData abstractBuildingDynamicData;

        public virtual void ChangeState(BuildingState state)
        {
            buildingState = state;
        }

        public abstract void FindData(BuildingType buildingType);
    }

    public enum BuildingState
    {
        NotSet = 0,
        InProgress = 1,
        Ready = 2
    }
}