using System;
using System.Collections.Generic;
using System.Text;

namespace SelfMonitoring.Model
{
    public class TeamsAddressQuarantineInfo
    {
        public string UserId { get; set; }
        public string TeamsAddress { get; set; }
        public bool QuarantineRequired { get; set; }
    }
}
