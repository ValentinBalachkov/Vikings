using System.Collections;
using _Vikings.Refactoring.Character;
using UnityEngine;
using Vikings.Items;

namespace _Vikings._Scripts.Refactoring
{
    public class Resource : AbstractResource
    {
        [SerializeField] private AudioSource _audioSource;
        
        private GameObject _view;
        private const float DELAY_ENABLE = 5f;
        private ItemData _itemData;

        public override void CharacterAction(CharacterStateMachine characterStateMachine)
        {
            characterStateMachine.Inventory.SetItemToInventory(_itemData.ResourceType, _itemData.DropCount);
            StartCoroutine(GetItemCoroutine());
        }

        public override void Init()
        {
            SpawnModel();
            SetAudioSetting();
        }

        public override IItemData GetItemData()
        {
            return _itemData;
        }

        public override void SetItemData(IItemData itemData)
        {
            _itemData = itemData as ItemData;
        }

        public override bool IsEnableToGet()
        {
            return _view.activeSelf;
        }

        private void SetAudioSetting()
        {
            _audioSource.outputAudioMixerGroup = _itemData._mixer;
            _audioSource.clip = _itemData._actionSound;
        }

        private void SpawnModel()
        {
            _view = Instantiate(_itemData.View, transform);
        }


        private IEnumerator GetItemCoroutine()
        {
            _view.SetActive(false);
            if (_audioSource != null)
            {
                _audioSource.Play();
            }
            
            yield return new WaitForSeconds(DELAY_ENABLE);
            _view.SetActive(true);
        }
    }
}