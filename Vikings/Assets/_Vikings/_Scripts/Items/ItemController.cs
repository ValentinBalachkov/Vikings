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
        public bool IsEngaged { get; set; }
        public ItemData Item => _itemData;
        public int Priority { get; set; }
        public bool DisableToGet { get; set; }

        [SerializeField] private GameObject _model;
        [SerializeField] private AudioSource _audioSource;
        

        private const float DELAY_ENABLE = 5f;
        private ItemData _itemData;
        private bool _isEnable;


        public void Init(ItemData itemData)
        {
            Priority = 1;
            _itemData = itemData;
            _isEnable = true;
        }

        public Transform GetItemPosition()
        {
            return transform;
        }

        public void TakeItem()
        {
            StartCoroutine(GetItemCoroutine());
        }

        public ItemData GetItemData()
        {
            return _itemData;
        }

        private IEnumerator GetItemCoroutine()
        {
            IsEngaged = false;
            _isEnable = false;
            DisableToGet = true;
            _model.SetActive(_isEnable);
            if (_audioSource != null)
            {
                _audioSource.Play();
            }
            
            yield return new WaitForSeconds(DELAY_ENABLE);
            DisableToGet = false;
            _isEnable = true;
            _model.SetActive(_isEnable);
            OnEnable?.Invoke();
        }
    }
}