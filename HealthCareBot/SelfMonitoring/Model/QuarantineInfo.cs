using System;
using System.Collections.Generic;
using System.Text;

namespace SelfMonitoring.Model
{
    public class QuarantineInfo
    {
        public string UserId { get; set; }
        public int Cycle { get; set; }
        public string QuarStartDate { get; set; }
        public string QuarMidpointDate { get; set; }
        public string QuarEndDate { get; set; }
        public string DateOfEntry { get; set; }
        public bool SymptomFever { get; set; }
        public bool SymptomShortnessOfBreath { get; set; }
        public bool SymptomCough { get; set; }
        public bool SymptomRunningNose { get; set; }
        public bool SymptomSoreThroat { get; set; }
        public bool SymptomChills { get; set; }
        public bool SymptomDizziness { get; set; }
        public bool SymptomAbdomenPain { get; set; }
        public bool SymptomDiarrhea { get; set; }
        public bool SymptomFatigue { get; set; }
        public string SymptomOther { get; set; }
        public decimal Temperature { get; set; }
        public decimal O2Saturation { get; set; }
        public string AntibodyTestDate { get; set; }
        public bool AntibodyTestResult { get; set; }
        public string RequestRTW { get; set; }
        public bool ApprovalRTW { get; set; }
        public bool TeamsCallInitiated { get; set; }
        public bool TeamsCallCompleted { get; set; }
    }
}
