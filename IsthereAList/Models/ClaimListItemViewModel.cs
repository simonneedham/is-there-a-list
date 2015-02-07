using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IsThereAList.Models
{
    public class ClaimListItemViewModel
    {
        public ClaimListItemViewModel()
        {
            this.ErrorMessage = String.Empty;
        }

        public int ListId { get; set; }
        public int ListItemId { get; set; }
        public string ListName { get; set; }
        public string ListOwnerFirstName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string PictureUrl { get; set; }
        public bool HasBeenPurchased { get; set; }

        [Display(Name="Purchased by")]
        public string UserPurchasedFullName { get; set; }
        public bool Deleted { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsPurchasee { get; set; }
    }
}
