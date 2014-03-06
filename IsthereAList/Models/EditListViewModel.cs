using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IsThereAList.Models
{
    public class EditListViewModel
    {
        public List<ListType> ListTypes { get; set; }

        public ApplicationUser Owner { get; set; }

        public int NewListTypeId { get; set; }

        public List EditableList { get; set; }
    }
}