using System.Collections;
using _Vikings.Refactoring.Character;
using DG.Tweening;
using UnityEngine;
using Vikings.Items;

namespace _Vikings._Scripts.Refactoring
{
    public class Resource : AbstractResource
    {
        [SerializeField] private AudioSource _audioSource;
        
        private GameObject _view;
        protected ItemData _itemData;
        protected ParticleSystem _effectOnTap;
        protected Coroutine _getItemCoroutine;
        protected bool _isEffectActive;

        protected virtual void OnMouseUp()
        {
            if (_isEffectActive || _effectOnTap == null)
            {
                return;
            }
            StartCoroutine(EffectOnTapCoroutine());
        }
     
        public override float GetStoppingDistance()
        {
            return _itemData.stoppingDistance;
        }

        public override void CharacterAction(CharacterStateMachine characterStateMachine)
        {
            _getItemCoroutine = StartCoroutine(GetItemCoroutine(characterStateMachine));
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
            if (_getItemCoroutine != null)
            {
                StopCoroutine(_getItemCoroutine);
            }
            
            EndAction = null;
            IsTarget = false;
            _view.SetActive(true);
        }

        private void SetAudioSetting()
        {
            _audioSource.outputAudioMixerGroup = _itemData._mixer;
            _audioSource.clip = _itemData._actionSound;
        }

        protected IEnumerator EffectOnTapCoroutine()
        {
            _isEffectActive = true;
            _effectOnTap.gameObject.SetActive(true);
            _effectOnTap.Play();
            ModelAnimationOnTap();
            yield return new WaitForSeconds(2f);
            _effectOnTap.Stop();
            _effectOnTap.gameObject.SetActive(false);
            _isEffectActive = false;
        }

        protected virtual void ModelAnimationOnTap()
        {
            var tween = _view.transform.DOShakeScale(1f, new Vector3(0.5f, 0, 0.5f));
            tween.onComplete += () =>
            {
                tween.Kill();
            };
        }

        protected virtual void SpawnModel()
        {
            _view = Instantiate(_itemData.View, transform);
            if (_itemData.EffectOnTap == null)
            {
                return;
            }
            _effectOnTap = Instantiate(_itemData.EffectOnTap, transform);
            _effectOnTap.gameObject.SetActive(false);
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