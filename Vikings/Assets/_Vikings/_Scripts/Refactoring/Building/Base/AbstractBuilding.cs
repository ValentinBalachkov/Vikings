using System;
using System.Collections;
using System.Collections.Generic;
using _Vikings._Scripts.Refactoring;
using _Vikings.Refactoring.Character;
using PanelManager.Scripts.Interfaces;
using UniRx;
using UnityEngine;

namespace Vikings.Object
{
    public abstract class AbstractBuilding : AbstractObject, IAcceptArg<MainPanelManager>
    {
        public ReactiveProperty<int> CurrentLevel = new();

        public Action<int, ResourceType> ChangeCount;

        public Action<AbstractBuilding> BuildingComplete;

        public Dictionary<ResourceType, int> currentItems = new();
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
        
        public BuildingState buildingState;

        protected MainPanelManager panelManager;

        protected bool isCraftActivated;
        
        [SerializeField] protected ParticleSystem particleCraftEffect;


        public virtual void AcceptArg(MainPanelManager arg)
        {
            panelManager = arg;
        }
        
        protected IEnumerator UpgradeDelay(CharacterStateMachine characterStateMachine, float buildingTime)
        {
            isCraftActivated = true;
            particleCraftEffect.gameObject.SetActive(true);
            particleCraftEffect.Play();
            var time = buildingTime * characterStateMachine.SpeedWork;
            panelManager.SudoGetPanel<CraftingIndicatorView>().Setup((int)time, GetPosition());
            yield return new WaitForSeconds(time);
            particleCraftEffect.Stop();
            particleCraftEffect.gameObject.SetActive(false);
            Upgrade();
            panelManager.SudoGetPanel<MenuButtonsManager>().EnableButtons(true);
            EndAction?.Invoke();
        }
    }
    

    public enum BuildingState
    {
        NotSet = 0,
        InProgress = 1,
        Ready = 2,
        Crafting = 3
    }
}