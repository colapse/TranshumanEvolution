using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="UpgradePart",menuName="TransHuman/New Upgrade Part",order=6)]
public class UpgradePart : ScriptableObject
{
    public string upgradePartName;
    [TextArea] public string upgradePartDescription;
    public Sprite upgradePartIcon;

    public TechUpgradeCategory upgradeCategory;
    public GameObject prefab;
}
