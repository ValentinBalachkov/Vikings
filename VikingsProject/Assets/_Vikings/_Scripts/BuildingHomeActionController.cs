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
        [SerializeField] private CharactersConfig _charactersConfig;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
        }

        private void Start()
        {
            if (_charactersConfig.cameraPositionY != 0)
            {
                _camera.transform.position = new Vector3
                (_camera.transform.position.x,
                    _charactersConfig.cameraPositionY,
                    _charactersConfig.cameraPositionZ);
            }
        }

        public void OnHomeBuilding()
        {
            _charactersOnMap.AddCharacterOnMap(1);
            _charactersConfig.charactersCount++;

            var position = _camera.transform.position;
            _camera.transform.DOMove(new Vector3
            (position.x,
                position.y + 1,
                position.z + 1), 0.01f);

            _charactersConfig.cameraPositionY = _camera.transform.position.y;
            _charactersConfig.cameraPositionZ = _camera.transform.position.z;
        }
    }
}