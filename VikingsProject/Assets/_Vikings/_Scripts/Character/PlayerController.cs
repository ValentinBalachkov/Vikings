using UnityEngine;

namespace Vikings.Chanacter
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Transform point;
        
        [SerializeField] private float _speed;
        
        public void MoveToPoint(Transform point)
        {
            transform.position = Vector3.MoveTowards(transform.position, point.position, _speed * Time.deltaTime);
        }
    }
}

