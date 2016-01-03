using System.Data.Entity.Migrations;

namespace BiofuelSouth.Migrations
{
    public partial class modelchange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inputs", "Finance_InterestRate", c => c.Double(false));
            AddColumn("dbo.Inputs", "Finance_LandCost", c => c.Double(false));
            AddColumn("dbo.Inputs", "Finance_AdministrativeCost", c => c.Double(false));
            AddColumn("dbo.Inputs", "Finance_IncentivePayment", c => c.Double(false));
            AddColumn("dbo.Inputs", "Finance_YearsOfIncentivePayment", c => c.Int(false));
            AddColumn("dbo.Inputs", "Finance_AvailableEquity", c => c.Double(false));
            AddColumn("dbo.Inputs", "Finance_LoanAmount", c => c.Double(false));
            AddColumn("dbo.Inputs", "Finance_EquityLoanInterestRate", c => c.Double(false));
            DropColumn("dbo.Inputs", "InterestRate");
            DropColumn("dbo.Inputs", "LandCost");
            DropColumn("dbo.Inputs", "AdministrativeCost");
            DropColumn("dbo.Inputs", "IncentivePayment");
            DropColumn("dbo.Inputs", "YearsOfIncentivePayment");
            DropColumn("dbo.Inputs", "AvailableEquity");
            DropColumn("dbo.Inputs", "LoanAmount");
            DropColumn("dbo.Inputs", "EquityLoanInterestRate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inputs", "EquityLoanInterestRate", c => c.Double(false));
            AddColumn("dbo.Inputs", "LoanAmount", c => c.Double(false));
            AddColumn("dbo.Inputs", "AvailableEquity", c => c.Double(false));
            AddColumn("dbo.Inputs", "YearsOfIncentivePayment", c => c.Int(false));
            AddColumn("dbo.Inputs", "IncentivePayment", c => c.Double(false));
            AddColumn("dbo.Inputs", "AdministrativeCost", c => c.Double(false));
            AddColumn("dbo.Inputs", "LandCost", c => c.Double(false));
            AddColumn("dbo.Inputs", "InterestRate", c => c.Double(false));
            DropColumn("dbo.Inputs", "Finance_EquityLoanInterestRate");
            DropColumn("dbo.Inputs", "Finance_LoanAmount");
            DropColumn("dbo.Inputs", "Finance_AvailableEquity");
            DropColumn("dbo.Inputs", "Finance_YearsOfIncentivePayment");
            DropColumn("dbo.Inputs", "Finance_IncentivePayment");
            DropColumn("dbo.Inputs", "Finance_AdministrativeCost");
            DropColumn("dbo.Inputs", "Finance_LandCost");
            DropColumn("dbo.Inputs", "Finance_InterestRate");
        }
    }
}
