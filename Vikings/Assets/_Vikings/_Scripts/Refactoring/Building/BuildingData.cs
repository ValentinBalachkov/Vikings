using System.Collections.Generic;
using _Vikings._Scripts.Refactoring.Objects;
using UnityEngine;
using Vikings.Building;
using Vikings.Object;
using ItemCount = _Vikings.Refactoring.Character.ItemCount;

[CreateAssetMenu(fileName = "BuildingData", menuName = "Data/BuildingData")]
public class BuildingData : ScriptableObject
{
    public int priorityToAction;
    public Sprite icon;
    public string nameText;
    public string description;
    public string required;
    public Sprite requiredSprite;
    public List<GameObject> buildingModel;
    public string saveKey;
    public AbstractBuilding prefab;
    public BuildingView _buildingView;
    public List<ItemCount> priceToUpgrades = new();
    public TaskData taskData;
    public int priorityInMenu;
    public float stoppingDistance;
}
