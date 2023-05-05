namespace Vikings.Building
{
    public class HomeController : StorageController
    {
        public override void Init(BuildingData buildingData, bool isSaveInit = false)
        {
            base.Init(buildingData, isSaveInit);
            BuildingHomeActionController.Instance.OnHomeBuilding();
        }

        public override void UpgradeStorage()
        {
           base.UpgradeStorage();
           BuildingHomeActionController.Instance.OnHomeBuilding();
        }
    }
}

