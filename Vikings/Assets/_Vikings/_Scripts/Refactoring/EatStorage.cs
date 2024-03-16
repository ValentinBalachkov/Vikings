namespace _Vikings._Scripts.Refactoring
{
    public class EatStorage : Storage
    {
        public int HouseLevel => _storageDynamicData.CurrentLevel;
    }
}