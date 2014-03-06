using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;

namespace IsThereAList.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("IsThereAListContext")
        {
        }

        public DbSet<List> Lists { get; set; }
        public DbSet<ListItem> ListItems { get; set; }
        public DbSet<ListType> ListTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());

            //Schema config that I don't want to be used by MVC validation
            modelBuilder.Entity<ListItem>().Property(p => p.ApplicationUserIdUpdated).IsRequired();
            modelBuilder.Entity<List>().Property(p => p.OwnerId).IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }

}