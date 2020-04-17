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

    [Range(0,10)]
    public int humanityIndex;
    public GameSetupData.WealthLevels minWealthLevel;
    public List<UpgradePart> unlockUpgradeParts;

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
