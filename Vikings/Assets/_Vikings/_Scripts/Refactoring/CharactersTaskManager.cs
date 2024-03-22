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
        private ConfigSetting _configSetting;


        [Inject]
        public void Init(MapFactory mapFactory, CharacterFactory characterFactory, WeaponFactory weaponFactory,
            MainPanelManager panelManager, ConfigSetting configSetting)
        {
            _mapFactory = mapFactory;
            _characterFactory = characterFactory;
            _weaponFactory = weaponFactory;
            _panelManager = panelManager;
            _configSetting = configSetting;
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
                var resource = GetNearedResource(storage.ResourceType, character);
                if (resource == null)
                {
                    SetCharacterToFire(character);
                    continue;
                }

                character.SetBuildingToAction(storage,
                    resource, SetCharacterToStorage, false);
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
                if (resource.Value > 0)
                {
                    var resourceObject = GetNearedResource(resource.Key, character);
                    if (resourceObject == null)
                    {
                        continue;
                    }

                    character.SetBuildingToAction(_currentBuilding,
                        resourceObject, OnCharacterActionDone);

                    _neededResource[resource.Key] -= GetItemDropCount(character, resource.Key);

                    return;
                }
            }

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

                int dropCount = 1;

                if (resource as AbstractResource)
                {
                    var data = (resource as AbstractResource).GetItemData();
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
            var boneFire = _mapFactory.GetBoneFire();
            character.SetObject(boneFire);
        }

        private AbstractObject GetNearedResource(ResourceType resourceType, CharacterStateMachine characterStateMachine,
            bool isStorage = false)
        {
            var storage = _mapFactory.GetAllBuildings<Storage>()
                .FirstOrDefault(x => x.ResourceType == resourceType && x.CurrentLevel.Value > 0);

            if (storage != null && !isStorage && storage.Count > 1 * storage.CharactersCount)
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
                item.IsTarget = true;
            }

            return item;
        }

        private int GetItemDropCount(CharacterStateMachine characterStateMachine, ResourceType resource)
        {
            var itemData = _configSetting.resourcesData.FirstOrDefault(x => x.ResourceType == resource &&
                                                                            _weaponFactory.GetOpenWeapons()
                                                                                .SelectMany(weapon =>
                                                                                    weapon.GetWeaponData()
                                                                                        .avaleableResources)
                                                                                .Contains(x));
            int itemPerActionCount = characterStateMachine.Inventory.GetItemPerActionCount(itemData);
            var count = characterStateMachine.ActionCount * itemPerActionCount;
            if (count > itemData.DropCount)
            {
                count = itemData.DropCount;
            }

            return count;
        }
        
        private int GetItemDropCount(CharacterStateMachine characterStateMachine, ItemData resource)
        {
            int itemPerActionCount = characterStateMachine.Inventory.GetItemPerActionCount(resource);
            var count = characterStateMachine.ActionCount * itemPerActionCount;
            if (count > resource.DropCount)
            {
                count = resource.DropCount;
            }

            return count;
        }
    }
}