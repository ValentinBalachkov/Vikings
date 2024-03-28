using System;
using System.Collections;
using System.Collections.Generic;
using _Vikings._Scripts.Refactoring;
using UnityEngine;
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

        private EatStorage _eatStorage;

        private float _cameraScale;

        private void Awake()
        {
            _cameraScale = _camera.orthographicSize / _cameraPassive.orthographicSize;
        }

        [Inject]
        public void Init(CharacterFactory characterFactory)
        {
            _charactersOnMap = characterFactory;
        }

        private void OnDestroy()
        {
            _eatStorage.OnHomeBuilding -= OnHomeBuilding;
        }

        public void Init(EatStorage eatStorage)
        {
            _eatStorage = eatStorage;
            _eatStorage.OnHomeBuilding += OnHomeBuilding;
            
            if (_eatStorage.CurrentLevel.Value != 0)
            {
                StartCoroutine(MoveCameraCoroutine(_houseCameraPosition[_eatStorage.CurrentLevel.Value]));
            }

            float scaleMove = _houseCameraPosition[0].size / _houseCameraPosition[_eatStorage.CurrentLevel.Value].size;
            foreach (var t in _allRespawnPointTransform)
            {
                t.localScale *= scaleMove;
            }
        }

        private void OnHomeBuilding()
        {
            if (_eatStorage.CurrentLevel.Value >= 5) return;
            _charactersOnMap.addCharacter.Execute(false);
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

                foreach (var t in _allRespawnPointTransform)
                {
                    t.localScale.Set(t.localScale.x * scaleMove, t.localScale.y * scaleMove,
                        t.localScale.z * scaleMove);
                }

                yield return null;
            }
        }
    }

    [Serializable]
    public class HouseCameraPositionInfo
    {
        public float size;
    }
}