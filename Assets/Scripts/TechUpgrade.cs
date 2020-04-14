using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName="TechUpgrade",menuName="TransHuman/New Tech Upgrade",order=0)]
public class TechUpgrade : ScriptableObject
{
    public string upgradeName;
    [TextArea]
    public string upgradeDescription;
    public Sprite upgradeIcon;
    
    public List<TechUpgrade> requiredTechUpgrades;
    public int requiredMechLevel = 0;
    public int requiredBioLevel = 0;

    [Range(0,10)]
    public int humanityIndex;
    public GameSetupData.WealthLevels minWealthLevel;
    public List<UpgradePart> unlockUpgradeParts;
    
    public bool CheckRequirements(Player player)
    {
        //TODO
        return true;
    }
}
