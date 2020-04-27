using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName="GameSetupData",menuName="TransHuman/New Game Setup Data",order=4)]
public class GameSetupData : SerializedScriptableObject
{
    // Order is important {Poorest, ... , Richest}
    public enum WealthLevels:int{LowerClass = 0, MiddleClass = 1, UpperClass = 2}

    public static Vector2Int GetWealthLevelsMinMax() // x=min, y=max
    {
        var enumMinValue = 0;
        var enumMaxValue = 0;
        foreach (int enumVal in Enum.GetValues(typeof(GameSetupData.WealthLevels)))
        {
            if (enumVal < enumMinValue) enumMinValue = enumVal;
            if (enumVal > enumMaxValue) enumMaxValue = enumVal;
        }
        
        return new Vector2Int(enumMinValue, enumMaxValue);
    }

    public static int GetWealthLevelsCount() // Returns amount of wealth levels
    {
        return Enum.GetNames(typeof(WealthLevels)).Length;
    }

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
    
    [FoldoutGroup("Population Setup"), Tooltip("Population per year in millions. X0=Start Population.")]
    public AnimationCurve populationGrowthCurve;
    [FoldoutGroup("Population Setup"), Tooltip("0=Lower Class, 1=Middle Class, 2=Upper Class; Percentage value should sum up to 1.")]
    public Dictionary<WealthLevels,float> populationWealthLevelPercentages; // Percentages 0-1

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
    [FoldoutGroup("Time Phase Settings"), Tooltip("decade, century, ...")]
    public string timePhaseDurationName = "decade";


    [FoldoutGroup("Cost Settings")] public Dictionary<TechBranch, AnimationCurve> techBranchCost;
    [FoldoutGroup("Cost Settings")] public Dictionary<TechUpgrade, int> techUpgradesCost;

    [Button, PropertyOrder(2), FoldoutGroup("Cost Settings")]
    public void LoadAllTechUpgradesCost(bool keepExistingSettings = true)
    {
        if(!keepExistingSettings)
            techUpgradesCost.Clear();
        techUpgrades.ForEach(upg =>
        {
            if(!keepExistingSettings || !techUpgradesCost.ContainsKey(upg))
                techUpgradesCost.Add(upg,0);
        });
    }
    
    
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
    [FoldoutGroup("Technology Settings"), PropertyOrder(9)] public List<UpgradePart> startHumanParts;

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
