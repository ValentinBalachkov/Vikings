using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Vikings._Scripts.Refactoring;
using UnityEngine;
using Vikings.Map;
using Zenject;

namespace Vikings.Building
{
    public class BuildingHomeActionController : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Camera _cameraPassive;
        [SerializeField] private HouseCameraPositionInfo[] _houseCameraPosition;
        [SerializeField] private List<Transform> _allRespawnPointTransform = new();

        private CharacterFactory _charactersOnMap;
        private MapFactory _mapFactory;

        private EatStorage _eatStorage;

        private float _cameraScale;

        private void Awake()
        {
            _cameraScale = _camera.orthographicSize / _cameraPassive.orthographicSize;
        }

        [Inject]
        public void Init(CharacterFactory characterFactory, MapFactory mapFactory)
        {
            _charactersOnMap = characterFactory;
            _mapFactory = mapFactory;
            _eatStorage = _mapFactory.GetAllBuildings<EatStorage>().FirstOrDefault();
            _eatStorage.OnHomeBuilding += OnHomeBuilding;
        }

        private void OnDestroy()
        {
            _eatStorage.OnHomeBuilding -= OnHomeBuilding;
        }

        private void Start()
        {
            if (_eatStorage.CurrentLevel.Value != 0)
            {
                StartCoroutine(MoveCameraCoroutine(_houseCameraPosition[_eatStorage.CurrentLevel.Value]));
            }

            CollectingResourceView.Instance.camera = _camera;

            float scaleMove = _houseCameraPosition[0].size / _houseCameraPosition[_eatStorage.CurrentLevel.Value].size;
            foreach (var transform in _allRespawnPointTransform)
            {
                transform.localScale *= scaleMove;
            }
        }

        public void OnHomeBuilding()
        {
            if (_eatStorage.CurrentLevel.Value >= 5) return;
            _charactersOnMap.addCharacter.Execute();
            StartCoroutine(MoveCameraCoroutine(_houseCameraPosition[_eatStorage.CurrentLevel.Value]));
        }

        private IEnumerator MoveCameraCoroutine(HouseCameraPositionInfo positionInfo)
        {
            while (_cameraPassive.orthographicSize < positionInfo.size)
            {
                float startSize = _camera.orthographicSize;
                _camera.orthographicSize += 0.01f;
                _cameraPassive.orthographicSize = _camera.orthographicSize / _cameraScale;
                float scaleMove = startSize / _camera.orthographicSize;

                foreach (var transform in _allRespawnPointTransform)
                {
                    transform.localScale.Set(transform.localScale.x * scaleMove, transform.localScale.y * scaleMove,
                        transform.localScale.z * scaleMove);
                }

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