using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismBase.Modules.Details.Models
{
    public class Task
    {
        public int TaskID { get; set; }
        public List<int> WorkerIDs { get; set; }
        public string TaskTitle { get; set; }
        public string Description { get; set; }
    }
}
