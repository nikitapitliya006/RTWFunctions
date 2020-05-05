using System;
using System.Collections.Generic;
using System.Text;

namespace SelfMonitoring.Model
{
    public class ScreeningInfo
    {
        public string UserId { get; set; }
        public string DateOfEntry { get; set; }
        public bool UserExposed { get; set; }
        public bool ExposureDirect { get; set; }
        public bool ExposureIndirect { get; set; }
        public bool ExposureMultiple { get; set; }
        public bool ExposureNotsure { get; set; }
        public string ExposureDate { get; set; }
        public bool SymptomsYesNo { get; set; }
        public bool SymptomFever { get; set; }
        public bool SymptomShortnessOfBreath { get; set; }
        public bool SymptomCough { get; set; }
        public bool SymotomRunningNose { get; set; }
        public bool SymptomSoreThroat { get; set; }
        public bool SymptomChills { get; set; }
        public bool SymptomDizziness { get; set; }
        public bool SymptomAbdomenPain { get; set; }
        public string SymptomOther { get; set; }
        public string GUID { get; set; }
        public bool QuarantineRequired { get; set; }
    }
}
































