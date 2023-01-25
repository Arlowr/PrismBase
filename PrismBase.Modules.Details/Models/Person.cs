using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismBase.Modules.Details.Models
{
    public class Person
    {
        public string Description { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return FirstName + " " + LastName;}}
        public string Title { get; set; }
        public string Address { get; set; }
    }
}
