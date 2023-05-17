using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Vikings.Chanacter
{
    public class PlayerController : MonoBehaviour
    {
        public Action OnEndAnimation;
        [SerializeField] private Animator _animator;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        public void MoveToPoint(Transform point)
        {
            _navMeshAgent.SetDestination(point.position);
        }

        public void SetMoveAnimation()
        {
            _animator.SetTrigger("Move");
        }
        
        public void SetIdleAnimation()
        {
            _animator.SetTrigger("Idle");
        }
        
        public void SetCollectAnimation()
        {
            _animator.SetTrigger("Collect");
            StartCoroutine(AwaitAnimationCoroutine(0.5f));
        }
        
        public void SetCraftingAnimation()
        {
            _animator.SetTrigger("Crafting");
            StartCoroutine(AwaitAnimationCoroutine(2f));
        }

        private IEnumerator AwaitAnimationCoroutine(float time)
        {
            yield return new WaitForSeconds(time);
            OnEndAnimation?.Invoke();
        }
    }
}

