using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

namespace Reporting
{
    public class ReportTextTechBranchResearch : ReportText
    {
        public override string GetReportText(Report report)
        {
            string text = "";
            var totalNewResearchPoints = report?._timePeriod?.obtainedTechBranchLevels?.Values.Sum();
            var totalResearchedBranches =
                report?._timePeriod?.obtainedTechBranchLevels?.Count(entry => entry.Value > 0);

            if (totalResearchedBranches == 0)
            {
                text += " The last "+report._player.gameSetupData.timePhaseDurationName+" saw a total halt in research" +
                        " for all major technology branches.";
                if (report._timePeriod.obtainedTechUpgrades.Count > 0)
                    text += " Although there were some advancements and improvements achieved for existing technology," +
                            " rumors say the lack of investments into the research of new technologies may indicate" +
                            " underlying issues in the transhuman industry.";
                else
                    text += " In fact, the transhuman industry hasn't seen any progress or innovation going on." +
                            " Is there a crisis coming heading towards humanity?";
            }else if (totalResearchedBranches == 1)
            {
                var advancedTechBranch =
                    report._timePeriod?.obtainedTechBranchLevels?.First(kvp => kvp.Value > 0).Key;
                text += " The last "+report._player.gameSetupData.timePhaseDurationName+" saw some progress in the research" +
                        " in "+advancedTechBranch.branchName+" technology. The question arises, are "+advancedTechBranch.branchName +
                        " parts the future of transhumanism?";
            }else if (totalResearchedBranches > 1)
            {
                var maxBranchLevel = report._player.techBranchLevels.Values.Max();
                var minBranchLevel = report._player.techBranchLevels.Values.Min();
                bool bigBranchLevelDif = maxBranchLevel-minBranchLevel > 2;
                TechBranch mostAdvancedTechBranch = null;
                report._player?.techBranchLevels?.ForEach(kvp =>
                {
                    if(mostAdvancedTechBranch == null || kvp.Value == maxBranchLevel) mostAdvancedTechBranch = kvp.Key;
                });
                
                text += " The last " + report._player.gameSetupData.timePhaseDurationName +
                        " saw progress in the research of multiple technology branches.";
                if (bigBranchLevelDif)
                {
                    if (maxBranchLevel < 5)
                    {
                        text += " The large differences in advancements of the various technology branches in research may suggest" +
                                " that society has decided on a path to go down for transhumanism. However, experts" +
                                " say that we still have to learn a lot about all of the technologies currently being researched," +
                                " and that it's still possible that we will see a change in preference of technology" +
                                " in the future.";
                    }
                    else
                    {
                        text += " However, some get much more attention than others due to their much more dedicated" +
                                " and advanced level of progress. Due to the superior progression of the " +
                                mostAdvancedTechBranch.branchName+" technology branch, experts suggest that our path" +
                                " to the future of transhumanism has been set - at least for now.";
                    }
                }
                else
                {
                    if (maxBranchLevel < 5)
                    {
                        text += " Experts say, our society hasn't decided yet what road to go down. Will we end up as robots?" +
                                " Or are we going to end up as an advanced biological organism? Maybe even a hybrid between them?";
                    }
                    else
                    {
                        text += " Experts predict that due to the continuous research in multiple technology branches" +
                                " transhumans will evolve into a hybrid organism consisting of various types of technology.";
                    }
                }
            }
            
            return text;
        }

        public override int GetPositivityIndex(Report report)
        {
            if (report == null) return 0;
            
            var totalNewResearchPoints = report?._timePeriod?.obtainedTechBranchLevels?.Values.Sum();
            var totalResearchedBranches =
                report?._timePeriod?.obtainedTechBranchLevels?.Count(entry => entry.Value > 0);

            int index = 0;
            if (totalResearchedBranches > 1) index++;
            if (totalNewResearchPoints > report._player.gameSetupData.techUpgradeCategories.Count) index++;

            return index;
        }
    }
}