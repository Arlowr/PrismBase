using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismBase.Modules.Details.Models
{
    public class Note
    {
        public int ClientId { get; set; }
        public int NoteID { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
    }
}
