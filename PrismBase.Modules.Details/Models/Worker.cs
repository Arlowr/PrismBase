using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismBase.Modules.Details.Models
{
    public class Worker : Person
    {
        public int WorkerID { get; set; }
        public string Department { get; set; }
        public string JobTitle { get; set; }
    }
}
