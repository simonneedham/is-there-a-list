using IsThereAList.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace IsThereAList.Models
{
    public class CreateListViewModel
    {
        public List<ListType> ListTypes { get; set; }

        public ApplicationUser Owner { get; set; }

        [Display(Name = "List Type")]
        public int NewListTypeId { get; set; }
    }
}
