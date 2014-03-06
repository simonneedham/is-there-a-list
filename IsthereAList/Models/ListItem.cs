using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IsThereAList.Models
{
    [Table("ListItem")]
    public class ListItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ListItemId { get; set; }

        public int ListId { get; set; }

        [Required, MaxLength(255)]
        [Display(Name="Suggestion")]
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        [Display(Name = "Web Link")]
        public string Url { get; set; }
        
        [Display(Name="Picture")]
        public string PictureUrl { get; set; }

        [MaxLength(128)]
        public string ApplicationUserIdPurchased { get; set; }

        public bool Deleted { get; set; }

        [MaxLength(128)]
        public string ApplicationUserIdUpdated { get; set; }

        public DateTime Updated { get; set; }

        [NotMapped]
        public bool HasBeenPurchased
        {
            get { return !String.IsNullOrEmpty(this.ApplicationUserIdPurchased); }
        }

        [ForeignKey("ListId")]
        public virtual List List { get; set; }

        [Display(Name = "Purchased by")]
        [ForeignKey("ApplicationUserIdPurchased")]
        public virtual ApplicationUser UserPurchased { get; set; }

        [ForeignKey("ApplicationUserIdUpdated")]
        public virtual ApplicationUser UserUpdated { get; set; }
    }
}