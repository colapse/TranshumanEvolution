using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="UpgradeType",menuName="TransHuman/New Upgrade Type",order=2)]
public class TechUpgradeType : ScriptableObject
{
    public string upgradeTypeName;
    [TextArea]
    public string upgradeTypeDescription;
    public Sprite upgradeTypeIcon;
}
