using System;
using System.Collections.Generic;
using _Vikings._Scripts.Refactoring;
using UniRx;

namespace Vikings.Object
{
    public abstract class AbstractBuilding : AbstractObject
    {
        public ReactiveProperty<int> CurrentLevel = new();

        protected Action<int, ResourceType> ChangeCount;

        protected Dictionary<ResourceType, int> currentItems = new();
        public abstract void Upgrade();

        public AbstractBuildingDynamicData abstractBuildingDynamicData;

        public virtual void ChangeState(BuildingState state)
        {
            buildingState = state;
        }

        public abstract Dictionary<ResourceType, int> GetNeededItemsCount();
        
        public abstract Dictionary<ResourceType, int> GetPriceForUpgrade();

        public abstract void SetData(BuildingData buildingData);
        
        public abstract (bool, string) IsEnableToBuild<T>(T arg);
        public abstract BuildingData GetData();
        
        protected BuildingState buildingState;
       
    }
    

    public enum BuildingState
    {
        NotSet = 0,
        InProgress = 1,
        Ready = 2
    }
}