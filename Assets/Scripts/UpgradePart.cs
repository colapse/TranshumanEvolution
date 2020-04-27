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

    [Range(0, 3)] public int security; // 0 = No security, 1 = somewhat secure, 2 = secure, 3 = Unhackable
    [Range(0, 3)] public int connectiveness; // 0 = No connection, 1 = near connection (Phone, BT..), 2 = +Mobile Network, 3 = +Satelite
    [Range(0, 3)] public int physicalAggressionThreat; // 0 = No threat, 1 = Low Risk, 2 = Can cause medium damage, 3 = Big threat (riots, robbery, attacks)
    
    
}
