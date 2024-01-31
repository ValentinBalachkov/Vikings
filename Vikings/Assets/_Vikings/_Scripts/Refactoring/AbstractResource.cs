using UnityEngine;
using Vikings.Object;

namespace _Vikings._Scripts.Refactoring
{
    public class AbstractResource : AbstractObject
    {
        public override Transform GetPosition()
        {
            return transform;
        }

        public override void CharacterAction()
        {
            
        }

        public override void Init()
        {
            
        }
    }
}