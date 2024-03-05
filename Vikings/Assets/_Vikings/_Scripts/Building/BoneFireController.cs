using System;
using UnityEngine;

namespace Vikings.Building
{
    public class BoneFireController : MonoBehaviour
    {
        public GameObject BoneFire => _boneFire;
        [SerializeField] private GameObject _boneFirePrefab;
        [SerializeField] private Transform _boneFirePoint;
        [SerializeField] private BoneFirePositionData[] _positionsData;
        
        private GameObject _boneFire;

        public void Spawn()
        {
            _boneFire = Instantiate(_boneFirePrefab, _boneFirePoint);
        }

        public BoneFirePositionData GetCurrentPosition()
        {
            foreach (var pos in _positionsData)
            {
                if (!pos.isDisable)
                {
                    pos.isDisable = true;
                    return pos;
                }
            }

            return _positionsData[0];
        }

        public void ResetPos(int id)
        {
            foreach (var pos in _positionsData)
            {
                if (pos.id == id)
                {
                    pos.isDisable = false;
                }
            }
        }
        
    }

   
}