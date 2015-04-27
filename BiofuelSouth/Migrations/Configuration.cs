
using System;
using System.Linq;
using BiofuelSouth.Models;
using Microsoft.Ajax.Utilities;

namespace BiofuelSouth.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "BiofuelSouth.Models.DatabaseContext";
        }

        protected override void Seed(DatabaseContext context)
        {
            //Clean up all empty glossaries
            var allEmpty = context.Glossaries.Where(m => m.Term.Length == 0);
            context.Glossaries.RemoveRange(allEmpty);
            context.SaveChanges();

            var glossaries = context.Glossaries.Where(m => m.Id == null);
            foreach (var g in glossaries)
            {
                g.Id = Guid.NewGuid();
                
            }


           
            context.SaveChanges();

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
