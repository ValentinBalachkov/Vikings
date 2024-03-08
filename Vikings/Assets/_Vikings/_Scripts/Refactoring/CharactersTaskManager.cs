using System;
using System.Collections.Generic;
using _Vikings.Refactoring.Character;
using UniRx;
using UnityEngine;
using Vikings.Map;
using Vikings.Object;
using Zenject;

namespace _Vikings._Scripts.Refactoring
{
    public class CharactersTaskManager : MonoBehaviour
    {
        public ReactiveCommand<AbstractBuilding> setBuildingToQueue = new();
        
        private CompositeDisposable _disposable = new();

        private MapFactory _mapFactory;
        private Dictionary<ResourceType, int> _neededResource = new();
        private AbstractBuilding _currentBuilding;
        private CharacterFactory _characterFactory;
        

        [Inject]
        public void Init(MapFactory mapFactory, CharacterFactory characterFactory)
        {
            _mapFactory = mapFactory;
            _characterFactory = characterFactory;
            setBuildingToQueue.Subscribe(OnSetBuilding).AddTo(_disposable);
        }

        private void Start()
        {
            _characterFactory.SpawnCharacters();
            
            var characters = _characterFactory.GetCharacters();

            foreach (var character in characters)
            {
                var boneFire = _mapFactory.GetBoneFire();
                character.SetState<DoMoveState>(boneFire);
            }
        }

        private void OnSetBuilding(AbstractBuilding abstractBuilding)
        {
            abstractBuilding.ChangeState(BuildingState.InProgress);

            _currentBuilding = abstractBuilding;

            _neededResource.Clear();

            _neededResource = abstractBuilding.GetNeededItemsCount();

            var characters = _characterFactory.GetCharacters();

            foreach (var character in characters)
            {
                SetCharacterToBuilding(abstractBuilding, character);
            }
        }

        private void OnCharacterActionDone(CharacterStateMachine character)
        {
            SetCharacterToBuilding(_currentBuilding, character);
        }

        private void SetCharacterToBuilding(AbstractBuilding abstractBuilding, CharacterStateMachine character)
        {
            foreach (var resource in _neededResource)
            {
                if (resource.Value > 0)
                {
                    character.SetBuildingToAction(abstractBuilding,
                        _mapFactory.GetNearestResource(resource.Key, character), OnCharacterActionDone);

                    _neededResource[resource.Key] -= character.Count;

                    return;
                }
            }

            SetCharacterToStorage(character);
        }

        private void SetCharacterToStorage(CharacterStateMachine character)
        {
            var storage = _mapFactory.GetPartialStorage();

            if (storage == null)
            {
                var boneFire = _mapFactory.GetBoneFire();
                character.SetState<DoMoveState>(boneFire);
                return;
            }

            character.SetBuildingToAction(storage,
                _mapFactory.GetNearestResource(storage.ResourceType, character), SetCharacterToStorage);
        }
    }
}