using System;
using System.Collections;
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
        [SerializeField] private HouseCameraPositionInfo[] _houseCameraPosition;
        public Action<int> OnHomeLevelUp;


        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
        }

        private void Start()
        {
            if (_charactersConfig.houseLevel != 0)
            {
                StartCoroutine(MoveCameraCoroutine(_houseCameraPosition[_charactersConfig.houseLevel]));
            }
        }

        public void OnHomeBuilding()
        {
            _charactersOnMap.AddCharacterOnMap(1);
            _charactersConfig.charactersCount++;
            if (_charactersConfig.houseLevel >= 10) return;
            _charactersConfig.houseLevel++;
            OnHomeLevelUp?.Invoke(_charactersConfig.houseLevel);
            StartCoroutine(MoveCameraCoroutine(_houseCameraPosition[_charactersConfig.houseLevel]));
        }

        private IEnumerator MoveCameraCoroutine(HouseCameraPositionInfo positionInfo)
        {
           // _camera.transform.localPosition = positionInfo.position;
            while (_camera.orthographicSize < positionInfo.size)
            {
                _camera.orthographicSize += 0.01f;
                yield return null;
            }
        }
    }

    [Serializable]
    public class HouseCameraPositionInfo
    {
        public Vector3 position;
        public float size;
    }
}