using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Reporting
{
    public class Report
    {
        public enum ReportType
        {
            AccessiblityPerWL,
            AvailablePartsTechLevelByWL,
            TranshumanIndexOfPopulation,
            TranshumanIndexPerWL,
            TranshumanIndexAestheticPerWL,
            TechBranchResearch
        }

        private Dictionary<ReportType,ReportText> _reportTexts = new Dictionary<ReportType,ReportText>();
        
        public Player _player;
        public TimePeriod _timePeriod;
        
        // Data (WL = Wealth Level, TH = TransHuman)
        public List<ObtainedUpgradePart> availableParts;
        
        public Dictionary<GameSetupData.WealthLevels, int> thIndexPerWL; // Averages of all parts
        public Dictionary<GameSetupData.WealthLevels, int> aestheticThIndexPerWL; // Averages of all parts
        public int thIndexTotalPopulation;
        public int aesThIndexTotalPopulation;
        public Dictionary<GameSetupData.WealthLevels, int> partsAccessibilityPerWL; // # parts available per WL
        public Dictionary<GameSetupData.WealthLevels, int> partsTechLevelSumPerWL; // Highest TechLevel of available parts per WL
        

        public Report(Player player, TimePeriod timePeriod)
        {
            _player = player;
            _timePeriod = timePeriod;
            
            InitReportData();
            
            _reportTexts.Add(ReportType.AccessiblityPerWL, new ReportTextAccessibilityPerWL());
            _reportTexts.Add(ReportType.TechBranchResearch, new ReportTextTechBranchResearch());
        }

        public ReportText GetReportText(ReportType reportType)
        {
            return _reportTexts.ContainsKey(reportType) ? _reportTexts[reportType] : null;
        }

        public void InitReportData()
        {
            if (_player == null) return;
            
            thIndexPerWL = new Dictionary<GameSetupData.WealthLevels, int>();
            aestheticThIndexPerWL = new Dictionary<GameSetupData.WealthLevels, int>();
            partsAccessibilityPerWL = new Dictionary<GameSetupData.WealthLevels, int>();
            partsTechLevelSumPerWL = new Dictionary<GameSetupData.WealthLevels, int>();
            
            var wealthLevelsMinMax = GameSetupData.GetWealthLevelsMinMax();
            var wealthLevelsCount = GameSetupData.GetWealthLevelsCount();
            foreach (var enumEntry in Enum.GetValues(typeof(GameSetupData.WealthLevels)))
            {
                thIndexPerWL.Add((GameSetupData.WealthLevels)enumEntry,0);
                aestheticThIndexPerWL.Add((GameSetupData.WealthLevels)enumEntry,0);
                partsAccessibilityPerWL.Add((GameSetupData.WealthLevels)enumEntry,0);
                partsTechLevelSumPerWL.Add((GameSetupData.WealthLevels)enumEntry,0);
            }
            
            
            availableParts = _player.obtainedUpgradeParts;//.Where(oup => oup.originalUpgradePart.transhumanIndex > 0).ToList();
            
            if (availableParts != null && availableParts.Count > 0)
            {
                int[] tmpThIndex = new int[wealthLevelsCount];
                int[] countThIndexes = new int[wealthLevelsCount];
                int[] tmpAestheticThIndex = new int[wealthLevelsCount];
                int[] countAestheticThIndexes = new int[wealthLevelsCount];
                foreach (var upgradePart in availableParts)
                {
                    var minWealthLevel = upgradePart.GetMinWealthLevelAccessibility();
                    for (int i = (int)minWealthLevel; i <= wealthLevelsMinMax.y; i++)
                    {
                        partsAccessibilityPerWL[(GameSetupData.WealthLevels) i]++;
                        partsTechLevelSumPerWL[(GameSetupData.WealthLevels) i] += upgradePart.GetTechLevel();
                        tmpThIndex[i] += upgradePart.GetTranshumanIndex();
                        countThIndexes[i]++;
                        tmpAestheticThIndex[i] += upgradePart.GetAestheticTranshumanIndex();
                        countAestheticThIndexes[i]++;
                    }
                }

                // Calc TH Index per WL
                for (int i = 0; i <= wealthLevelsMinMax.y; i++)
                {
                    thIndexPerWL[(GameSetupData.WealthLevels) i] = countThIndexes[i] == 0 ? 0 : tmpThIndex[i] / countThIndexes[i];
                    aestheticThIndexPerWL[(GameSetupData.WealthLevels) i] = countAestheticThIndexes[i]==0?0:tmpAestheticThIndex[i] / countAestheticThIndexes[i];
                    Debug.Log("TH Index of "+((GameSetupData.WealthLevels) i)+" "+thIndexPerWL[(GameSetupData.WealthLevels) i]);
                    Debug.Log("aesTH Index of "+((GameSetupData.WealthLevels) i)+" "+aestheticThIndexPerWL[(GameSetupData.WealthLevels) i]);
                }
                
                // Calc TH Index
                var currentPopulation = _player.gameSetupData.populationGrowthCurve.Evaluate(_timePeriod.GetYearsSinceStart(_player.gameSetupData));
                //var populationPerWL = new Dictionary<GameSetupData.WealthLevels,int>();
                Debug.Log("CurPop: "+currentPopulation);
            
                int thIndexAllSum = 0;
                int aesThIndexAllSum = 0;
                foreach (var kvp in _player.gameSetupData.populationWealthLevelPercentages)
                {
                    var wLPopulation = Mathf.FloorToInt(kvp.Value * currentPopulation);
                    thIndexPerWL.TryGetValue(kvp.Key, out var wlThIndex);
                    aestheticThIndexPerWL.TryGetValue(kvp.Key, out var wlAesThIndex);
                    //populationPerWL.Add(kvp.Key, wLPopulation);
                    thIndexAllSum += wLPopulation * wlThIndex;
                    aesThIndexAllSum += wLPopulation * wlAesThIndex;
                    Debug.Log("Pop of "+kvp.Key+" "+wLPopulation+"; ThIndex: "+wlThIndex+"; aesThIndex: "+wlAesThIndex);
                }
            
                thIndexTotalPopulation = Mathf.CeilToInt(thIndexAllSum / currentPopulation); // RoundToInt
                aesThIndexTotalPopulation = Mathf.CeilToInt(aesThIndexAllSum / currentPopulation);
                Debug.Log("thIndexTotalPopulation "+thIndexTotalPopulation);
            }
        }

        /*private int CalculateTHIndexTotalPopulation()
        {
            var index = 0;

            var currentPopulation = _player.gameSetupData.populationGrowthCurve.Evaluate(_timePeriod.GetYearsSinceStart(_player.gameSetupData));
            var populationPerWL = new Dictionary<GameSetupData.WealthLevels,int>();
            
            int thIndexAllSum = 0;
            foreach (var kvp in _player.gameSetupData.populationWealthLevelPercentages)
            {
                populationPerWL.Add(kvp.Key, Mathf.FloorToInt(kvp.Value * currentPopulation));
            }
            
            index = thIndexAllSum / wealthLevelsMinMax.y;
            
            
            return index;
        }*/
    }
}
