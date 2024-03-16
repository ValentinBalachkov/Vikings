using System;

namespace _Vikings._Scripts.Refactoring
{
    public class EatStorage : Storage
    {
        public event Action OnHomeBuilding;
        public override void Upgrade()
        {
            base.Upgrade();
            OnHomeBuilding?.Invoke();
        }
    }
}