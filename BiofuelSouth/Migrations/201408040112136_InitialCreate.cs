using System.Data.Entity.Migrations;

namespace BiofuelSouth.Migrations
{
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {

            CreateTable(
                "dbo.CountyEntities",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                    CountyCode = c.String(nullable: false),
                    State = c.String(),
                    Lat = c.Double(nullable: true),
                    Lon = c.Double(nullable: true),
                    GeoID = c.String()

                })
                .PrimaryKey(t => t.Id);

            CreateTable(
               "dbo.GlossaryEntities",
               c => new
               {
                   ID = c.Guid(nullable: false),
                   term = c.String(false, 128),
                   keywords = c.String(),
                   description = c.String(),
                   counter = c.Int(),
                   source = c.String(),
                   ModifiedBy = c.String(),
                   CreatedBy = c.String(),
                   Created = c.DateTime(),
                   Modified = c.DateTime(),
                   IsDirty = c.Int()

               })
               .PrimaryKey(t => t.term);

            CreateTable(
                "dbo.ProductivityEntities",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    GeoId = c.String(false, 5),
                    CropType = c.Int(false),
                    Yield = c.Double(false),
                    Cost = c.Double(false)
                })
                .PrimaryKey(t => t.Id);

            //CreateTable(
            // "dbo.Inputs",
            // c => new
            // {
            //     Id = c.Int(nullable: false, identity: true),





            //     Financial_RequireFinance = c.Boolean(false),
            //     Financial_InterestRate = c.Double(nullable: false),
            //     Financial_AdministrativeCost = c.Double(nullable: false),
            //     Financial_IncentivePayment = c.Double(nullable: false),
            //     Financial_YearsOfIncentivePayment = c.Int(nullable: false),
            //     Financial_AvailableEquity = c.Double(nullable: false),
            //     Financial_LoanAmount = c.Double(nullable: false),
            //     Financial_EquityLoanInterestRate = c.Double(nullable: false),





            //     Storage_RequireStorage = c.Boolean(false),

            //     Storage_StorageTime = c.Double(false),
            //     Storage_PercentDirectlyToPlantGate = c.Double(false),
            //     Storage_PercentStored = c.Double(false),
            //     Storage_StorageMethod = c.String(false),
            //     General_Id = c.Int(),

            //     Storage_CostOption = c.Int(false),
            //     Storage_Description = c.String(),
            //     Storage_PalletCost = c.Decimal(false, 18, 2),
            //     Storage_TarpCost = c.Decimal(false, 18, 2),
            //     Storage_GravelCost = c.Decimal(false, 18, 2),
            //     Storage_LaborCost = c.Decimal(false, 18, 2),
            //     Storage_LandCost = c.Decimal(false, 18, 2),
            //     Storage_UserEstimatedCost = c.Decimal(false, 18, 2),


            // })
            // .PrimaryKey(t => t.Id);

            //CreateTable(
            //   "dbo.Generals",
            //   c => new
            //   {
            //       Id = c.Int(false, true),
            //       State = c.String(false),
            //       County = c.String(false),
            //       Category = c.String(false),
            //       ProjectSize = c.Double(false),
            //       ProjectLife = c.Int(false),
            //       BiomassPriceAtFarmGate = c.Double(false),
            //       LandCost = c.Double(false)
            //   })
            //   .PrimaryKey(t => t.Id);

            CreateTable(
               "dbo.FeedBackEntities",
               c => new
               {
                   Id = c.Int(false, true),
                   Date = c.DateTime(),
                   Name = c.String(),
                   Email = c.String(),
                   Phone = c.String(),
                   Message = c.String()
               })
               .PrimaryKey(t => t.Id);

            CreateTable(
              "dbo.LookUpEntities",
              c => new
              {
                  Id = c.Guid(false),
                  LookUpId = c.Int(false),
                  Name = c.String(),
                  Description = c.String(),
                  Source = c.String(),
                  Label = c.String(),
                  Value = c.String(),
                  LookUpGroup = c.String(),
                  SortOrder = c.Int(false),
                  System = c.Boolean(false)
              })
              .PrimaryKey(t => t.Id);


        }

        public override void Down()
        {
            DropTable("dbo.ProductivityEntities");
            DropTable("dbo.CountyEntities");
            DropTable("dbo.GlossaryEntities");
           // DropTable("dbo.Inputs");
            DropTable("dbo.FeedbackEntities");
           // DropTable("dbo.Generals");
            DropTable("dbo.LookUpEntities");
        }
    }
}
