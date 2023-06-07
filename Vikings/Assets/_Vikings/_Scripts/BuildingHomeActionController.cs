﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vikings.Chanacter;

namespace Vikings.Building
{
    public class BuildingHomeActionController : MonoBehaviour
    {
        public static BuildingHomeActionController Instance => _instance;
        private static BuildingHomeActionController _instance;
        [SerializeField] private Camera _camera;
        [SerializeField] private Camera _cameraPassive;
        [SerializeField] private CharactersOnMap _charactersOnMap;
        [SerializeField] private CharactersConfig _charactersConfig;
        [SerializeField] private HouseCameraPositionInfo[] _houseCameraPosition;
        [SerializeField] private List<Transform> _allRespawnPointTransform = new();
        private float _cameraScale;
        public Action<int> OnHomeLevelUp;


        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
           

            _cameraScale = _camera.orthographicSize / _cameraPassive.orthographicSize;
        }

        private void Start()
        {
            if (_charactersConfig.houseLevel != 0)
            {
                StartCoroutine(MoveCameraCoroutine(_houseCameraPosition[_charactersConfig.houseLevel]));
            }

            CollectingResourceView.Instance.camera = _camera;
            
            float scaleMove = _houseCameraPosition[0].size / _houseCameraPosition[_charactersConfig.houseLevel].size;
            foreach (var transform in _allRespawnPointTransform)
            {
                Debug.Log(scaleMove.ToString());
                transform.localScale*=scaleMove;
            }
        }

        public void OnHomeBuilding()
        {
            _charactersOnMap.AddCharacterOnMap(0);
            _charactersConfig.charactersCount++;
            if (_charactersConfig.houseLevel >= 10) return;
            _charactersConfig.houseLevel++;
            OnHomeLevelUp?.Invoke(_charactersConfig.houseLevel);
            StartCoroutine(MoveCameraCoroutine(_houseCameraPosition[_charactersConfig.houseLevel]));
           
        }

        private IEnumerator MoveCameraCoroutine(HouseCameraPositionInfo positionInfo)
        {
            while (_camera.orthographicSize < positionInfo.size)
            {
                float startSize = _camera.orthographicSize;
                _camera.orthographicSize += 0.01f;
                _cameraPassive.orthographicSize = _camera.orthographicSize/_cameraScale;
                float scaleMove = startSize / _camera.orthographicSize;
               
                foreach (var transform in _allRespawnPointTransform)
                {
                    transform.localScale.Set(transform.localScale.x*scaleMove,transform.localScale.y*scaleMove,transform.localScale.z*scaleMove);
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