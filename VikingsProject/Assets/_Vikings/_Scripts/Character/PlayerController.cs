using UnityEngine;
using UnityEngine.AI;

namespace Vikings.Chanacter
{
    public class PlayerController : MonoBehaviour
    {
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
        }
        
        public void SetCraftingAnimation()
        {
            _animator.SetTrigger("Crafting");
        }
    }
}

