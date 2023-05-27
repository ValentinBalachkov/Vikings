using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vikings.Building;
using Vikings.Chanacter;
using Random = UnityEngine.Random;

namespace Vikings.Items
{
    public class ItemsOnMapController : MonoBehaviour
    {
        public List<ItemController> ItemsList => _itemsList;

        [SerializeField] private ItemPosition[] _itemPositions;
        [SerializeField] private CharactersConfig _charactersConfig;

        [SerializeField] private BoneFireController _boneFireController;
        [SerializeField] private BuildingsOnMap _buildingsOnMap;

        private List<ItemController> _itemsList = new();

        private StorageController _storageController;
        private GameObject _boneFire;
        private List<ItemController> _allItems = new();

        private void Awake()
        {
            Spawn();
            BuildingHomeActionController.Instance.OnHomeLevelUp += SpawnLevelItems;
        }

        private void OnDestroy()
        {
            BuildingHomeActionController.Instance.OnHomeLevelUp -= SpawnLevelItems;
        }

        public void AddItemToItemsList(ItemData itemData)
        {
            var items = _allItems.Where(x => x.Item.ID == itemData.ID).ToList();
            foreach (var item in items)
            {
                _itemsList.Add(item);
            }
        }

        public void SpawnLevelItems(int level)
        {
            foreach (var item in _itemPositions)
            {
                foreach (var posLevel in item.position)
                {
                    if (level == posLevel.level)
                    {
                        foreach (var pos in posLevel.position)
                        {
                            var itemOnScene = Instantiate(item.item.Prefab, pos);
                            if (item.item.ItemName == "Three")
                            {
                                var scale = Random.Range(0.7f, 1f);
                                itemOnScene.transform.localScale = new Vector3(scale, scale, scale);
                            }

                            itemOnScene.Init(item.item);
                            _allItems.Add(itemOnScene);
                            itemOnScene.OnEnable += () => _buildingsOnMap.UpdateCurrentBuilding(false, true);
                            if (item.item.IsOpen)
                            {
                                _itemsList.Add(itemOnScene);
                            }
                        }
                    }
                }
            }
        }

        private void Spawn()
        {
            foreach (var item in _itemPositions)
            {
                foreach (var posLevel in item.position)
                {
                    if (_charactersConfig.houseLevel >= posLevel.level)
                    {
                        foreach (var pos in posLevel.position)
                        {
                            var itemOnScene = Instantiate(item.item.Prefab, pos);
                            if (item.item.ItemName == "Three")
                            {
                                var scale = Random.Range(0.7f, 1f);
                                itemOnScene.transform.localScale = new Vector3(scale, scale, scale);
                            }

                            itemOnScene.Init(item.item);
                            _allItems.Add(itemOnScene);
                            itemOnScene.OnEnable += () => _buildingsOnMap.UpdateCurrentBuilding(false, true);
                            if (item.item.IsOpen)
                            {
                                _itemsList.Add(itemOnScene);
                            }
                        }
                    }
                }
            }

            _boneFireController.Spawn();
        }
    }
}