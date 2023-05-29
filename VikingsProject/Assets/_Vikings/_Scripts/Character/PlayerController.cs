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
        [SerializeField] private float _rotateSpeed = 10;
        [SerializeField] private CharactersConfig _charactersConfig;
        
        
        private Action _onGetPosition;
        private bool _onPosition;
        private Transform _currentPoint;
        

        private void Update()
        {
            var targetRotation = Quaternion.LookRotation(_navMeshAgent.destination - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotateSpeed * Time.deltaTime);
            if (CheckDestinationReached() && !_onPosition)
            {
                _onGetPosition?.Invoke();
                _onPosition = true;
            }
            
        }
        private bool CheckDestinationReached() {
            float distanceToTarget = Vector3.Distance(transform.position, _currentPoint.position);
            if(Math.Round(distanceToTarget,1) <= _navMeshAgent.stoppingDistance + 0.1f)
            {
                return true;
            }

            return false;
       }

        public void SetActionOnGetPosition(Action action)
        {
            _onGetPosition = null;
            _onPosition = false;
            _onGetPosition = action;
        }

        public void MoveToPoint(Transform point)
        {
            _navMeshAgent.speed = _charactersConfig.SpeedMove;
            _currentPoint = point;
            _navMeshAgent.SetDestination(point.position);
        }

        public void SetStoppingDistance(float distance)
        {
            _navMeshAgent.stoppingDistance = distance;
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

