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
    public abstract class AbstractBuilding : AbstractObject
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
        
        public abstract (bool, int, Sprite) IsEnableToBuild<T>(T arg);
        public abstract BuildingData GetData();
        
        public BuildingState buildingState;

        protected MainPanelManager _panelManager;

        protected bool isCraftActivated;

        [SerializeField] protected AudioSource _changeItemSound;
        [SerializeField] protected AudioSource _craftingSound;
        
        [SerializeField] protected ParticleSystem particleCraftEffect;
        
        
        protected IEnumerator UpgradeDelay(CharacterStateMachine characterStateMachine, float buildingTime)
        {
            _craftingSound.Play();
            isCraftActivated = true;
            particleCraftEffect.gameObject.SetActive(true);
            particleCraftEffect.Play();
            var time = buildingTime * characterStateMachine.SpeedWork;
            _panelManager.SudoGetPanel<CraftingIndicatorView>().Setup((int)time, GetPosition());
            yield return new WaitForSeconds(time);
            particleCraftEffect.Stop();
            particleCraftEffect.gameObject.SetActive(false);
            _craftingSound.Stop();
            Upgrade();
            _panelManager.SudoGetPanel<MenuButtonsManager>().EnableButtons(true);
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