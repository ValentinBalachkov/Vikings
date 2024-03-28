using System;
using System.Collections.Generic;
using _Vikings._Scripts.Refactoring;
using UnityEngine;
using Vikings.Chanacter;
using Vikings.Object;

namespace _Vikings.Refactoring.Character
{
    public class CharacterStateMachine : StateMachine
    {
        public bool IsIdle;

        public Action<CharacterStateMachine> OnCharacterAction;
        public int BackpackCount => _characterManager.ItemsCount;

        public float SpeedWork => _characterManager.SpeedWork;
        public Inventory Inventory => _inventory;

        private BaseCharacterState _currentState;

        private List<BaseCharacterState> _states = new();

        private DoActionState _doActionState;
        private DoMoveState _doMoveState;

        private PlayerController _playerController;

        private Inventory _inventory;

        private CharacterManager _characterManager;

        private Queue<ActionQueueData> _actionsQueue = new();


        public void Init(Transform position, PlayerController playerController, CharacterManager characterManager,
            WeaponFactory weaponFactory)
        {
            _inventory = new Inventory(weaponFactory);
            _characterManager = characterManager;

            _playerController = Instantiate(playerController, position);
            _playerController.Init(transform, _characterManager);

            _doActionState = new DoActionState("DoActionState", this);
            _doMoveState = new DoMoveState("DoMoveState", this, _playerController);

            _states.Add(_doActionState);
            _states.Add(_doMoveState);
        }

        public void SetCollectAnimation(AnimatorOverrideController animatorOverrideController, Action endAction)
        {
            _playerController.PlayerAnimationEvent.DisableEffects();
            _playerController.PlayerAnimationController.ReturnBaseAnimator();
            
            if (animatorOverrideController != null)
            {
                _playerController.PlayerAnimationController.ChangeAnimatorController(animatorOverrideController);
            }

            _playerController.OnEndAnimation = endAction;
            _playerController.SetCollectAnimation();
        }

        public void SetWorkAnimation(Action endAction)
        {
            _playerController.PlayerAnimationEvent.DisableEffects();
            _playerController.PlayerAnimationController.ReturnBaseAnimator();
            _playerController.OnEndAnimation = endAction;
            _playerController.SetCraftingAnimation();
        }


        public void SetIdleAnimation()
        {
            _playerController.PlayerAnimationEvent.DisableEffects();
            _playerController.PlayerAnimationController.ReturnBaseAnimator();
            _playerController.SetIdleAnimation();
        }

        public void ResetDestinationForLook(Transform newDestination)
        {
            _playerController.ResetDestinationForLook(newDestination);
        }

        public void ClearRotation()
        {
            _playerController.ResetIdleFlag();
        }

        public Transform GetPosition()
        {
            return transform;
        }


        public void SetObject(AbstractObject abstractObject, Action<CharacterStateMachine> endAction = null)
        {
            OnCharacterAction = endAction;
            _actionsQueue.Clear();
            Inventory.Clear();
            AddActionToQueue(abstractObject, _doMoveState);
            AddActionToQueue(abstractObject, _doActionState);
            OnNextAction();
        }

        private void SetState(BaseCharacterState characterState, AbstractObject abstractObject)
        {
            _currentState = characterState;
            ChangeState(_currentState, abstractObject);
        }

        public void OnNextAction()
        {
            ActionQueueData actionData = new();

            if (_actionsQueue.TryDequeue(out actionData))
            {
                IsIdle = actionData.abstractObject is BoneFire;

                SetState(actionData.characterState, actionData.abstractObject);
                return;
            }

            OnCharacterAction?.Invoke(this);
        }

        public void SetBuildingToAction(AbstractObject setObject, AbstractObject getObject,
            Action<CharacterStateMachine> endAction, bool clearQueue = true)
        {
            if (clearQueue)
            {
                OnCharacterAction = endAction;
                _actionsQueue.Clear();
                Inventory.Clear();
            }

            AddActionToQueue(getObject, _doMoveState);
            AddActionToQueue(getObject, _doActionState);
            AddActionToQueue(setObject, _doMoveState);
            AddActionToQueue(setObject, _doActionState);

            if (clearQueue)
            {
                OnNextAction();
            }
        }

        private void AddActionToQueue(AbstractObject abstractObject, BaseCharacterState characterState)
        {
            ActionQueueData actionMoveFirst = new()
            {
                characterState = characterState,
                abstractObject = abstractObject
            };
            _actionsQueue.Enqueue(actionMoveFirst);
        }
    }

    [Serializable]
    public class ActionQueueData
    {
        public BaseCharacterState characterState;
        public AbstractObject abstractObject;
    }
}