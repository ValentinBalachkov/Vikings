using System.Collections.Generic;
using System.Linq;
using _Vikings._Scripts.Refactoring;
using UnityEngine;
using Vikings.Chanacter;
using Vikings.Object;

namespace Vikings.Map
{
    public class MapSpawner : MonoBehaviour
    {
        [SerializeField] private List<MapResourceData> mapResourceData;
        [SerializeField] private List<MapBuildingData> mapBuildingData;
        [SerializeField] private CharactersConfig _charactersConfig;


        private List<AbstractResource> _abstractResources = new();
        private List<AbstractBuilding> _abstractBuildings = new();

        public void CreateBuilding<T>(BuildingType buildingType) where T : AbstractBuilding
        {
            var mapBuilding = mapBuildingData.FirstOrDefault(x => x.abstractBuilding is T);
            var pos = mapBuilding?.buildingPositions.FirstOrDefault(x => x.type == buildingType);
            if (mapBuilding == null || pos == null)
            {
                Debug.LogError("Building not found!");
                return;
            }

            var building = CreateObject<AbstractBuilding>(mapBuilding.abstractBuilding, pos.positions);
            building.FindData(buildingType);
        }

        public void CreateResource(int level)
        {
            foreach (var data in mapResourceData)
            {
                foreach (var posLevel in data.levelPosition)
                {
                    if (posLevel.level <= level)
                    {
                        foreach (var pos in posLevel.positions)
                        {
                            _abstractResources.Add(CreateObject<AbstractResource>(data.objectOnMap, pos));
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private T CreateObject<T>(AbstractObject obj, Transform pos) where T : AbstractObject
        {
            var objectOnMap = Instantiate(obj, pos);
            objectOnMap.Init();
            return objectOnMap as T;
        }
    }
}