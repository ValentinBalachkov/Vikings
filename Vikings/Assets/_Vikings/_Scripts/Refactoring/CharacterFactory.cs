using System.Collections.Generic;
using _Vikings.Refactoring.Character;
using UniRx;
using UnityEngine;
using Vikings.Chanacter;
using Zenject;

namespace _Vikings._Scripts.Refactoring
{
    public class CharacterFactory : MonoInstaller
    {
        public CharacterManager CharacterManager => _characterManager;
        public ReactiveCommand addCharacter = new();
        
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private CharacterStateMachine _characterStateMachine;

        private CharactersConfig _charactersConfig;

        private CompositeDisposable _disposable = new();

        private CharacterManager _characterManager;
        private WeaponFactory _weaponFactory;

        public override void InstallBindings()
        {
            AddCharacterFactory();
        }
        
        public void Init(ConfigSetting configSetting, WeaponFactory weaponFactory, int charactersCount)
        {
            _charactersConfig = configSetting.charactersConfig;
            _characterManager = new CharacterManager(_charactersConfig);
            _weaponFactory = weaponFactory;
            addCharacter.Subscribe(_ => OnAddCharacter()).AddTo(_disposable);
            
            SpawnCharacters(charactersCount);
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
        }

        private void AddCharacterFactory()
        {
            Container
                .Bind<CharacterFactory>()
                .FromInstance(this)
                .AsSingle()
                .NonLazy();
        }

        public void AddCharactersCount()
        {
            _characterManager.AddCharacter();
        }

        private void SpawnCharacters(int count)
        {
            for (int i = 0; i < count; i++)
            {
                addCharacter.Execute();
            }
        }

        public List<CharacterStateMachine> GetCharacters()
        {
            return Container.ResolveAll<CharacterStateMachine>();
        }
        
        private void OnAddCharacter()
        {
            var instance = Container.InstantiatePrefabForComponent<CharacterStateMachine>(_characterStateMachine, _spawnPoint.position, Quaternion.identity, _spawnPoint);
            Container.Bind<CharacterStateMachine>().FromInstance(instance).AsTransient();
            instance.Init(instance.transform, _charactersConfig.playerController, _characterManager, _weaponFactory);
        }
        
    }
} 