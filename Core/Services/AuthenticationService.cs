using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Identity;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Abstractions;
using Shared.Security;

namespace Services
{
    public class AuthenticationService(UserManager<User> userManager,IOptions<JwtOptions> options) : IAuthenticationService
    {
        public async Task<UserResultDto> LoginAsync(LoginDto loginDto)
        {
            //Check If There Is User Under This Email 
            var User =await userManager.FindByEmailAsync(loginDto.Email);
            if (User == null) throw new UnAuthorizedexceptions("InCorrect Email");// Email Validation
            //Check If The Password Is Correct For This Email 
            var Result = await userManager.CheckPasswordAsync(User, loginDto.Password);
            if (!Result) throw new UnAuthorizedexceptions("InCorrect Password");// Password Validation
            //Create Token And Return Response
            return new UserResultDto
                (
                User.DisplayName,
                User.Email,
                await CreateTokenAsync(User)
                );
        }

        public async Task<UserResultDto> RegisterAsync(UserRegisterDto userRegisterDto)
        {
            var User = new User()
            {
                Email = userRegisterDto.Email,
                DisplayName = userRegisterDto.DisplayName,
                PhoneNumber = userRegisterDto.phoneNumber,
                UserName = userRegisterDto.UserName,
            };
            var Result = await userManager.CreateAsync(User, userRegisterDto.Password);
            if (!Result.Succeeded)
            {
                var errors = Result.Errors.Select(e=>e.Description).ToList();
                throw new RegisterValidationExceptions(errors);
            }
            return new UserResultDto
                (
                    User.DisplayName,
                    User.Email,
                    await CreateTokenAsync(User)
                );
        }
        public async Task<string> CreateTokenAsync(User user)
        {
            var Jwtoption = options.Value;
            //Private Claims
            var AuthClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.UserName)!,
                new Claim(ClaimTypes.Email,user.Email)!
            };
            // Add Roles To Claims If Exist
            var Roles = await userManager.GetRolesAsync(user);
            foreach (var Role in Roles)
            {
                AuthClaims.Add(new Claim(ClaimTypes.Role, Role));
            }
            // For Secret Key
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Jwtoption.SecretKey));// For Key
            var SigningCredintials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
            //Token
            var Token = new JwtSecurityToken(
                audience: Jwtoption.Audience,
                issuer: Jwtoption.Issure,
                expires: DateTime.UtcNow.AddDays(Jwtoption.DurationsInDays),
                claims:AuthClaims,
                signingCredentials: SigningCredintials
                );
            return new JwtSecurityTokenHandler().WriteToken(Token);
            
        }
    }
}
