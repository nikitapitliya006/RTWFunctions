using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HealthBotV4DataService.Data
{
    public partial class PatientData
    {
        [Key]
        public int PatientId { get; set; }
        public string Name { get; set; }
        public string Result { get; set; }
        public string State { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
