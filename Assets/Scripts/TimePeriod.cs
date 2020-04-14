using System.Collections.Generic;
using UnityEngine;

public class TimePeriod
{
    public TimePeriod previousPeriod;
    public int budget;
    public int spentBudget;
    public List<TechUpgrade> obtainedTechUpgrades;
    public Dictionary<TechBranch, int> obtainedTechBranchLevels;
    
    
}