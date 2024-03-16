using System.Collections.Generic;
using System.Linq;
using _Vikings._Scripts.Refactoring;
using UnityEngine;
using Vikings.Items;
using Vikings.Object;
using Zenject;
using AbstractBuilding = Vikings.Object.AbstractBuilding;

namespace Vikings.Map
{
    public class MapFactory : MonoInstaller
    {
        [SerializeField] private List<MapResourceData> mapResourceData;
        [SerializeField] private List<MapBuildingData> mapBuildingData;

        [Space(10)]
        [SerializeField] private List<BoneFirePositionData> _boneFirePositionData;

        [SerializeField] private BoneFire _boneFire;
        [SerializeField] private Transform _boneFireSpawnPoint;
        

        public override void InstallBindings()
        {
            AddMapSpawner();
            CreateBoneFire();
        }

        public void CreateBuilding(BuildingData buildingData)
        {
            var pos = mapBuildingData.FirstOrDefault(x => x.data == buildingData);
            if (pos == null)
            {
                Debug.LogError("Building not found!");
                return;
            }

            var building = CreateObject<AbstractBuilding>(pos.data.prefab, pos.positions);
            building.SetData(buildingData);
            building.Init();
        }

        public void CreateResource(int level, ItemData itemData)
        {
            var data = mapResourceData.FirstOrDefault(x => x.resourceConfig == itemData);

            if (data == null)
            {
                Debug.LogError("Resource config not found!");
                return;
            }

            var posLevel = data.levelPosition.FirstOrDefault(x => x.level == level);

            if(posLevel == null)
            {
                Debug.LogError("Resource position not found!");
                return;
            }

            foreach (var pos in posLevel.positions)
            {
                var resource = CreateObject<AbstractResource>(data.abstractResource, pos);
                resource.SetItemData(data.resourceConfig);
                resource.Init();
            }
        }
        
        public List<AbstractResource> GetAllResources()
        {
            var abstractResources = Container.ResolveAll<AbstractResource>();
            return abstractResources;
        }

        public Storage GetPartialStorage()
        {
            var storages = Container.ResolveAll<Storage>();

            foreach (var storageAbstract in storages)
            {
                var storage = storageAbstract;

                if (storage.CheckNeededItem())
                {
                    return storage;
                }
            }

            return null;
        }

        public BoneFire GetBoneFire()
        {
            return Container.Resolve<BoneFire>();
        }

        public List<T> GetAllBuildings<T>() where T : AbstractBuilding
        {
            var buildings = Container.ResolveAll<AbstractBuilding>();

            List<T> list = new();

            foreach (var building in buildings)
            {
                if (building is T abstractBuilding)
                {
                    list.Add(abstractBuilding);
                }
            }

            return list;
        }

        private void CreateBoneFire()
        {
            var building = CreateObject<BoneFire>(_boneFire, _boneFireSpawnPoint);
            building.Init();
            building.AcceptArg(_boneFirePositionData);
        }

        private T CreateObject<T>(AbstractObject obj, Transform pos) where T : AbstractObject
        {
            var instance = Container.InstantiatePrefabForComponent<T>(obj, pos.position, Quaternion.identity, pos);
            Container.Bind<T>().FromInstance(instance).AsTransient();

            return instance;
        }

        private void AddMapSpawner()
        {
            Container
                .Bind<MapFactory>()
                .FromInstance(this)
                .AsSingle()
                .NonLazy();
        }
    }
}