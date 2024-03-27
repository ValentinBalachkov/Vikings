using UnityEngine;

namespace _Vikings._Scripts.Refactoring
{
    public class EatResourceView : MonoBehaviour
    {
        [SerializeField] private GameObject _activeSprite;
        [SerializeField] private GameObject _offSprite;

        public void ChaneState(bool isActive)
        {
            _activeSprite.SetActive(isActive);
            _offSprite.SetActive(!isActive);
        }
        
    }
}