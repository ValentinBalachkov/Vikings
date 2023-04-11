using UnityEngine;
using Vikings.Building;
using Vikings.Chanacter;
using Vikings.Items;

namespace Vikings.Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private InventoryData _inventoryData;
        [SerializeField] private StorageData _storageData;
        [SerializeField] private CharacterStateMachine _characterStateMachine;
        
        private ItemData[] _itemDatas;

        public void ChangeItemCount(ItemData itemData, int count)
        {
            _inventoryData.ChangeItemCount(itemData, count);
        }

        private void Awake()
        {
            _itemDatas = Resources.LoadAll<ItemData>($"ItemData");
            _inventoryData.OnInventoryChange += OnChangeInventoryAndStorage;
        }

        private void OnChangeInventoryAndStorage(ItemData itemData)
        {
            if (_inventoryData.IsFullInventory() && _storageData.IsFullStorage())
            {
                _characterStateMachine.SetState<IdleState>();
            }
            else if(_characterStateMachine.CurrentState is IdleState)
            {
                _characterStateMachine.SetState<MovingState>();
            }
        }
    }
}