namespace BiofuelSouth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovesInputTableFromDB : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inputs", "General_Id", "dbo.Generals");
            DropIndex("dbo.Inputs", new[] { "General_Id" });
            DropTable("dbo.Inputs");
            DropTable("dbo.Generals");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Generals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        State = c.String(nullable: false),
                        County = c.String(nullable: false),
                        Category = c.String(nullable: false),
                        ProjectSize = c.Double(nullable: false),
                        ProjectLife = c.Int(nullable: false),
                        BiomassPriceAtFarmGate = c.Double(nullable: false),
                        LandCost = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Inputs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Storage_RequireStorage = c.Boolean(nullable: false),
                        Storage_StorageTime = c.Double(nullable: false),
                        Storage_PercentDirectlyToPlantGate = c.Double(nullable: false),
                        Storage_PercentStored = c.Double(nullable: false),
                        Storage_StorageMethod = c.String(nullable: false),
                        Storage_CostOption = c.Int(nullable: false),
                        Storage_Description = c.String(),
                        Storage_PalletCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Storage_TarpCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Storage_GravelCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Storage_LaborCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Storage_LandCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Storage_UserEstimatedCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Financial_RequireFinance = c.Boolean(nullable: false),
                        Financial_InterestRate = c.Double(nullable: false),
                        Financial_AdministrativeCost = c.Double(nullable: false),
                        Financial_IncentivePayment = c.Double(nullable: false),
                        Financial_YearsOfIncentivePayment = c.Int(nullable: false),
                        Financial_AvailableEquity = c.Double(nullable: false),
                        Financial_LoanAmount = c.Double(nullable: false),
                        Financial_EquityLoanInterestRate = c.Double(nullable: false),
                        General_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Inputs", "General_Id");
            AddForeignKey("dbo.Inputs", "General_Id", "dbo.Generals", "Id");
        }
    }
}
