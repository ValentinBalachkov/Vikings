using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialConfig", menuName = "Data/TutorialConfig")]
public class TutorialConfig : ScriptableObject
{
    public List<TutorialSteps> _tutorialStepsList = new();
}
