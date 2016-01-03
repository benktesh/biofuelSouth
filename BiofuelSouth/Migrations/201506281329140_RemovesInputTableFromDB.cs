using System.Data.Entity.Migrations;

namespace BiofuelSouth.Migrations
{
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
                        Id = c.Int(false, true),
                        State = c.String(false),
                        County = c.String(false),
                        Category = c.String(false),
                        ProjectSize = c.Double(false),
                        ProjectLife = c.Int(false),
                        BiomassPriceAtFarmGate = c.Double(false),
                        LandCost = c.Double(false)
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Inputs",
                c => new
                    {
                        Id = c.Int(false, true),
                        Storage_RequireStorage = c.Boolean(false),
                        Storage_StorageTime = c.Double(false),
                        Storage_PercentDirectlyToPlantGate = c.Double(false),
                        Storage_PercentStored = c.Double(false),
                        Storage_StorageMethod = c.String(false),
                        Storage_CostOption = c.Int(false),
                        Storage_Description = c.String(),
                        Storage_PalletCost = c.Decimal(false, 18, 2),
                        Storage_TarpCost = c.Decimal(false, 18, 2),
                        Storage_GravelCost = c.Decimal(false, 18, 2),
                        Storage_LaborCost = c.Decimal(false, 18, 2),
                        Storage_LandCost = c.Decimal(false, 18, 2),
                        Storage_UserEstimatedCost = c.Decimal(false, 18, 2),
                        Financial_RequireFinance = c.Boolean(false),
                        Financial_InterestRate = c.Double(false),
                        Financial_AdministrativeCost = c.Double(false),
                        Financial_IncentivePayment = c.Double(false),
                        Financial_YearsOfIncentivePayment = c.Int(false),
                        Financial_AvailableEquity = c.Double(false),
                        Financial_LoanAmount = c.Double(false),
                        Financial_EquityLoanInterestRate = c.Double(false),
                        General_Id = c.Int()
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Inputs", "General_Id");
            AddForeignKey("dbo.Inputs", "General_Id", "dbo.Generals", "Id");
        }
    }
}
