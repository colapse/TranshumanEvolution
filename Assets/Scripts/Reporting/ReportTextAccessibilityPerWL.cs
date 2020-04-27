using UnityEngine;

namespace Reporting
{
    public class ReportTextAccessibilityPerWL : ReportText
    {

        private static int noPartsPoor;
        private static int noPartsMiddle;
        private static int noPartsRich;
        
        private static int difPoorMiddle;
        private static int difPoorRich;
        private static int difMiddleRich;
        
        private static float poorAvailablePartsPercent; // 0-1
        private static float middleAvailablePartsPercent; // 0-1
        
        public ReportTextAccessibilityPerWL()
        {
            reportTextCategory = Report.ReportType.AccessiblityPerWL;
        }
        
        public override string GetReportText(Report report)
        {
            if (report.partsAccessibilityPerWL == null || report.partsAccessibilityPerWL.Count == 0) return "";
            string text = "";

            UpdateValues(report);

            // Poor
            if (Mathf.Approximately(poorAvailablePartsPercent,0))
            {
                text += "The poor of the world are left behind in the transhuman revolution as they cannot afford any upgrades." +
                        " Few to no efforts can be seen to lower cost of transhuman technology which would allow also the lower class to" +
                        " be part of the evolution.";
            }else if (poorAvailablePartsPercent < 0.25f)
            {
                text += "The lower class has access only to a small portion of available transhuman upgrades yet leaving them" +
                        " behind, deprived of being a substantial part of the transhuman evolution.";
            }else if (poorAvailablePartsPercent < 0.5f)
            {
                text += "The lower class can somewhat take part in the transhuman evolution yet they don't have access" +
                        " to many available upgrades.";
            }else if (poorAvailablePartsPercent < 0.75f)
            {
                text += "The world is in balance as most technological advancements of the transhuman revolution are" +
                        " available to all levels of wealth.";
            }/*else
            {
                text += "Equality and accessibility. The worlds' population lives in prosperity as the poor of the world" +
                        " have equal access to the entire transhuman progress, eliminating any differentiation between" +
                        " wealth classes.";
            }*/
            
            // Middle
            if (Mathf.Approximately(middleAvailablePartsPercent,0))
            {
                text += " However, the same applies to the middle class. As of now, the transhuman evolution is solely" +
                        " reserved for the rich and powerful of the world while the rest is left behind." +
                        " These advancements lead to a huge separation between the upper class and the rest of the population";
            }else if (middleAvailablePartsPercent < 0.25f)
            {
                if (poorAvailablePartsPercent > 0 && poorAvailablePartsPercent < 0.25f)
                    text += " The same goes for the middle class, leaving the upper class far more advanced.";
                else
                    text += " While the middle class has access to only a small minority of transhuman upgrades, the" +
                            " privileged are far more advanced in transhuman terms.";
            }else if (middleAvailablePartsPercent < 0.5f)
            {
                if (poorAvailablePartsPercent >= 2.5f && poorAvailablePartsPercent < 0.5f)
                    text +=
                        " While the middle class sits in the same boat with the lower class, the upper class is taking" +
                        " the lead in the journey of getting to be Human V2.0.";
                else
                    text += " The middle class can somewhat take part in the transhuman evolution, they're not yet part" +
                            " of the transhuman revolution.";
            }else if (middleAvailablePartsPercent < 0.75f)
            {
                if (poorAvailablePartsPercent < 0.5f)
                    text += " The middle class has adequate opportunities in upgrading themselves and being part of" +
                            " the transhuman evolution.";
            }else
            {
                text += " Equality and accessibility. The worlds' population lives in prosperity as all classes of the world can mostly or fully participate in the entire transhuman progress.";
            }
            
            return text;
        }

        public override int GetPositivityIndex(Report report)
        {
            if (report.partsAccessibilityPerWL == null || report.partsAccessibilityPerWL.Count == 0) return 0;
            float positivityIndex = 0;

            UpdateValues(report);

            if (poorAvailablePartsPercent < 0.25f) positivityIndex -= 2f;
            else if (poorAvailablePartsPercent < 0.5f) positivityIndex -= .5f;
            else if (poorAvailablePartsPercent < 0.75f) positivityIndex += 0.5f;
            else positivityIndex++;
            
            if (middleAvailablePartsPercent < 0.25f) positivityIndex -= 2f;
            else if (middleAvailablePartsPercent < 0.5f) positivityIndex -= 1f;
            else if (middleAvailablePartsPercent < 0.75f) positivityIndex += 0.5f;
            else positivityIndex++;
            
            return Mathf.Clamp(Mathf.RoundToInt(positivityIndex),-2,2);
        }
        
        private static void UpdateValues(Report report)
        {
            if (report.partsAccessibilityPerWL == null || report.partsAccessibilityPerWL.Count == 0) return;

            report.partsAccessibilityPerWL.TryGetValue(GameSetupData.WealthLevels.LowerClass, out noPartsPoor);
            report.partsAccessibilityPerWL.TryGetValue(GameSetupData.WealthLevels.MiddleClass, out noPartsMiddle);
            report.partsAccessibilityPerWL.TryGetValue(GameSetupData.WealthLevels.UpperClass, out noPartsRich);
            
            
            difPoorMiddle = noPartsMiddle-noPartsPoor;
            difPoorRich = noPartsRich-noPartsPoor;
            difMiddleRich = noPartsRich-noPartsMiddle;
            
            poorAvailablePartsPercent = noPartsRich>0?(float)noPartsPoor / noPartsRich:1;
            middleAvailablePartsPercent = noPartsRich>0?(float)noPartsMiddle / noPartsRich:1;
            
            
            /*
            Debug.Log("==== Accessibility Text DATA ====");
            Debug.Log("noPartsPoor "+noPartsPoor);
            Debug.Log("noPartsMiddle "+noPartsMiddle);
            Debug.Log("noPartsRich "+noPartsRich);
            Debug.Log("difPoorMiddle "+difPoorMiddle);
            Debug.Log("difPoorRich "+difPoorRich);
            Debug.Log("difMiddleRich "+difMiddleRich);
            Debug.Log("poorAvailablePartsPercent "+poorAvailablePartsPercent);
            Debug.Log("middleAvailablePartsPercent "+middleAvailablePartsPercent);
            Debug.Log("==== END ====");*/
        }
    }
}