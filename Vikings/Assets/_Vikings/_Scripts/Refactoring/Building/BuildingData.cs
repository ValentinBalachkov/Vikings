using System.Collections.Generic;
using _Vikings._Scripts.Refactoring.Objects;
using UnityEngine;
using Vikings.Building;
using Vikings.Object;

[CreateAssetMenu(fileName = "BuildingData", menuName = "Data/BuildingData")]
public class BuildingData : ScriptableObject
{
    public Sprite icon;
    public string nameText;
    public string description;
    public string required;
    public List<BuildingVisualSprites> buildingVisualSprites;
    public string saveKey;
    public AbstractBuilding prefab;
    public BuildingView _buildingView;
    public TaskData taskData;
}
