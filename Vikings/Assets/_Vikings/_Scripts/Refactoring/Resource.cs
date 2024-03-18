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
        private ItemData _itemData;

        public override void CharacterAction(CharacterStateMachine characterStateMachine)
        {
            StartCoroutine(GetItemCoroutine(characterStateMachine));
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


        private IEnumerator GetItemCoroutine(CharacterStateMachine characterStateMachine)
        {
            int actionCount = 0;
            int itemCount = 0;

            int itemPerActionCount = characterStateMachine.Inventory.GetItemPerActionCount(_itemData);
            
            characterStateMachine.SetCollectAnimation();

            while (actionCount < characterStateMachine.ActionCount || itemCount < _itemData.DropCount)
            {
                actionCount++;
                itemCount += itemPerActionCount;
                yield return new WaitForSeconds(1);
                yield return null;
            }

            if (itemCount > _itemData.DropCount)
            {
                itemCount = _itemData.DropCount;
            }
            
            characterStateMachine.Inventory.SetItemToInventory(_itemData.ResourceType, itemCount);
            
            _view.SetActive(false);
            if (_audioSource != null)
            {
                _audioSource.Play();
            }
            
            yield return new WaitForSeconds(_itemData.disableDelaySecond);
            _view.SetActive(true);
        }
    }
}