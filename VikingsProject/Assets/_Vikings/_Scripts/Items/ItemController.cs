using System;
using System.Collections;
using UnityEngine;
using Vikings.Building;

namespace Vikings.Items
{
    public class ItemController : MonoBehaviour, IGetItem
    {
        public Action OnEnable;
        public bool IsEnable => _isEnable;
        public ItemData Item => _itemData;
        
        [SerializeField] private GameObject _model;

        private const float DELAY_ENABLE = 5f;
        private ItemData _itemData;
        private bool _isEnable;

        public void Init(ItemData itemData)
        {
            _itemData = itemData;
        }

        public void DisableItemOnMap()
        {
            StartCoroutine(GetItemCoroutine());
        }

        private IEnumerator GetItemCoroutine()
        {
            _isEnable = false;
            _model.SetActive(_isEnable);
            yield return new WaitForSeconds(DELAY_ENABLE);
            _isEnable = true;
            _model.SetActive(_isEnable);
            OnEnable?.Invoke();
        }

        public Transform GetItemPosition()
        {
            return transform;
        }

        public void TakeItem()
        {
            StartCoroutine(GetItemCoroutine());
        }
    }
}