using System;
using System.Collections.Generic;
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
            TranshumanIndexAestheticPerWL
        }

        private Dictionary<ReportType,ReportText> _reportTexts = new Dictionary<ReportType,ReportText>();
        
        private Player _player;
        
        // Data (WL = Wealth Level, TH = TransHuman)
        public List<ObtainedUpgradePart> availableParts;
        
        public Dictionary<GameSetupData.WealthLevels, int> thIndexPerWL; // Averages of all parts
        public Dictionary<GameSetupData.WealthLevels, int> aestheticThIndexPerWL; // Averages of all parts
        public int thIndexTotalPopulation;
        public Dictionary<GameSetupData.WealthLevels, int> partsAccessibilityPerWL; // # parts available per WL
        public Dictionary<GameSetupData.WealthLevels, int> partsTechLevelSumPerWL; // Highest TechLevel of available parts per WL

        public Report(Player player)
        {
            _player = player;
            
            InitReportData();
            
            _reportTexts.Add(ReportType.AccessiblityPerWL, new ReportTextAccessibilityPerWL());
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
            foreach (var enumEntry in Enum.GetValues(typeof(GameSetupData.WealthLevels)))
            {
                thIndexPerWL.Add((GameSetupData.WealthLevels)enumEntry,0);
                aestheticThIndexPerWL.Add((GameSetupData.WealthLevels)enumEntry,0);
                partsAccessibilityPerWL.Add((GameSetupData.WealthLevels)enumEntry,0);
                partsTechLevelSumPerWL.Add((GameSetupData.WealthLevels)enumEntry,0);
            }
            
            
            availableParts = _player.obtainedUpgradeParts;

            if (availableParts != null && availableParts.Count > 0)
            {
                int[] tmpThIndex = new int[wealthLevelsMinMax.y];
                int[] countThIndexes = new int[wealthLevelsMinMax.y];
                int[] tmpAestheticThIndex = new int[wealthLevelsMinMax.y];
                int[] countAestheticThIndexes = new int[wealthLevelsMinMax.y];
                foreach (var upgradePart in availableParts)
                {
                    var minWealthLevel = upgradePart.GetMinWealthLevelAccessibility();
                    for (int i = (int)minWealthLevel; i < wealthLevelsMinMax.y; i++)
                    {
                        partsAccessibilityPerWL[(GameSetupData.WealthLevels) i]++;
                        partsTechLevelSumPerWL[(GameSetupData.WealthLevels) i] += upgradePart.GetTechLevel();
                        tmpThIndex[i] += upgradePart.GetTranshumanIndex();
                        countThIndexes[i]++;
                        tmpAestheticThIndex[i] += upgradePart.GetAestheticTranshumanIndex();
                        countAestheticThIndexes[i]++;
                    }
                }

                for (int i = 0; i < wealthLevelsMinMax.y; i++)
                {
                    thIndexPerWL[(GameSetupData.WealthLevels) i] = tmpThIndex[i] / countThIndexes[i];
                    aestheticThIndexPerWL[(GameSetupData.WealthLevels) i] = tmpAestheticThIndex[i] / countAestheticThIndexes[i];
                }
            }
        }
    }
}
