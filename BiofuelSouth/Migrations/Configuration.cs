
using System;
using System.Linq;
using BiofuelSouth.Models;
using BiofuelSouth.Models.Entity;

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

            var x = new LookUpEntity();
            x.Name = @"Tarp Cost $/sq ft";
            x.Label = @"$ 0.15/sq ft";
            x.Value = Convert.ToString(0.15);
            x.LookUpGroup = LookupGroup.StorageCostParameter;
            x.SortOrder = 1;

            context.LookUps.AddOrUpdate(

                new LookUpEntity
                {
                    Id = Guid.NewGuid(),
                    Name = @"Tarp Cost $/sq ft",
                    Label = @"$ 0.15/sq ft",
                    Value = Convert.ToString(0.15),
                    LookUpGroup = LookupGroup.StorageCostParameter,
                    Description = @"Storing switchgrass",
                    SortOrder = 1
                },
                 new LookUpEntity
                {
                    Id = Guid.NewGuid(),
                    Name = @"Pallet Cost $/sq ft",
                    Label = @"$ 0.25/sq ft",
                    Value = Convert.ToString(0.15),
                    LookUpGroup = LookupGroup.StorageCostParameter,
                    Description = @"Storing switchgrass",
                    SortOrder = 1
                },

                      new LookUpEntity
                {
                    Id = Guid.NewGuid(),
                    Name = @"Gravel Cost $/sq ft",
                    Label = @"$ 0.75/sq ft",
                    Value = Convert.ToString(0.15),
                    LookUpGroup = LookupGroup.StorageCostParameter,
                    Description = @"Storing switchgrass",
                    SortOrder = 1
                },

                      new LookUpEntity
                      {
                          Id = Guid.NewGuid(),
                          Name = @"Labor Cost $/sq ft",
                          Label = @"$ 10/sq ft",
                          Value = Convert.ToString(0.15),
                          LookUpGroup = LookupGroup.StorageCostParameter,
                          Description = @"Storing switchgrass",
                          SortOrder = 1
                      },
                           new LookUpEntity
                           {
                               Id = Guid.NewGuid(),
                               Name = @"Land Cost $/sq ft",
                               Label = @"$ 10/sq ft",
                               Value = Convert.ToString(0.15),
                               LookUpGroup = LookupGroup.StorageCostParameter,
                               Description = @"Storing switchgrass",
                               SortOrder = 1
                           }


                );
            context.LookUps.AddOrUpdate(x);
            context.SaveChanges();


        }
    }
}
