namespace BiofuelSouth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateFinancialModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inputs", "Financial_RequireFinance", c => c.Boolean(nullable: false));
            AddColumn("dbo.Inputs", "Financial_InterestRate", c => c.Double(nullable: false));
            AddColumn("dbo.Inputs", "Financial_AdministrativeCost", c => c.Double(nullable: false));
            AddColumn("dbo.Inputs", "Financial_IncentivePayment", c => c.Double(nullable: false));
            AddColumn("dbo.Inputs", "Financial_YearsOfIncentivePayment", c => c.Int(nullable: false));
            AddColumn("dbo.Inputs", "Financial_AvailableEquity", c => c.Double(nullable: false));
            AddColumn("dbo.Inputs", "Financial_LoanAmount", c => c.Double(nullable: false));
            AddColumn("dbo.Inputs", "Financial_EquityLoanInterestRate", c => c.Double(nullable: false));
            DropColumn("dbo.Inputs", "Finance_InterestRate");
            DropColumn("dbo.Inputs", "Finance_AdministrativeCost");
            DropColumn("dbo.Inputs", "Finance_IncentivePayment");
            DropColumn("dbo.Inputs", "Finance_YearsOfIncentivePayment");
            DropColumn("dbo.Inputs", "Finance_AvailableEquity");
            DropColumn("dbo.Inputs", "Finance_LoanAmount");
            DropColumn("dbo.Inputs", "Finance_EquityLoanInterestRate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inputs", "Finance_EquityLoanInterestRate", c => c.Double(nullable: false));
            AddColumn("dbo.Inputs", "Finance_LoanAmount", c => c.Double(nullable: false));
            AddColumn("dbo.Inputs", "Finance_AvailableEquity", c => c.Double(nullable: false));
            AddColumn("dbo.Inputs", "Finance_YearsOfIncentivePayment", c => c.Int(nullable: false));
            AddColumn("dbo.Inputs", "Finance_IncentivePayment", c => c.Double(nullable: false));
            AddColumn("dbo.Inputs", "Finance_AdministrativeCost", c => c.Double(nullable: false));
            AddColumn("dbo.Inputs", "Finance_InterestRate", c => c.Double(nullable: false));
            DropColumn("dbo.Inputs", "Financial_EquityLoanInterestRate");
            DropColumn("dbo.Inputs", "Financial_LoanAmount");
            DropColumn("dbo.Inputs", "Financial_AvailableEquity");
            DropColumn("dbo.Inputs", "Financial_YearsOfIncentivePayment");
            DropColumn("dbo.Inputs", "Financial_IncentivePayment");
            DropColumn("dbo.Inputs", "Financial_AdministrativeCost");
            DropColumn("dbo.Inputs", "Financial_InterestRate");
            DropColumn("dbo.Inputs", "Financial_RequireFinance");
        }
    }
}
