namespace Vikings.Building
{
    public class HomeController : StorageController
    {
        public override void Init(BuildingData buildingData)
        {
            base.Init(buildingData);
            BuildingHomeActionController.Instance.OnHomeBuilding();
        }

        public override void UpgradeStorage()
        {
           base.UpgradeStorage();
           BuildingHomeActionController.Instance.OnHomeBuilding();
        }
    }
}

