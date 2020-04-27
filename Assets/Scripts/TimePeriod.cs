using System.Collections.Generic;
using Reporting;
using UnityEngine;

public class TimePeriod
{
    public TimePeriod previousPeriod;
    public int newAddedBudget;
    public int totalStartBudget;
    public int spentMoney;
    public List<TechUpgrade> obtainedTechUpgrades = new List<TechUpgrade>();
    public Dictionary<TechBranch, int> obtainedTechBranchLevels = new Dictionary<TechBranch, int>();
    public Report report;

    public Report GenerateReport(Player player)
    {
        report = new Report(player, this);
        
        return report;
    }
    
    public int GetSpentMoneyToThisPeriod()
    {
        return (previousPeriod?.GetSpentMoneyToThisPeriod() ?? 0) + spentMoney;
    }

    public int GetCurrentTimePeriodLevel()
    {
        return (previousPeriod?.GetCurrentTimePeriodLevel() ?? 0) + 1;
    }

    public int GetYear(GameSetupData gameSetupData)
    {
        return gameSetupData.timePhaseStartYear + GetYearsSinceStart(gameSetupData);
    }

    public int GetYearsSinceStart(GameSetupData gameSetupData)
    {
        return (GetCurrentTimePeriodLevel() - 1) * gameSetupData.timePhaseDuration;
    }
}