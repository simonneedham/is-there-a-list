using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IsThereAList.Models
{
    [Table("List")]
    public class List
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ListId { get; set; }

        public int ListTypeId { get; set; }

        [Required]
        public string Name { get; set; }

        [MaxLength(128)]
        public string OwnerId { get; set; }

        public DateTime EffectiveDate { get; set; }
        public DateTime Updated { get; set; }

        [ForeignKey("ListTypeId")]
        public virtual ListType ListType { get; set; }

        [ForeignKey("OwnerId")]
        public virtual ApplicationUser Owner { get; set; }

        [InverseProperty("List")]
        public virtual ICollection<ListItem> ListItems { get; set; }

        public void SetEffectiveDateAndName(string listTypeCode, ApplicationUser owner)
        {
            DateTime effectiveDate;
            string listName;

            switch (listTypeCode)
            {
                case "BDY":
                    effectiveDate = new DateTime(DateTime.UtcNow.Year, owner.DobMonth, owner.DobDay);
                    listName = String.Format("{0}'s Birthday List {1}", owner.FirstName, effectiveDate.Year);
                    break;
                case "XMS":
                    effectiveDate = new DateTime(DateTime.UtcNow.Year, 12, 25);
                    listName = String.Format("{0}'s Xmas List {1}", owner.FirstName, effectiveDate.Year);
                    break;
                default:
                    throw new AppException("Unrecognised list type code");
            }

            this.EffectiveDate = effectiveDate;
            this.Name = listName;
        }
    }
}