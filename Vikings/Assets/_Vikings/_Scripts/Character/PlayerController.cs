using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Vikings.Chanacter
{
    public class PlayerController : MonoBehaviour
    {
        public Action OnEndAnimation;
        public PlayerAnimationController PlayerAnimationController => _playerAnimationController;
        
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private float _rotateSpeed = 10;
        [SerializeField] private CharactersConfig _charactersConfig;
        [SerializeField] private PlayerAnimationController _playerAnimationController;
        
        public Action OnGetPosition;
        private bool _onPosition = true;
        private Transform _currentPoint, _thisTransform;
        private bool _isIdleRotate;
        private Vector3 _currentDestination;

        private void Awake()
        {
            _thisTransform = GetComponent<Transform>();
        }

        private void Update()
        {
            var rotate = _navMeshAgent.destination - _thisTransform.position;
            if (_isIdleRotate)
            {
                 rotate = _currentDestination - _thisTransform.position;
            }
            if (rotate != Vector3.zero)
            {
                var targetRotation = Quaternion.LookRotation(rotate);
                transform.rotation = Quaternion.Slerp(_thisTransform.rotation, targetRotation, _rotateSpeed * Time.deltaTime);
            }
            if (CheckDestinationReached() && !_onPosition)
            {
                OnGetPosition?.Invoke();
                _onPosition = true;
            }
        }

        private bool CheckDestinationReached()
        {
            if (_currentPoint == null) return false;
            float distanceToTarget = Vector3.Distance(_thisTransform.position, _currentPoint.position);
            
            if (Math.Round(distanceToTarget, 1) <= Math.Round(_navMeshAgent.stoppingDistance + 0.5f, 1))
            {
                return true;
            }

            return false;
        }

        public void SetActionOnGetPosition(Action action)
        {
            OnGetPosition = null;
            _onPosition = false;
            OnGetPosition = action;
        }

        public void ClearAction()
        {
            OnGetPosition = null;
            _onPosition = true;
        }

        public void MoveToPoint(Transform point)
        {
            _navMeshAgent.speed = _charactersConfig.SpeedMove;
            _currentPoint = point;
            _navMeshAgent.SetDestination(point.position);
            _currentDestination = _navMeshAgent.destination;
        }

        public void ResetDestinationForLook(Transform newDestination)
        {
            _currentDestination = newDestination.position;
            _isIdleRotate = true;
        }

        public void ResetIdleFlag()
        {
            _isIdleRotate = false;
        }

        public void SetStoppingDistance(float distance)
        {
            _navMeshAgent.stoppingDistance = distance;
        }

        public void SetMoveAnimation()
        {
            _playerAnimationController.Run();
        }

        public void SetIdleAnimation()
        {
            _playerAnimationController.Idle();
        }

        public void SetCollectAnimation()
        {
            _playerAnimationController.Collect();
            StartCoroutine(AwaitAnimationCoroutine(0.5f));
        }

        public void SetCraftingAnimation()
        {
            _playerAnimationController.Work();
            StartCoroutine(AwaitAnimationCoroutine(2f));
        }

        private IEnumerator AwaitAnimationCoroutine(float time)
        {
            yield return new WaitForSeconds(time);
            OnEndAnimation?.Invoke();
        }
    }
}