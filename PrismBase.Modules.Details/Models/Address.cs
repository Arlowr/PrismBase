using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismBase.Modules.Details.Models
{
    public class Address
    {
        public string FullAddressString() 
        {
            string fullAddress = "";
            bool first = true;

            for (int i = 1; i <= 6; i++)
            {
                if (!first)
                {
                    fullAddress += ", ";
                }
                if (i == 1 && !String.IsNullOrEmpty(Line1))
                {
                    fullAddress += Line1;
                    first = false;
                }
                if (i == 2 && !String.IsNullOrEmpty(Line2))
                {
                    fullAddress += Line2;
                }
                if (i == 3 && !String.IsNullOrEmpty(Line3))
                {
                    fullAddress += Line3;
                }
                if (i == 4 && !String.IsNullOrEmpty(City))
                {
                    fullAddress += City;
                }
                if (i == 5 && !String.IsNullOrEmpty(County))
                {
                    fullAddress += County;
                }
                if (i == 6 && !String.IsNullOrEmpty(PostCode))
                {
                    fullAddress += PostCode;
                }
            }

            return fullAddress;
        }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string PostCode { get; set; }
    }
}
