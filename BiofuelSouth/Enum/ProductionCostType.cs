using System.ComponentModel.DataAnnotations;

namespace BiofuelSouth.Enum
{
    public enum ProductionCostType
    {
        Default,
        [Display(Name = @"Site Preparation")]
        SitePreparation,
        Planting,
        [Display(Name = @"Burning Treatment")]
        BurningTreatment,
        [Display(Name = @"Chemical Treatment")]
        ChemicalTreatment,
        Fertilization,
        Thinning,
        Harvesting,
        [Display(Name = @"Custodial Management")]
        CustodialManagement,
        [Display(Name = @"Annual Lumpsum")]
        AnnualLumpsum
    }
}