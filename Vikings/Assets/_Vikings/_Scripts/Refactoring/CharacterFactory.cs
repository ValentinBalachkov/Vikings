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
        public ReactiveCommand<PlayerController> addCharacter = new();

        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private CharacterStateMachine _characterStateMachine;


        private CharactersConfig _charactersConfig;

        private CompositeDisposable _disposable = new();


        private CharacterManager _characterManager;
        

        public override void InstallBindings()
        {
            AddBind();
            
            addCharacter.Subscribe(OnAddCharacter).AddTo(_disposable);
            
        }

        [Inject]
        private void Init(ConfigSetting configSetting)
        {
            _charactersConfig = configSetting.charactersConfig;
            _characterManager = new CharacterManager(_charactersConfig);
        }

        public void SpawnCharacters()
        {
            for (int i = 0; i < _characterManager.charactersCount; i++)
            {
                addCharacter.Execute(_charactersConfig.playerController);
            }
        }
        

        public List<CharacterStateMachine> GetCharacters()
        {
            return Container.ResolveAll<CharacterStateMachine>();
        }
        
        private void OnAddCharacter(PlayerController playerController)
        {
            var instance = Container.InstantiatePrefabForComponent<CharacterStateMachine>(_characterStateMachine, _spawnPoint.position, Quaternion.identity, _spawnPoint);
            Container.Bind<CharacterStateMachine>().FromInstance(instance).AsTransient();
            instance.Init(instance.transform, playerController, _characterManager);
        }
        
        private void AddBind()
        {
            Container
                .Bind<CharacterFactory>()
                .FromInstance(this)
                .AsSingle()
                .NonLazy();
        }
    }
} 