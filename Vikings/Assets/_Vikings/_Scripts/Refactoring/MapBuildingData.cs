using System;
using _Vikings._Scripts.Refactoring;
using UnityEngine;
using Vikings.Items;
using AbstractBuilding = Vikings.Object.AbstractBuilding;


namespace Vikings.Map
{
   [Serializable]
    public class MapResourceData
    {
        public AbstractResource abstractResource;
        public ItemData resourceConfig;
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
        public BuildingData data;
        public Transform positions;
    }
    
    
}