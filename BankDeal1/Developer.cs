using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankDeal1
{
    class Developer
    {
        
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Struct, AllowMultiple = true)]
        public class DeveloperInfo : Attribute
        {
            private string developerName;
            private string dateCreated;
            public string Developer
            {
                get { return developerName; }
            }
            public string Date
            {
                get { return dateCreated; }
                set { dateCreated = value; }
            }

            public DeveloperInfo(string developerName)
            {
                this.developerName = developerName;
            }
        }
    }
}
    
