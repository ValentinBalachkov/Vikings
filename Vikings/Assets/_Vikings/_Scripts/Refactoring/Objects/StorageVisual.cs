using UnityEngine;
using Vikings.Building;

namespace _Vikings._Scripts.Refactoring.Objects
{
    public class StorageVisual : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _buildingSprite;
        [SerializeField] private SpriteRenderer _shadowSprite;

        private StorageData _storageData;

        public void Init(StorageData storageData)
        {
            _storageData = storageData;
        }
        
        
        public void SetupSprite(int lvl)
        {
            int index;

            if (lvl > _storageData.storageVisualSprites.Count)
            {
                index = _storageData.storageVisualSprites.Count - 1;
            }
            else
            {
                index = lvl - 1;
            }
            
            _buildingSprite.sprite = _storageData.storageVisualSprites[index].buildingsSprites;
            _shadowSprite.sprite = _storageData.storageVisualSprites[index].shadowSprites;
        }
    }
}