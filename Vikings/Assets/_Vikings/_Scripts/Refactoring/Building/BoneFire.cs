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

    public override void Init(MainPanelManager mainPanelManager)
    {
    }

    public override float GetStoppingDistance()
    {
        return -0.1f;
    }

    public override Transform GetPosition()
    {
        foreach (var pos in _positionsData)
        {
            if (pos.character == null)
            {
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

    public void SetFlagState(Transform pos, CharacterStateMachine character)
    {
        var data = _positionsData.FirstOrDefault(x => x.point == pos);
        if (data != null)
        {
            data.character = character;
        }
    }

    public void SetFlagState(CharacterStateMachine character)
    {
        var data = _positionsData.FirstOrDefault(x => x.character == character);
        if (data != null)
        {
            data.character = null;
        }
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
    public CharacterStateMachine character;
}