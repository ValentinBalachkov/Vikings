using DG.Tweening;
using UnityEngine;
using Vikings.Chanacter;

namespace Vikings.Building
{
    public class BuildingHomeActionController : MonoBehaviour
    {
        public static BuildingHomeActionController Instance => _instance;
        private static BuildingHomeActionController _instance;
        [SerializeField] private Camera _camera;
        [SerializeField] private CharactersOnMap _charactersOnMap;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
        }

        public void OnHomeBuilding()
        {
            _charactersOnMap.AddCharacterOnMap(1);

            var position = _camera.transform.position;
            _camera.transform.DOMove(new Vector3
            (position.x,
                position.y + 1,
                position.z + 1), 0.01f);
        }
    }
}