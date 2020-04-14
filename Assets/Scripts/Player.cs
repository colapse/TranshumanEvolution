using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameSetupData gameSetupData;
    
    public Dictionary<TechBranch, int> techBranchLevels;
    public List<TechUpgrade> techUpgrades;
    public List<UpgradePart> upgradeParts;

    public Transhuman transhuman;
    
    public List<TimePeriod> timePeriodsData;
    public TimePeriod currentTimePeriod;

    public void EndCurrentTimePeriod()
    {
        //TODO
    }

    public void InitData()
    {
        transhuman = FindObjectOfType<Transhuman>();
        UpdateTranshumanParts();
        
    }

    
    [Button]
    public void UpdateTranshumanParts()
    {
        if (transhuman != null && upgradeParts != null)
        {
            foreach (var upgradePart in upgradeParts)
            {
                // TODO What if there are two upgradeParts of the same category? How can I determine which is the latest?
                transhuman.SwapActiveUpgradePart(upgradePart);
            }
        }
        //transhuman?.UpdateActiveUpgradeParts();
    }
}