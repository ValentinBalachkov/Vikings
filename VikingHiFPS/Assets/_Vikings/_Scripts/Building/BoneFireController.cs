using UnityEngine;

namespace Vikings.Building
{
    public class BoneFireController : MonoBehaviour
    {
        public GameObject BoneFire => _boneFire;
        [SerializeField] private GameObject _boneFirePrefab;
        [SerializeField] private Transform _boneFirePoint;
        private GameObject _boneFire;

        public void Spawn()
        {
            _boneFire = Instantiate(_boneFirePrefab, _boneFirePoint);
        }

        public Transform GetCurrentPosition()
        {
            return _boneFire.transform;
        }
    }
}