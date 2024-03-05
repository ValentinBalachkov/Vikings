using System;
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

        private CharactersConfig _charactersConfig;
        
        private CompositeDisposable _disposable = new();
        
        private CharacterStateMachine _characterStateMachine;

        private void Awake()
        {
            addCharacter.Subscribe(OnAddCharacter).AddTo(_disposable);
            _characterStateMachine = Resources.LoadAll<CharacterStateMachine>("Character")[0];
            _charactersConfig = Resources.LoadAll<CharactersConfig>("Character")[0];
            
            DebugLogger.SendMessage(_charactersConfig.name, Color.green);
            
            for (int i = 0; i < _charactersConfig.charactersCount; i++)
            {
                addCharacter.Execute(_charactersConfig.playerController);
            }
        }

        public override void InstallBindings()
        {
            AddBind();
        }

        public List<CharacterStateMachine> GetCharacters()
        {
            return Container.ResolveAll<CharacterStateMachine>();
        }
        
        private void OnAddCharacter(PlayerController playerController)
        {
            var instance = Container.InstantiatePrefabForComponent<CharacterStateMachine>(_characterStateMachine, _spawnPoint.position, Quaternion.identity, _spawnPoint);
            Container.Bind<CharacterStateMachine>().FromInstance(instance).AsTransient();
            instance.Init(instance.transform, playerController);
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