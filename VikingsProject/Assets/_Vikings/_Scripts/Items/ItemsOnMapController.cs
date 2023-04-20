using System.Collections.Generic;
using UnityEngine;
using Vikings.Building;

namespace Vikings.Items
{
    public class ItemsOnMapController : MonoBehaviour
    {
        public List<ItemController> ItemsList => _itemsList;

        [SerializeField] private ItemPosition[] _itemPositions;

        [SerializeField] private BoneFireController _boneFireController;

        private List<ItemController> _itemsList = new();

        private StorageController _storageController;
        private GameObject _boneFire;

        private void Awake()
        {
            Spawn();
        }

        private void Spawn()
        {
            foreach (var item in _itemPositions)
            {
                foreach (var pos in item.position)
                {
                    var itemOnScene = Instantiate(item.item.Prefab, pos);
                    itemOnScene.Init(item.item);
                    _itemsList.Add(itemOnScene);
                }
            }

            _boneFireController.Spawn();
        }
    }
}