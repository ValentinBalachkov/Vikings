using UnityEngine;
using UnityEngine.AI;

namespace Vikings.Chanacter
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        public void MoveToPoint(Transform point)
        {
            _navMeshAgent.SetDestination(point.position);
        }
    }
}

