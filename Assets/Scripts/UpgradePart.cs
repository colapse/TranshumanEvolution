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

    public int techLevel; // Determines how advanced it is. The higher the more advanced.
    public GameSetupData.WealthLevels minWealthLevel;
    [Range(0,10)]
    public int transhumanIndex; // 0 = Human original, 10 = Not human at all
    [Range(0,10)]
    public int aestheticTranshumanIndex; // 0 = Very humane, 10 = very Transhuman/robotic looking
}
