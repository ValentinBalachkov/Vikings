using UnityEngine;

namespace _Vikings._Scripts.Refactoring
{
    public class EatResourceView : MonoBehaviour
    {
        [SerializeField] private GameObject _applesObject;
        

        public void ChaneState(bool isActive)
        {
            _applesObject.SetActive(isActive);
        }
        
    }
}