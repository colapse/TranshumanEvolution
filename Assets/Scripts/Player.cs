using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class Player : MonoBehaviour
{
    public event Action MoneySpentEvent;
    public event Action<TechBranch> TechBranchPointIncreasedEvent;
    public event Action<TimePeriod,TimePeriod> TimePeriodChangedEvent; // OldTimePeriod, NewTimePeriod
    public event Action<TechUpgrade> NewTechUpgradeResearchedEvent;
    
    public GameSetupData gameSetupData;
    
    public Dictionary<TechBranch, int> techBranchLevels = new Dictionary<TechBranch, int>();
    public List<TechUpgrade> techUpgrades = new List<TechUpgrade>();
    public List<ObtainedUpgradePart> obtainedUpgradeParts = new List<ObtainedUpgradePart>();

    public Transhuman transhuman;
    
    public List<TimePeriod> timePeriodsData = new List<TimePeriod>();
    public TimePeriod currentTimePeriod;

    [field: ShowInInspector]
    public int AvailableBudget { get; private set; }

    
    
    [ShowInInspector, ReadOnly]
    public int SpentMoneyThisTimePeriod => currentTimePeriod?.spentMoney ?? 0;
    
    [ShowInInspector]
    public int SpentMoneyAllTimePeriods => currentTimePeriod?.GetSpentMoneyToThisPeriod() ?? 0;

    public void AdvanceToNextTimePeriod()
    {
        //TODO
        TimePeriod newTimePeriod = new TimePeriod
        {
            previousPeriod = currentTimePeriod, newAddedBudget = gameSetupData.budgetPerPhase
        };

        AvailableBudget = gameSetupData.budgetPerPhase + (gameSetupData.canTakeOverLeftBudget ? AvailableBudget : 0);
        newTimePeriod.totalStartBudget = AvailableBudget;

        if (currentTimePeriod != null)
        {
            currentTimePeriod?.GenerateReport(this);
            timePeriodsData.Add(currentTimePeriod);
        }

        var oldTimePeriod = currentTimePeriod;
        currentTimePeriod = newTimePeriod;
        TimePeriodChangedEvent?.Invoke(oldTimePeriod,currentTimePeriod);
    }

    public void InitData()
    {
        transhuman = FindObjectOfType<Transhuman>();
        UpdateTranshumanParts();
    }

    public bool BuyTechBranchPoint(TechBranch techBranch)
    {
        if(techBranchLevels == null) techBranchLevels = new Dictionary<TechBranch, int>();
        var nextTechBranchLevel = techBranchLevels.ContainsKey(techBranch) ? techBranchLevels[techBranch] + 1 : 1;
        var cost = gameSetupData.GetTechBranchCost(techBranch, nextTechBranchLevel);

        if (SpendMoney(cost))
        {
            if (techBranchLevels.ContainsKey(techBranch))
                techBranchLevels[techBranch] = nextTechBranchLevel;
            else
                techBranchLevels.Add(techBranch,nextTechBranchLevel);

            if (currentTimePeriod != null)
            {
                if (currentTimePeriod.obtainedTechBranchLevels.ContainsKey(techBranch))
                    currentTimePeriod.obtainedTechBranchLevels[techBranch]++;
                else
                    currentTimePeriod.obtainedTechBranchLevels.Add(techBranch,1);
            }
            
            TechBranchPointIncreasedEvent?.Invoke(techBranch);
            return true;
        }
        
        return false;
    }

    public bool CanBuyTechBranchPoint(TechBranch techBranch)
    {
        if(techBranchLevels == null) techBranchLevels = new Dictionary<TechBranch, int>();
        var currentTechBranchLevel = techBranchLevels.ContainsKey(techBranch) ? techBranchLevels[techBranch] : 0;
        var cost = gameSetupData.GetTechBranchCost(techBranch, currentTechBranchLevel+1);

        return HasSufficientFunds(cost) && currentTechBranchLevel < gameSetupData.maxTechBranchLevel;
    }

    public bool BuyTechUpgrade(TechUpgrade techUpgrade)
    {
        if(techUpgrades == null) techUpgrades = new List<TechUpgrade>();
        if (techUpgrades.Contains(techUpgrade)) return false;

        var cost = gameSetupData.GetTechUpgradeCost(techUpgrade);
        if (SpendMoney(cost))
        {
            techUpgrades.Add(techUpgrade);

            if (techUpgrade.obtainUpgradePart && !HasObtainedUpgradePart(techUpgrade.upgradePart))
            {
                var newObtainedUpgradePart = new ObtainedUpgradePart(techUpgrade.upgradePart);
                obtainedUpgradeParts.Add(newObtainedUpgradePart);
            }

            if (currentTimePeriod != null)
            {
                if(!currentTimePeriod.obtainedTechUpgrades.Contains(techUpgrade))
                    currentTimePeriod.obtainedTechUpgrades.Add(techUpgrade);
            }

            NewTechUpgradeResearchedEvent?.Invoke(techUpgrade);
            return true;
        }
        
        return false;
    }

    public bool HasObtainedUpgradePart(UpgradePart upgradePart)
    {
        return upgradePart != null && obtainedUpgradeParts.Count(oup => oup?.originalUpgradePart == upgradePart) > 0;
    }

    public bool SpendMoney(int amount)
    {
        if (HasSufficientFunds(amount))
        {
            AvailableBudget -= amount;
            if(currentTimePeriod != null)
                currentTimePeriod.spentMoney += amount;
            MoneySpentEvent?.Invoke();
            return true;
        }

        return false;
    }

    public bool HasSufficientFunds(int amount)
    {
        return AvailableBudget >= amount;
    }

    [Button]
    public void UpdateTranshumanParts()
    {
        if (transhuman != null && obtainedUpgradeParts != null)
        {
            foreach (var obtainedUpgradePart in obtainedUpgradeParts)
            {
                // TODO What if there are two upgradeParts of the same category? How can I determine which is the latest?
                transhuman.SwapActiveUpgradePart(obtainedUpgradePart.originalUpgradePart);
            }
        }
        //transhuman?.UpdateActiveUpgradeParts();
    }

    public int GetCurrentTechBranchLevel(TechBranch techBranch)
    {
        if (techBranchLevels == null || !techBranchLevels.ContainsKey(techBranch)) return 0;
        return techBranchLevels[techBranch];
    }

    public string GetCurrencyString(int amount)
    {
        return amount+"$";
    }
}