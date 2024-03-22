using System;
using System.Collections.Generic;
using System.Linq;
using _Vikings.Refactoring.Character;
using PanelManager.Scripts.Interfaces;
using UnityEngine;
using Vikings.Object;

public class BoneFire : AbstractObject, IAcceptArg<List<BoneFirePositionData>>
{
    private List<BoneFirePositionData> _positionsData;
    public override void Init()
    {
        
    }

    public override float GetStoppingDistance()
    {
        return 0.5f;
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
        characterStateMachine.SetIdleAnimation();
        characterStateMachine.ResetDestinationForLook(transform);
    }

    public void ResetFlag(Transform pos)
    {
        var data = _positionsData.FirstOrDefault(x => x.point == pos);
        data.isDisable = false;
    }
    

    public void AcceptArg(List<BoneFirePositionData> arg)
    {
        _positionsData = new List<BoneFirePositionData>(arg);
    }
}

[Serializable]
public class BoneFirePositionData
{
    public Transform point;
    public bool isDisable;
}
