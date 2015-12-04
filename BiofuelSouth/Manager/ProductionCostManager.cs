using System;
using BiofuelSouth.Enum;
using BiofuelSouth.Models;
using BiofuelSouth.Services;

namespace BiofuelSouth.Manager
{
    public interface IProductionCostManager
    {
        /// <summary>
        /// Returns production costs for the crop type
        /// </summary>
        /// <param name="productionCostViewModel"></param>
        /// <returns></returns>
        ProductionCostViewModel GetProductionCost(ProductionCostViewModel productionCostViewModel);

    }
    public class ProductionCostManager : IProductionCostManager
    {
        public ProductionCostViewModel GetProductionCost(ProductionCostViewModel productionCostViewModel)
        {
            if (productionCostViewModel == null)
                return null;
            switch (productionCostViewModel.CropType)
            {
                case CropType.Switchgrass:
                    return GetSwitchgrassProductionCost(productionCostViewModel);
                case CropType.Miscanthus:
                    return GetMiscanthusProductionCost(productionCostViewModel);
                case CropType.Poplar:
                    return GetPoplarProductionCost(productionCostViewModel);
                case CropType.Willow:
                    return GetWillowProductionCost(productionCostViewModel);
                case CropType.Pine:
                    return GetPineProductionCost(productionCostViewModel);
            }
            return null;
        }

        private ProductionCostViewModel GetPineProductionCost(ProductionCostViewModel productionCostViewModel)
        {
            productionCostViewModel.Amount = 657;

            productionCostViewModel.ProductionCosts.Add(new ProductionCost
            {
                ProductionCostType = ProductionCostType.SitePreparation,
                IsRequired = true,
                Amount = 168,
            });

            productionCostViewModel.ProductionCosts.Add(new ProductionCost
            {
                ProductionCostType = ProductionCostType.Planting,
                IsRequired = true,
                Amount = 81
            });

            productionCostViewModel.ProductionCosts.Add(new ProductionCost
            {
                ProductionCostType = ProductionCostType.Thinning,
                IsRequired = true,
                Amount = 50,
            });

            productionCostViewModel.ProductionCosts.Add(new ProductionCost
            {
                ProductionCostType = ProductionCostType.Harvesting,
                IsRequired = true,
                Amount = 350,
            });

            productionCostViewModel.ProductionCosts.Add(new ProductionCost
            {
                ProductionCostType = ProductionCostType.CustodialManagement,
                IsRequired = true,
                Amount = 6,
            });

            return productionCostViewModel;
        }

        private ProductionCostViewModel GetPoplarProductionCost(ProductionCostViewModel productionCostViewModel)
        {
            productionCostViewModel.Amount = 657;
            productionCostViewModel.ProductionCosts.Add(new ProductionCost
            {
                ProductionCostType = ProductionCostType.SitePreparation,
                IsRequired = true,
                Amount = 168,
            });

            productionCostViewModel.ProductionCosts.Add(new ProductionCost
            {
                ProductionCostType = ProductionCostType.Planting,
                IsRequired = true,
                Amount = 81
            });

            productionCostViewModel.ProductionCosts.Add(new ProductionCost
            {
                ProductionCostType = ProductionCostType.Thinning,
                IsRequired = true,
                Amount = 50,
            });

            productionCostViewModel.ProductionCosts.Add(new ProductionCost
            {
                ProductionCostType = ProductionCostType.Harvesting,
                IsRequired = true,
                Amount = 350,
            });

            productionCostViewModel.ProductionCosts.Add(new ProductionCost
            {
                ProductionCostType = ProductionCostType.CustodialManagement,
                IsRequired = true,
                Amount = 6,
            });

            return productionCostViewModel;

        }

        private ProductionCostViewModel GetMiscanthusProductionCost(ProductionCostViewModel productionCostViewModel)
        {
            productionCostViewModel.Amount = (decimal) Math.Round((DataService.GetCostPerAcreForCropByGeoId(CropType.Miscanthus,
                productionCostViewModel.County) * DataService.GetProductivityPerAcreForCropByGeoId(
                                                            CropType.Miscanthus,
                                                            productionCostViewModel.County)),0);

            productionCostViewModel.ProductionCosts.Add(new ProductionCost
            {
                ProductionCostType = ProductionCostType.SitePreparation,
                IsRequired = true,
                Amount = 168,
            });

            productionCostViewModel.ProductionCosts.Add(new ProductionCost
            {
                ProductionCostType = ProductionCostType.Planting,
                IsRequired = true,
                Amount = 81
            });

            productionCostViewModel.ProductionCosts.Add(new ProductionCost
            {
                ProductionCostType = ProductionCostType.Thinning,
                IsRequired = true,
                Amount = 50,
            });

            productionCostViewModel.ProductionCosts.Add(new ProductionCost
            {
                ProductionCostType = ProductionCostType.Harvesting,
                IsRequired = true,
                Amount = 350,
            });

            productionCostViewModel.ProductionCosts.Add(new ProductionCost
            {
                ProductionCostType = ProductionCostType.CustodialManagement,
                IsRequired = true,
                Amount = 6,
            });

            return productionCostViewModel;

        }

        private ProductionCostViewModel GetSwitchgrassProductionCost(ProductionCostViewModel productionCostViewModel)
        {
            productionCostViewModel.Amount = (decimal) Math.Round((
                DataService.GetCostPerAcreForCropByGeoId(CropType.Switchgrass,
                    productionCostViewModel.County) *
                DataService.GetProductivityPerAcreForCropByGeoId(CropType.Switchgrass,
                    productionCostViewModel.County)),0);

            productionCostViewModel.ProductionCosts.Add(new ProductionCost
            {
                ProductionCostType = ProductionCostType.SitePreparation,
                IsRequired = true,
                Amount = 168,
            });

            productionCostViewModel.ProductionCosts.Add(new ProductionCost
            {
                ProductionCostType = ProductionCostType.Planting,
                IsRequired = true,
                Amount = 81
            });

            productionCostViewModel.ProductionCosts.Add(new ProductionCost
            {
                ProductionCostType = ProductionCostType.Thinning,
                IsRequired = true,
                Amount = 50,
            });

            productionCostViewModel.ProductionCosts.Add(new ProductionCost
            {
                ProductionCostType = ProductionCostType.Harvesting,
                IsRequired = true,
                Amount = 350,
            });

            productionCostViewModel.ProductionCosts.Add(new ProductionCost
            {
                ProductionCostType = ProductionCostType.CustodialManagement,
                IsRequired = true,
                Amount = 6,
            });

            return productionCostViewModel;
        }

        private ProductionCostViewModel GetWillowProductionCost(ProductionCostViewModel productionCostViewModel)
        {
            productionCostViewModel.Amount = 657;
            productionCostViewModel.ProductionCosts.Add(new ProductionCost
            {
                ProductionCostType = ProductionCostType.SitePreparation,
                IsRequired = true,
                Amount = 168,
            });

            productionCostViewModel.ProductionCosts.Add(new ProductionCost
            {
                ProductionCostType = ProductionCostType.Planting,
                IsRequired = true,
                Amount = 81
            });

            productionCostViewModel.ProductionCosts.Add(new ProductionCost
            {
                ProductionCostType = ProductionCostType.Thinning,
                IsRequired = true,
                Amount = 50,
            });

            productionCostViewModel.ProductionCosts.Add(new ProductionCost
            {
                ProductionCostType = ProductionCostType.Harvesting,
                IsRequired = true,
                Amount = 350,
            });

            productionCostViewModel.ProductionCosts.Add(new ProductionCost
            {
                ProductionCostType = ProductionCostType.CustodialManagement,
                IsRequired = true,
                Amount = 6,
            });

            return productionCostViewModel;

        }

    }
}