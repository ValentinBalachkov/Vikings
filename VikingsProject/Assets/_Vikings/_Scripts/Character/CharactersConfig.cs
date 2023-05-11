using SecondChanceSystem.SaveSystem;
using UnityEngine;

namespace Vikings.Chanacter
{
    [CreateAssetMenu(fileName = "CharactersConfig", menuName = "Data/CharactersConfig", order = 9)]
    public class CharactersConfig : ScriptableObject, IData
    {
        public int charactersCount;
        public float cameraPositionY;
        public float cameraPositionZ;
        
        public void Save()
        {
            SaveLoadSystem.SaveData(this);
        }

        public void Load()
        {
            var data = SaveLoadSystem.LoadData(this) as CharactersConfig;
            if (data != null)
            {
                charactersCount = data.charactersCount;
                cameraPositionY = data.cameraPositionY;
                cameraPositionZ = data.cameraPositionZ;
            }
        }
    }
}