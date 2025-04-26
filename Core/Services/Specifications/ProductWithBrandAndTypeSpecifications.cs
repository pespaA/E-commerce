using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Entities;
using Shared;

namespace Services.Specifications
{
    public class ProductWithBrandAndTypeSpecifications : Specifications<Product>
    {
        // USe To Retrive Product By Id
        public ProductWithBrandAndTypeSpecifications(int id)
        :base(product=>product.Id ==id)
        {
            AddInclude(product => product.ProductBrand);
            AddInclude(product => product.ProductType);
        }


        // Use To Get All Products
        public ProductWithBrandAndTypeSpecifications(ProductSpecificationsParameters parameters)
            :base(product=>
            (!parameters.BrandId.HasValue||product.BrandId== parameters.BrandId)&&
            (!parameters.TypeId.HasValue||product.TypeId== parameters.TypeId))
        {
            AddInclude(product => product.ProductBrand);
            AddInclude(product => product.ProductType);
            #region Sort
            switch (parameters.Sort)
            {
                case ProductSortingOptions.PriceDesc:
                    SetOrderByDescending(product => product.Price);
                    break;
                case ProductSortingOptions.PriceAsc:
                    SetOrderBy(product => product.Price);
                    break;
                case ProductSortingOptions.NameDesc:
                    SetOrderByDescending(product => product.Name);
                    break;
                case ProductSortingOptions.NameAsc:
                    SetOrderBy(product => product.Name);
                    break; 
            }
            #endregion
        }

    }
}
