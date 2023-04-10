using UnityEngine;
using UnityEngine.AI;

namespace Vikings.Chanacter
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        
        [SerializeField] private float _speed;
        
        public void MoveToPoint(Transform point)
        {
            _navMeshAgent.SetDestination(point.position);
            //transform.position = Vector3.MoveTowards(transform.position, point.position, _speed * Time.deltaTime);
        }
    }
}

