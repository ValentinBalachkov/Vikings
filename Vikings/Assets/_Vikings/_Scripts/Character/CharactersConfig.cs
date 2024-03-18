using UnityEngine;

namespace Vikings.Chanacter
{
    [CreateAssetMenu(fileName = "CharactersConfig", menuName = "Data/CharactersConfig", order = 9)]
    public class CharactersConfig : ScriptableObject
    {
        public PlayerController playerController;
        
        public float speed_up;

        public TaskData taskDataBackpack;

        public string saveKey;
     

        public float speedMove = 1;
        public int speedMoveCost = 14;

        public int speedWork = 1;
        public int speedWorkCost = 17;

        public int itemsCount = 2;
        public int itemsCountCost = 10;

        public int actionCount;
    }
}