﻿using System.Collections;
using _Vikings.Refactoring.Character;
using UnityEngine;
using Vikings.Items;

namespace _Vikings._Scripts.Refactoring
{
    public class Resource : AbstractResource
    {
        [SerializeField] private AudioSource _audioSource;
        
        private GameObject _view;
        protected ItemData _itemData;
     
        public override float GetStoppingDistance()
        {
            return _itemData.stoppingDistance;
        }

        public override void CharacterAction(CharacterStateMachine characterStateMachine)
        {
            StartCoroutine(GetItemCoroutine(characterStateMachine));
        }

        public override void Init(MainPanelManager mainPanelManager)
        {
            SpawnModel();
            SetAudioSetting(); 
            transform.localScale = Vector3.one;
        }

        public override ItemData GetItemData()
        {
            return _itemData;
        }

        public override void SetItemData(ItemData itemData)
        {
            _itemData = itemData;
        }

        public override bool IsEnableToGet()
        {
            return !IsTarget;
        }

        public override void ResetState()
        {
            StopAllCoroutines();
            EndAction = null;
            IsTarget = false;
            _view.SetActive(true);
        }

        private void SetAudioSetting()
        {
            _audioSource.outputAudioMixerGroup = _itemData._mixer;
            _audioSource.clip = _itemData._actionSound;
        }

        protected virtual void SpawnModel()
        {
            _view = Instantiate(_itemData.View, transform);
        }

        protected void PlaySound()
        {
            _audioSource.Play();
        }


        protected virtual IEnumerator GetItemCoroutine(CharacterStateMachine characterStateMachine)
        {
            int actionCount = 0;
            int itemCount = 0;

            int itemPerActionCount = characterStateMachine.Inventory.GetItemPerActionCount(_itemData);
            
            characterStateMachine.SetCollectAnimation(_itemData.AnimatorOverride, null);

            while (actionCount < characterStateMachine.BackpackCount && itemCount < _itemData.DropCount)
            {
                actionCount++;
                itemCount += itemPerActionCount;
                yield return new WaitForSeconds(1);
            }

            PlaySound();

            if (itemCount > _itemData.DropCount)
            {
                itemCount = _itemData.DropCount;
            }
            
            characterStateMachine.Inventory.SetItemToInventory(_itemData.ResourceType, itemCount);

            EndAction?.Invoke();
            
            _view.SetActive(false);
            
            yield return new WaitForSeconds(_itemData.disableDelaySecond);
            _view.SetActive(true);
            IsTarget = false;
            ResourceEnable?.Invoke();
        }
    }
}