using System.Collections.Generic;
using UnityEngine;

public class TimePeriod
{
    public TimePeriod previousPeriod;
    public int newAddedBudget;
    public int totalStartBudget;
    public int spentMoney;
    public List<TechUpgrade> obtainedTechUpgrades;
    public Dictionary<TechBranch, int> obtainedTechBranchLevels;

    public int GetSpentMoneyToThisPeriod()
    {
        return (previousPeriod?.GetSpentMoneyToThisPeriod() ?? 0) + spentMoney;
    }

    public int GetCurrentTimePeriodLevel()
    {
        return (previousPeriod?.GetCurrentTimePeriodLevel() ?? 0) + 1;
    }
}