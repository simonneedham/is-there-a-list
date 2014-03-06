namespace IsThereAList.Migrations
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<IsThereAList.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(IsThereAList.Models.ApplicationDbContext context)
        {
            context.Roles.AddOrUpdate(
                r => r.Name,
                new IdentityRole { Name = "Admin"},
                new IdentityRole { Name = "User" }
            );

            context.ListTypes.AddOrUpdate(
                new IsThereAList.Models.ListType { ListTypeId = 1, Code = "BDY", Name = "Birthday" },
                new IsThereAList.Models.ListType { ListTypeId = 2, Code = "XMS", Name = "Christmas" }
            );
        }
    }
}
