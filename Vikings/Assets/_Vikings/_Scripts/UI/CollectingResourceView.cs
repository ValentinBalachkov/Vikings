using System.Collections.Generic;
using _Vikings._Scripts.Refactoring;
using PanelManager.Scripts.Panels;
using TMPro;
using UnityEngine;
using Vikings.Building;
using Vikings.Object;

public class CollectingResourceView : ViewBase
{
    public override PanelType PanelType => PanelType.Screen;
    public override bool RememberInHistory => false;

    [SerializeField] private TMP_Text _name;

    [SerializeField] private TMP_Text _woodCount;
    [SerializeField] private TMP_Text _rockCount;
    [SerializeField] private RectTransform _rectTransform;

    private Camera _camera;
    private Dictionary<ResourceType, int> _priceForUpgrade = new();

    protected override void OnInitialize()
    {
        base.OnInitialize();
        _camera = Camera.main;
        Clear();
    }

    public void Setup(AbstractBuilding abstractBuilding)
    {
        _priceForUpgrade = abstractBuilding.GetPriceForUpgrade();
        var pos = abstractBuilding.GetPosition();
        var current = abstractBuilding.currentItems;

        _woodCount.text = $"{current[ResourceType.Wood]}/{_priceForUpgrade[ResourceType.Wood]}";
        _rockCount.text = $"{current[ResourceType.Rock]}/{_priceForUpgrade[ResourceType.Rock]}";
        
        _name.text = abstractBuilding.GetData().nameText;
        _rectTransform.position =
            _camera.WorldToScreenPoint(new Vector3(pos.position.x + 0.5f, pos.position.y, pos.position.z - 4f));
        gameObject.SetActive(true);
    }
    
    public void Setup(CraftingTable table)
    {
        _priceForUpgrade = table.GetPriceForUpgrade();
        var pos = table.GetPosition();
        
        var current = table.currentItems;
        _name.text = table.GetData().nameText;
        
        if (table.CurrentWeapon != null)
        {
            current = table.currentItemsWeapon;
            _name.text = table.CurrentWeapon.GetWeaponData().nameText;
        }

        _woodCount.text = $"{current[ResourceType.Wood]}/{_priceForUpgrade[ResourceType.Wood]}";
        _rockCount.text = $"{current[ResourceType.Rock]}/{_priceForUpgrade[ResourceType.Rock]}";
        
        _rectTransform.position =
            _camera.WorldToScreenPoint(new Vector3(pos.position.x + 0.5f, pos.position.y, pos.position.z - 4f));
        gameObject.SetActive(true);
    }

    public void ChangeHeader(string header)
    {
        _name.text = header;
    }


    public void Clear()
    {
        _woodCount.text = $"";
        _rockCount.text = $"";
        gameObject.SetActive(false);
    }

    public void UpdateView(Dictionary<ResourceType, int> currentItems, Dictionary<ResourceType, int> priceForUpgrade)
    {
        _woodCount.text = $"{currentItems[ResourceType.Wood]}/{priceForUpgrade[ResourceType.Wood]}";
        _rockCount.text = $"{currentItems[ResourceType.Rock]}/{priceForUpgrade[ResourceType.Rock]}";
    }
}