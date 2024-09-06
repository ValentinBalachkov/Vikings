using UnityEngine;

namespace _Vikings._Scripts.Refactoring.Objects
{
    public class BuildingView : MonoBehaviour
    {
        private GameObject _model;
        private BuildingData _buildingData;

        public void Init(BuildingData storageData)
        {
            _buildingData = storageData;
        }
        
        
        public void SetupSprite(int lvl)
        {
            if(lvl == 0) return;
            
            int index;

            if (lvl > _buildingData.buildingModel.Count)
            {
                index = _buildingData.buildingModel.Count - 1;
            }
            else
            {
                index = lvl - 1;
            }

            if (_model != null)
            {
                Destroy(_model.gameObject);
            }
            
            _model = Instantiate(_buildingData.buildingModel[index], transform);
        }
    }
}