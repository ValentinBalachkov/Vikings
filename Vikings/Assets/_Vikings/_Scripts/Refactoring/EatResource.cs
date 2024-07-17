using System.Collections;
using _Vikings.Refactoring.Character;
using DG.Tweening;
using UnityEngine;

namespace _Vikings._Scripts.Refactoring
{
    public class EatResource : Resource
    {
        private EatResourceView _eatResourceView;
        
        public override void ResetState()
        {
            if (_getItemCoroutine != null)
            {
                StopCoroutine(_getItemCoroutine);
            }
            
            EndAction = null;
            IsTarget = false;
            _eatResourceView.ChaneState(true);
        }
        
        protected override void OnMouseUp()
        {
           base.OnMouseUp();

           if (IsTarget)
           {
               if (_getItemCoroutine != null)
               {
                   StopCoroutine(_getItemCoroutine);
               }
               
               _eatResourceView.ChaneState(true);
               IsTarget = false;
               ResourceEnable?.Invoke();
           }
        }

        protected override void SpawnModel()
        {
            _eatResourceView = Instantiate(_itemData.View, transform).GetComponent<EatResourceView>();
            if (_itemData.EffectOnTap == null)
            {
                return;
            }
            _effectOnTap = Instantiate(_itemData.EffectOnTap, transform);
            _effectOnTap.gameObject.SetActive(false);
        }
        
        protected override void ModelAnimationOnTap()
        {
            var tween = _eatResourceView.transform.DOShakeScale(1f, new Vector3(0.5f, 0, 0.5f));
            tween.onComplete += () =>
            {
                tween.Kill();
            };
        }

        protected override IEnumerator GetItemCoroutine(CharacterStateMachine characterStateMachine)
        {
            int actionCount = 0;
            int itemCount = 0;

            int itemPerActionCount = characterStateMachine.Inventory.GetItemPerActionCount(_itemData);
            
            characterStateMachine.SetCollectAnimation(_itemData.AnimatorOverride, null);

            while (actionCount < characterStateMachine.BackpackCount && itemCount < _itemData.DropCount)
            {
                actionCount++;
                itemCount += itemPerActionCount;
                PlaySound();
                yield return new WaitForSeconds(1);
            }

            if (itemCount > _itemData.DropCount)
            {
                itemCount = _itemData.DropCount;
            }
            
            characterStateMachine.Inventory.SetItemToInventory(_itemData.ResourceType, itemCount);

            EndAction?.Invoke();
            
            _eatResourceView.ChaneState(false);
            
            yield return new WaitForSeconds(_itemData.disableDelaySecond);
            _eatResourceView.ChaneState(true);
            IsTarget = false;
            ResourceEnable?.Invoke();
        }
    }
}