﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared;
using Shared.ErrorModels;

namespace Persentation
{

    public class ProductController(IServiceManager ServiceManager):ApiController
    {
        #region GetAllProduct
        [HttpGet("Products")]
        public async Task<ActionResult<PaginatedResult<ProductResultDto>>>GetAllProducts([FromQuery] ProductSpecificationsParameters parameters)
        {
            var Products = await ServiceManager.ProductService.GetAllProductAsync(parameters);
            return Ok(Products);
        }
        #endregion
        #region GetAllBrands
        [HttpGet("Brands")]
        public async Task<ActionResult<IEnumerable<BrandResultDto>>> GetAllBrands()
        {
            var Brands = await ServiceManager.ProductService.GetAllBrandsAsync();
            return Ok(Brands);
        }
        #endregion
        #region GetAllTypes
        [HttpGet("Types")]
        public async Task<ActionResult<IEnumerable<TypeResultDto>>> GetAllTypes()
        {
            var Types = await ServiceManager.ProductService.GetAllTypesAsync();
            return Ok(Types);
        }
        #endregion
        #region GetProductById
        
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<ProductResultDto>>> GetProductById(int id)
        {
            var Product = await ServiceManager.ProductService.GetProductByIdAsync(id);
            return Ok(Product);
        }
        #endregion
    }
}
