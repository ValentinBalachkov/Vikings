using System.Collections;
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
                StartCoroutine(MoveCameraCoroutine(_charactersConfig.cameraPositionY));
            }
        }

        public void OnHomeBuilding()
        {
            _charactersOnMap.AddCharacterOnMap(1);
            _charactersConfig.charactersCount++;
            float posY = _camera.orthographicSize + 1;

            _charactersConfig.cameraPositionY = posY;
            StartCoroutine(MoveCameraCoroutine(posY));
         }
        private IEnumerator MoveCameraCoroutine(float position)
        {
            while (_camera.orthographicSize < position)
            {
                _camera.orthographicSize += 0.01f;
                yield return null;
            }
        }
    }
}