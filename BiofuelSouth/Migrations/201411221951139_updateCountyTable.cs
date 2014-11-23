namespace BiofuelSouth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateCountyTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Inputs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Category = c.String(),
                        CountyCode = c.Int(nullable: false),
                        State = c.String(),
                        ProjectSize = c.Double(nullable: false),
                        InterestRate = c.Double(nullable: false),
                        ProjectLife = c.Int(nullable: false),
                        LandCost = c.Double(nullable: false),
                        AdministrativeCost = c.Double(nullable: false),
                        IncentivePayment = c.Double(nullable: false),
                        YearsOfIncentivePayment = c.Int(nullable: false),
                        BiomassPriceAtFarmGate = c.Double(nullable: false),
                        AvailableEquity = c.Double(nullable: false),
                        LoanAmount = c.Double(nullable: false),
                        EquityLoanInterestRate = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Inputs");
        }
    }
}
