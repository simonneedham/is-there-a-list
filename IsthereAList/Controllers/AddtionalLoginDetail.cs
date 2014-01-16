using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IsThereAList.Controllers
{
    public class AddtionalLoginDetail
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public int DobDay { get; set; }
        public int DobMonth { get; set; }

        public AddtionalLoginDetail()
        {
            this.DobDay = 0;
            this.DobMonth = 0;
        }
    }
}