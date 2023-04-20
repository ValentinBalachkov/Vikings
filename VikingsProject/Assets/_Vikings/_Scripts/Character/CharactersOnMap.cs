using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vikings.Building;
using Vikings.Weapon;

namespace Vikings.Chanacter
{
    public class CharactersOnMap : MonoBehaviour
    {
        public List<CharacterStateMachine> CharactersList => _charactersOnMap;

        [SerializeField] private BuildingsOnMap _buildingsOnMap;
        [SerializeField] private WeaponController _weaponController;
        [SerializeField] private BoneFireController _boneFireController;

        [SerializeField] private Transform _characterSpawnPoint;
        [SerializeField] private CharacterStateMachine[] _characterStateMachine;

        private List<CharacterStateMachine> _charactersOnMap = new();


        private void Start()
        {
            AddCharacterOnMap(0);
        }

        public void AddCharacterOnMap(int index)
        {
            var character = Instantiate(_characterStateMachine[index]);
            character.SpawnCharacter(_characterSpawnPoint);
            character.Init(_buildingsOnMap, _weaponController, _boneFireController);
            _charactersOnMap.Add(character);
        }
    }
}