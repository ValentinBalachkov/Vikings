using System.Collections.Generic;
using System.Linq;
using _Vikings.Refactoring.Character;
using UniRx;
using UnityEngine;
using Vikings.Items;
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
        private WeaponFactory _weaponFactory;


        [Inject]
        public void Init(MapFactory mapFactory, CharacterFactory characterFactory, WeaponFactory weaponFactory)
        {
            _mapFactory = mapFactory;
            _characterFactory = characterFactory;
            _weaponFactory = weaponFactory;
            setBuildingToQueue.Subscribe(OnSetBuilding).AddTo(_disposable);
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
                        GetNearedResource(resource.Key, character), OnCharacterActionDone);

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
                GetNearedResource(storage.ResourceType, character), SetCharacterToStorage);
        }

        private AbstractResource GetNearedResource(ResourceType resourceType, CharacterStateMachine characterStateMachine)
        {
            var abstractResources = _mapFactory.GetAllResources();
            var weapons = _weaponFactory.GetOpenWeapons();

            List<ItemData> openedResourcesData = weapons.SelectMany(weapon => weapon.GetWeaponData().avaleableResources).ToList();

            var openedResources = abstractResources.Where(x => openedResourcesData.Contains(x.GetItemData())).ToList();

            var item = openedResources.Where(x => x.GetItemData().ResourceType == resourceType && x.IsEnableToGet())
                .OrderBy(x => x.GetItemData().Priority).ThenByDescending(x => x.GetItemData().DropCount)
                .ThenBy(x => Vector3.Distance(x.GetPosition().position, characterStateMachine.GetPosition().position))
                .FirstOrDefault();

            return item;
        }
    }
}