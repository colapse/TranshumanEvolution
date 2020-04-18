using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName="TechBranch",menuName="TransHuman/New Tech Branch",order=1)]
public class TechBranch : ScriptableObject
{
    public string branchName;
    [TextArea]
    public string branchDescription;
    public Sprite branchIcon;
    public int maxLevels = 5;

    /*
    public bool CheckUpgradeRequirements(Player player)
    {
        return true; // TODO
    }*/
}
