using UnityEngine;

namespace _Vikings._Scripts.Refactoring.Objects
{
    public class BuildingView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _buildingSprite;
        [SerializeField] private SpriteRenderer _shadowSprite;

        private BuildingData _buildingData;

        public void Init(BuildingData storageData)
        {
            _buildingData = storageData;
        }
        
        
        public void SetupSprite(int lvl)
        {
            if(lvl == 0) return;
            
            int index;

            if (lvl > _buildingData.buildingVisualSprites.Count)
            {
                index = _buildingData.buildingVisualSprites.Count - 1;
            }
            else
            {
                index = lvl - 1;
            }
            
            _buildingSprite.sprite = _buildingData.buildingVisualSprites[index].buildingsSprites;
            _shadowSprite.sprite = _buildingData.buildingVisualSprites[index].shadowSprites;
        }
    }
}