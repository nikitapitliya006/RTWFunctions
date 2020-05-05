using System;
using System.Collections.Generic;
using System.Text;

namespace SelfMonitoring.Model
{
    public class UserUnderlyingInfo
    {
        public string UserId { get; set; }
        public bool HeartDisease { get; set; }
        public bool Asthma { get; set; }
        public bool LungProblems { get; set; }
        public bool Cancer { get; set; }
        public bool Diabetes { get; set; }
        public bool Chemotherapy { get; set; }
        public bool Arthritis { get; set; }
        public bool isThermometerHandy { get; set; }
        public bool isO2SatMonitorHandy { get; set; }
    }
}