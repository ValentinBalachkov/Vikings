using System.Collections.Generic;
using System.Linq;
using Zenject;
using _Vikings.WeaponObject;

namespace _Vikings._Scripts.Refactoring
{
    public class WeaponFactory : MonoInstaller
    {
        public override void InstallBindings()
        {
            AddWeaponFactory();
        }

        public Weapon GetWeapon(WeaponData weaponData)
        {
            var weapons = Container.ResolveAll<Weapon>();
            return weapons.FirstOrDefault(x => x.GetWeaponData() == weaponData);
        }

        public List<Weapon> GetOpenWeapons()
        {
            var weapons = Container.ResolveAll<Weapon>().Where(x => x.Level.Value > 0).ToList();
            return weapons;
        }

        public void CreateWeapons(List<WeaponData> weaponsData)
        {
            foreach (var data in weaponsData)
            {
                var weapon = new Weapon(data);
                Container.Bind<Weapon>().FromInstance(weapon).AsTransient();
            }
        }
        
        private void AddWeaponFactory()
        {
            Container
                .Bind<WeaponFactory>()
                .FromInstance(this)
                .AsSingle()
                .NonLazy();
        }
        
    }
}