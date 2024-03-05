using System;
using System.Collections.Generic;
using _Vikings.Refactoring.Character;
using UnityEngine;
using Vikings.Object;

public class BoneFire : AbstractObject, IAcceptArgs<List<BoneFirePositionData>>
{
    private List<BoneFirePositionData> _positionsData;
    public override void Init()
    {
        
    }

    public override Transform GetPosition()
    {
        foreach (var pos in _positionsData)
        {
            if (!pos.isDisable)
            {
                pos.isDisable = true;
                return pos.point;
            }
        }

        return _positionsData[0].point;
    }

    public override void CharacterAction(CharacterStateMachine characterStateMachine)
    {
        
    }
    

    public void AcceptArg(List<BoneFirePositionData> arg)
    {
        _positionsData = new List<BoneFirePositionData>(arg);
    }
}

[Serializable]
public class BoneFirePositionData
{
    public int id;
    public Transform point;
    public bool isDisable;
}
