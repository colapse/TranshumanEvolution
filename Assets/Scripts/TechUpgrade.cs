using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


[CreateAssetMenu(fileName="TechUpgrade",menuName="TransHuman/New Tech Upgrade",order=0)]
public class TechUpgrade : SerializedScriptableObject
{
    public string upgradeName;
    [TextArea]
    public string upgradeDescription;
    public Sprite upgradeIcon;
    
    public List<TechUpgrade> requiredTechUpgrades;
    public Dictionary<TechBranch, int> requiredTechBranchLevels;

    // === Effects
    //public GameSetupData.WealthLevels minWealthLevel;
    public UpgradePart upgradePart;
    public bool obtainUpgradePart = true;
    
    [Range(-10,10)]
    public int transhumanIndexChange; // Lower or Raise the humanityIndex of the upgradePart that gets upgraded
    [Range(-10,10)]
    public int aestheticTranshumanIndexChange; // Lower or Raise the humanityIndex of the upgradePart that gets upgraded
    /**
     * Example: If the upgradePart.minWealthLevel is MiddleClass(=1) and this value is set to .3,
     * the upgradePart will get more expensive and result in the minWealthLevel of the upgradePart to be
     * UpperClass(=2) [Value will be rounded UP to the next integer].
     * On the other hand, if this value is set to -1, it will get available for the lower class.
     */
    public float minWealthLevelChange;

    public int partTechLevelChange;

    [SerializeField]
    public Dictionary<TechUpgradeType, bool> upgradeTypes;
    
    public bool CheckRequirements(Player player)
    {
        if (player == null) return false;

        bool valid = true;
        if (requiredTechBranchLevels != null)
        {
            foreach (var techBranchLevelReq in requiredTechBranchLevels)
            {
                if (player.GetCurrentTechBranchLevel(techBranchLevelReq.Key) < techBranchLevelReq.Value)
                    valid = false;
            }
        }
        
        var cost = player.gameSetupData.GetTechUpgradeCost(this);
        if (!player.HasSufficientFunds(cost)) valid = false;

        if (requiredTechUpgrades != null)
        {
            foreach (var reqTechUpgrade in requiredTechUpgrades)
            {
                if (!player.techUpgrades?.Contains(reqTechUpgrade) ?? false) valid = false;
            }
        }
        
        return valid;
    }
}
