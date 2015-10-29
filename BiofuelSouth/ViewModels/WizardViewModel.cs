using BiofuelSouth.Enum;
using BiofuelSouth.Models;

namespace BiofuelSouth.ViewModels
{
    public class WizardViewModel
    {
        public WizardStep Step { get; set; }

        public General GeneralView { get; set;  }
        public ProductionCostViewModel ProductionCostView  { get; set; }

        public StorageCostModel StorageView { get; set;  }

        public Financial FinancialView { get; set; }

        public ResultsViewModel ResultsView { get; set; }
        
        public WizardViewModel()
        {
            Step = WizardStep.General;
            GeneralView = new General();
            ProductionCostView = new ProductionCostViewModel();
            StorageView = new StorageCostModel();
            FinancialView = new Financial();
            ResultsView = new ResultsViewModel();

        }
    }
}