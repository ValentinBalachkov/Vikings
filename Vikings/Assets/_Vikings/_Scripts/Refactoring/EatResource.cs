using System.Collections;
using _Vikings.Refactoring.Character;
using UnityEngine;

namespace _Vikings._Scripts.Refactoring
{
    public class EatResource : Resource
    {
        private EatResourceView _eatResourceView;
        public override void ResetState()
        {
            StopAllCoroutines();
            EndAction = null;
            IsTarget = false;
            _eatResourceView.ChaneState(true);
        }

        protected override void SpawnModel()
        {
            _eatResourceView = Instantiate(_itemData.View, transform).GetComponent<EatResourceView>();
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