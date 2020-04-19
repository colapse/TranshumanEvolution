using UnityEngine;

namespace Reporting
{
    public abstract class ReportText
    {
        public Report.ReportType reportTextCategory;
        
        public abstract string GetReportText(Report report);

        public abstract int
            GetPositivityIndex(Report report); // Value supposed to be between -2 and 2 (-2 Very Bad, 2 Very Good)
    }
}
