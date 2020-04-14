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
    
    [Tooltip("Population per year in millions. X0=Start Popuation.")]
    public AnimationCurve populationGrowthCurve;
    [Tooltip("0=Poor, 1=Middle, 2=Wealthy, 3=Rich, 4=Superrich")]
    public Dictionary<WealthLevels,float> populationWealthLevelPercentages; // Percentages

    [Button]
    public void LoadAllTechUpgrades() => techUpgrades = Resources.LoadAll<TechUpgrade>(resTechUpgradesPath).ToList();
    public List<TechUpgrade> techUpgrades;
    [Button]
    public void LoadAllTechUpgradeCategories() => techUpgradeCategories = Resources.LoadAll<TechUpgradeCategory>(resTechUpgradeCategoriesPath).ToList();
    public List<TechUpgradeCategory> techUpgradeCategories;
    [Button]
    public void LoadAllTechUpgradeTypes() => techUpgradeTypes = Resources.LoadAll<TechUpgradeType>(resTechUpgradesTypesPath).ToList();
    public List<TechUpgradeType> techUpgradeTypes;
    [Button]
    public void LoadAllTechBranches() => techBranches = Resources.LoadAll<TechBranch>(resTechBranchesPath).ToList();
    public List<TechBranch> techBranches;
    [Button]
    public void LoadAllUpgradeParts() => upgradeParts = Resources.LoadAll<UpgradePart>(resUpgradePartsPath).ToList();
    public List<UpgradePart> upgradeParts;

   
}
