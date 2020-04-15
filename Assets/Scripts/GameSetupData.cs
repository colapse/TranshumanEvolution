using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName="GameSetupData",menuName="TransHuman/New Game Setup Data",order=4)]
public class GameSetupData : SerializedScriptableObject
{
    // Order is important {Poorest, ... , Richest}
    public enum WealthLevels{Poor, Middle, Wealthy, Rich, Superrich}

    public string setupName;
    [TextArea]
    public string setupDescription;
    
    [FoldoutGroup("Path Setup")]
    public string resTechUpgradesPath = "ScriptableObjects/TechUpgrades";
    [FoldoutGroup("Path Setup")]
    public string resTechUpgradeCategoriesPath = "ScriptableObjects/TechUpgradeCategories";
    [FoldoutGroup("Path Setup")]
    public string resTechUpgradesTypesPath = "ScriptableObjects/TechUpgradeTypes";
    [FoldoutGroup("Path Setup")]
    public string resTechBranchesPath = "ScriptableObjects/TechBranches";
    [FoldoutGroup("Path Setup")]
    public string resUpgradePartsPath = "ScriptableObjects/UpgradeParts";
    
    [Tooltip("Population per year in millions. X0=Start Population.")]
    public AnimationCurve populationGrowthCurve;
    [Tooltip("0=Poor, 1=Middle, 2=Wealthy, 3=Rich, 4=Superrich")]
    public Dictionary<WealthLevels,float> populationWealthLevelPercentages; // Percentages

    [FoldoutGroup("Time Phase Settings")]
    public int maxTimePhases = 10;
    [FoldoutGroup("Time Phase Settings")]
    public int budgetPerPhase = 10000;
    [FoldoutGroup("Time Phase Settings")]
    public bool canTakeOverLeftBudget;
    [FoldoutGroup("Time Phase Settings")]
    public int timePhaseStartYear = 2020;
    [FoldoutGroup("Time Phase Settings"), Tooltip("Amount of years each time phase consists of.")]
    public int timePhaseDuration = 10;


    [FoldoutGroup("Cost Settings")] public Dictionary<TechBranch, AnimationCurve> techBranchCost;
    [FoldoutGroup("Cost Settings")] public Dictionary<TechUpgrade, int> techUpgradesCost;
    
    [Button, PropertyOrder(0),FoldoutGroup("Technology Settings")]
    public void LoadAllTechUpgrades() => techUpgrades = Resources.LoadAll<TechUpgrade>(resTechUpgradesPath).ToList();
    [FoldoutGroup("Technology Settings"), PropertyOrder(1)] public List<TechUpgrade> techUpgrades;
    [Button, PropertyOrder(2),FoldoutGroup("Technology Settings")]
    public void LoadAllTechUpgradeCategories() => techUpgradeCategories = Resources.LoadAll<TechUpgradeCategory>(resTechUpgradeCategoriesPath).ToList();
    [FoldoutGroup("Technology Settings"), PropertyOrder(3)] public List<TechUpgradeCategory> techUpgradeCategories;
    [Button, PropertyOrder(4),FoldoutGroup("Technology Settings")]
    public void LoadAllTechUpgradeTypes() => techUpgradeTypes = Resources.LoadAll<TechUpgradeType>(resTechUpgradesTypesPath).ToList();
    [FoldoutGroup("Technology Settings"), PropertyOrder(5)] public List<TechUpgradeType> techUpgradeTypes;
    [Button, PropertyOrder(6),FoldoutGroup("Technology Settings")]
    public void LoadAllTechBranches() => techBranches = Resources.LoadAll<TechBranch>(resTechBranchesPath).ToList();
    [FoldoutGroup("Technology Settings"), PropertyOrder(7)] public List<TechBranch> techBranches;
    [Button, PropertyOrder(8),FoldoutGroup("Technology Settings")]
    public void LoadAllUpgradeParts() => upgradeParts = Resources.LoadAll<UpgradePart>(resUpgradePartsPath).ToList();
    [FoldoutGroup("Technology Settings"), PropertyOrder(9)] public List<UpgradePart> upgradeParts;

    [FoldoutGroup("Technology Settings"), PropertyOrder(10)]
    public int maxTechBranchLevel = 10;

    public int GetTechBranchCost(TechBranch techBranch, int level)
    {
        if (techBranchCost != null && techBranchCost.ContainsKey(techBranch))
        {
            return (int)(techBranchCost[techBranch]?.Evaluate(level) ?? 0);
        }

        return 4;
    }

    public int GetTechUpgradeCost(TechUpgrade techUpgrade)
    {
        if (techUpgradesCost != null && techUpgradesCost.ContainsKey(techUpgrade))
            return techUpgradesCost[techUpgrade];

        return 0;
    }
}
