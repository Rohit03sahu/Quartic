using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssignmentQuartic.AI.Models
{
    public class Rules
    {
        public int Rule_ID { get; set; }
        public string rule_signal { get; set; }
        public string rule_minvalues { get; set; }
        public string rule_maxvalues { get; set; }
        public string rule_valuetype { get; set; }
        public string rule_desc { get; set; }

    }
}