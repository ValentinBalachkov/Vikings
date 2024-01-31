using System;
using _Vikings._Scripts.Refactoring;
using SecondChanceSystem.SaveSystem;
using UnityEngine;
using Vikings.Building;
using Vikings.Object;
using AbstractBuilding = Vikings.Object.AbstractBuilding;


namespace Vikings.Map
{
   [Serializable]
    public class MapResourceData
    {
        public AbstractResource objectOnMap;
        public MapResourceLevelPosition[] levelPosition;
        
        [Serializable]
        public class MapResourceLevelPosition
        {
            public Transform[] positions;
            public int level;
        }
    }
    
    
    
    [Serializable]
    public class MapBuildingData
    {
        public AbstractBuilding abstractBuilding;
        public MapBuildingPosition[] buildingPositions;
        
        [Serializable]
        public class MapBuildingPosition
        {
            public BuildingType type;
            public Transform positions;
        }
    }
    
    
}