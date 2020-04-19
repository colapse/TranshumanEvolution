using System;
using System.Collections.Generic;
using UnityEngine;

public class ObtainedUpgradePart
{
        public UpgradePart originalUpgradePart;
        public List<TechUpgrade> techUpgrades;

        public ObtainedUpgradePart(UpgradePart upgradePart)
        {
                originalUpgradePart = upgradePart;

        }

        public float GetWealthLevelCost()
        {
                var enumMinMax = GameSetupData.GetWealthLevelsMinMax();
                if (originalUpgradePart == null) return enumMinMax.x;
                
                float cost = (float)originalUpgradePart.minWealthLevel;

                if (techUpgrades != null && techUpgrades.Count > 0)
                {
                        foreach (var techUpgrade in techUpgrades)
                        {
                                if (techUpgrade.upgradePart == originalUpgradePart &&
                                    !Mathf.Approximately(techUpgrade.minWealthLevelChange,0))
                                        cost += techUpgrade.minWealthLevelChange;
                        }
                }


                cost = Mathf.Clamp(cost, enumMinMax.x, enumMinMax.y);

                return cost;
        }

        public GameSetupData.WealthLevels GetMinWealthLevelAccessibility()
        {
                return (GameSetupData.WealthLevels)Mathf.FloorToInt(GetWealthLevelCost());
        }

        public int GetTechLevel()
        {
                if (originalUpgradePart == null) return 0;
                
                int finalTechLevel = originalUpgradePart.techLevel;
                if (techUpgrades != null && techUpgrades.Count > 0)
                {
                        foreach (var techUpgrade in techUpgrades)
                        {
                                if (techUpgrade.upgradePart == originalUpgradePart)
                                        finalTechLevel += techUpgrade.partTechLevelChange;
                        }
                }

                return finalTechLevel;
        }

        public int GetTranshumanIndex()
        {
                if (originalUpgradePart == null) return 0;
                int index = originalUpgradePart.transhumanIndex;

                if (techUpgrades != null && techUpgrades.Count > 0)
                {
                        foreach (var techUpgrade in techUpgrades)
                        {
                                if (techUpgrade.upgradePart == originalUpgradePart)
                                        index += techUpgrade.transhumanIndexChange;
                        }
                }
                
                return Mathf.Clamp(index,0,10);
        }

        public int GetAestheticTranshumanIndex()
        {
                if (originalUpgradePart == null) return 0;
                int index = originalUpgradePart.aestheticTranshumanIndex;

                if (techUpgrades != null && techUpgrades.Count > 0)
                {
                        foreach (var techUpgrade in techUpgrades)
                        {
                                if (techUpgrade.upgradePart == originalUpgradePart)
                                        index += techUpgrade.aestheticTranshumanIndexChange;
                        }
                }
                
                return Mathf.Clamp(index,0,10);
        }
}