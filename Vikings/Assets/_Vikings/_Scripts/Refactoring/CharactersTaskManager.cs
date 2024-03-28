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
        private MainPanelManager _panelManager;


        [Inject]
        public void Init(MapFactory mapFactory, CharacterFactory characterFactory, WeaponFactory weaponFactory,
            MainPanelManager panelManager)
        {
            _mapFactory = mapFactory;
            _characterFactory = characterFactory;
            _weaponFactory = weaponFactory;
            _panelManager = panelManager;
            setBuildingToQueue.Subscribe(OnSetBuilding).AddTo(_disposable);
        }

        public void OnResourceEnable()
        {
            var characters = _characterFactory.GetCharacters();

            foreach (var character in characters)
            {
                if (!character.IsIdle)
                {
                    continue;
                }

                SetCharacterWork(character);
            }
        }

        public void SetCharacterWork(CharacterStateMachine character)
        {
            if (_currentBuilding != null)
            {
                SetCharacterToBuilding(character);
            }
            else
            {
                SetCharacterToStorage(character);
            }
        }

        public void SetCharactersToCrafting(AbstractBuilding building)
        {
            _currentBuilding = null;
            var resources = _mapFactory.GetAllResources();

            foreach (var resource in resources)
            {
                resource.ResetState();
            }

            var characters = _characterFactory.GetCharacters();

            foreach (var character in characters)
            {
                character.SetObject(building, SetCharacterToStorage);
            }
        }

        public void SetCharacterToStorage(Storage storage)
        {
            if (_currentBuilding != null) return;

            var characters = _characterFactory.GetCharacters();

            foreach (var character in characters)
            {
                var resource = GetNearedResource(storage.ResourceType, character, true);
                if (resource == null)
                {
                    SetCharacterToFire(character);
                    continue;
                }

                character.SetBuildingToAction(storage,
                    resource, SetCharacterToStorage);
            }
        }

        private void OnSetBuilding(AbstractBuilding abstractBuilding)
        {
            _currentBuilding = abstractBuilding;

            _neededResource.Clear();

            _neededResource = abstractBuilding.GetNeededItemsCount();

            _panelManager.SudoGetPanel<CollectingResourceView>().Setup(abstractBuilding);

            var characters = _characterFactory.GetCharacters();

            foreach (var character in characters)
            {
                SetCharacterToBuilding(character);
            }
        }

        private void OnCharacterActionDone(CharacterStateMachine character)
        {
            SetCharacterToBuilding(character);
        }

        private void SetCharacterToBuilding(CharacterStateMachine character)
        {
            foreach (var resource in _neededResource)
            {
                DebugLogger.SendMessage($"{resource.Key} 1", Color.green);

                if (resource.Value > 0)
                {
                    DebugLogger.SendMessage($"{resource.Key} 2", Color.green);

                    AbstractObject resourceObject = null;

                    if (_currentBuilding is Storage storage)
                    {
                        DebugLogger.SendMessage($"{resource.Key} 2 1", Color.green);

                        if (storage.ResourceType == resource.Key)
                        {
                            DebugLogger.SendMessage($"{resource.Key} 2 2", Color.green);

                            resourceObject = GetNearedResource(resource.Key, character, true);
                        }
                        else
                        {
                            resourceObject = GetNearedResource(resource.Key, character);
                        }
                    }
                    else
                    {
                        resourceObject = GetNearedResource(resource.Key, character);
                    }

                    if (resourceObject == null)
                    {
                        DebugLogger.SendMessage($"{resourceObject} 3", Color.green);

                        continue;
                    }

                    int dropCount = character.BackpackCount;

                    if (resourceObject is AbstractResource abstractResource)
                    {
                        var data = abstractResource.GetItemData();
                        dropCount = GetItemDropCount(character, data);
                    }

                    DebugLogger.SendMessage($"{dropCount} 4", Color.green);

                    character.SetBuildingToAction(_currentBuilding,
                        resourceObject, OnCharacterActionDone);

                    _neededResource[resource.Key] -= dropCount;

                    return;
                }
            }

            DebugLogger.SendMessage($"5", Color.green);

            SetCharacterToStorage(character);
        }

        public void SetCharacterToStorage(CharacterStateMachine character)
        {
            var storages = _mapFactory.GetPartialStorage().OrderBy(x => x.Priority).ToList();

            if (storages.Count == 0)
            {
                SetCharacterToFire(character);
                return;
            }

            foreach (var storage in storages)
            {
                var resource = GetNearedResource(storage.ResourceType, character, true);

                if (resource == null)
                {
                    continue;
                }

                int dropCount = character.BackpackCount;

                if (resource is AbstractResource abstractResource)
                {
                    var data = abstractResource.GetItemData();
                    dropCount = GetItemDropCount(character, data);
                }

                if (storage.MaxStorageCount >= storage.Count + dropCount)
                {
                    character.SetBuildingToAction(storage,
                        resource, SetCharacterToStorage);
                    return;
                }
            }

            SetCharacterToFire(character);
        }

        private void SetCharacterToFire(CharacterStateMachine character)
        {
            if (character.IsIdle)
            {
                return;
            }

            var boneFire = _mapFactory.GetBoneFire();
            character.SetObject(boneFire);
        }

        private AbstractObject GetNearedResource(ResourceType resourceType, CharacterStateMachine characterStateMachine,
            bool isStorage = false)
        {
            var storage = _mapFactory.GetAllBuildings<Storage>()
                .FirstOrDefault(x => x.ResourceType == resourceType && x.CurrentLevel.Value > 0);

            if (storage != null && !isStorage &&
                storage.Count > characterStateMachine.BackpackCount * storage.CharactersCount)
            {
                storage.CharactersCount++;
                return storage;
            }

            var abstractResources = _mapFactory.GetAllResources();
            var weapons = _weaponFactory.GetOpenWeapons();

            List<ItemData> openedResourcesData =
                weapons.SelectMany(weapon => weapon.GetWeaponData().avaleableResources).ToList();

            var openedResources = abstractResources.Where(x => openedResourcesData.Contains(x.GetItemData())).ToList();


            var item = openedResources.Where(x => x.GetItemData().ResourceType == resourceType && x.IsEnableToGet())
                .OrderBy(x => x.GetItemData().Priority).ThenByDescending(x => x.GetItemData().DropCount)
                .ThenBy(x => Vector3.Distance(x.GetPosition().position, characterStateMachine.GetPosition().position))
                .FirstOrDefault();

            if (item != null)
            {
                DebugLogger.SendMessage($"{item} is select", Color.green);

                item.IsTarget = true;
            }

            return item;
        }


        private int GetItemDropCount(CharacterStateMachine characterStateMachine, ItemData resource)
        {
            int itemPerActionCount = characterStateMachine.Inventory.GetItemPerActionCount(resource);
            var count = characterStateMachine.BackpackCount * itemPerActionCount;
            if (count > resource.DropCount)
            {
                count = resource.DropCount;
            }

            return count;
        }
    }
}