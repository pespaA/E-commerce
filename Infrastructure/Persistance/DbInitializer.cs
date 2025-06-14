﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistance.Data;

namespace Persistance
{
    public class DbInitializer : IDbInitializer
    {
        private readonly StoreContext _storeContext;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(StoreContext storeContext,UserManager<User> userManager,RoleManager<IdentityRole> roleManager) 
        {
            _storeContext = storeContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task InitializeAsync()
        {
            try
            {
                #region ForUpdatingDataBase
                //Create DataBase If It Doesnot Exist And Applying Any Pending Migration
                if (_storeContext.Database.GetPendingMigrations().Any())
                    await _storeContext.Database.MigrateAsync();
                #endregion
                #region ProductTypes
                if (!_storeContext.ProductTypes.Any())
                {
                    //Read Types From File As String
                    var TypesData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistance\Data\Seeding\types.json");
                    //Transform Into C# objets
                    var Types = JsonSerializer.Deserialize<List<ProductType>>(TypesData);
                    //Add to Db $ Save Changes
                    if (Types is not null && Types.Any())
                    {
                        await _storeContext.ProductTypes.AddRangeAsync(Types);
                        await _storeContext.SaveChangesAsync();
                    }
                }
                #endregion
                #region ProductBrands
                if (!_storeContext.ProductBrands.Any())
                {
                    //Read Brands From File As String
                    var BrandsData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistance\Data\Seeding\brands.json");
                    //Transform Into C# objets
                    var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsData);
                    //Add to Db $ Save Changes
                    if (Brands is not null && Brands.Any())
                    {
                        await _storeContext.ProductBrands.AddRangeAsync(Brands);
                        await _storeContext.SaveChangesAsync();
                    }
                }
                #endregion
                #region Product
                if (!_storeContext.Products.Any())
                {
                    //Read Products From File As String
                    var ProductsData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistance\Data\Seeding\products.json");
                    //Transform Into C# objets
                    var Products = JsonSerializer.Deserialize<List<Product>>(ProductsData);
                    //Add to Db $ Save Changes
                    if (Products is not null && Products.Any())
                    {
                        await _storeContext.Products.AddRangeAsync(Products);
                        await _storeContext.SaveChangesAsync();
                    }
                }
                #endregion
            }
            catch (Exception)
            {
                throw;
            }


        }

        public async Task InitializeIdentityAsync()
        {
            //Seed Defult Roles
            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            //Seed Defult Roles
            if (!_userManager.Users.Any())
            {
                var SuperAdminUser = new User
                {
                    DisplayName = "SuperAdminUser",
                    Email = "SuperAdminUser@gmail.com",
                    UserName = "SuperAdminUser",
                    PhoneNumber = "1234567890",
                };
                var AdminUser = new User
                {
                    DisplayName = "AdminUser",
                    Email = "AdminUser@gmail.com",
                    UserName = "AdminUser",
                    PhoneNumber = "1234567890",
                };
                await _userManager.CreateAsync(SuperAdminUser,"Passw0rd");
                await _userManager.CreateAsync(AdminUser, "Passw0rd");
                // Set Roles
                await _userManager.AddToRoleAsync(SuperAdminUser, "SuperAdmin");
                await _userManager.AddToRoleAsync(AdminUser, "Admin");

            }
        }
    }
}
