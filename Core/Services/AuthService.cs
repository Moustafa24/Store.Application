using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain.Exceptions;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Services.Abstractions;
using Shared;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Shared.OrderModels;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Services
{
    public class AuthService(UserManager<AppUser> userManager 
        ,IOptions<JwtOptions> options,
        IMapper mapper
       ) : IAuthService
    {
       
        public async Task<UserResultDto> LoginAsync(LoginDto loginDto)
        {
           var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (user is null) throw new UnauthorizedAccessException();

           var flag =  await userManager.CheckPasswordAsync(user, loginDto.Password);
        
            if (!flag) throw new UnauthorizedAccessException();

            return new UserResultDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email ,
                Token =  await GenerateJWTTokenAsync(user),

            };
        }

        public async Task<UserResultDto> RegisterAsync(RegisterDto registerDto)
        {
            if (await CheckEmailExistAsync(registerDto.Email))
            {
                throw new DuplicatedEmailBadRequestException(registerDto.Email);
            }

            var user = new AppUser()
            {
                DisplayName =  registerDto.DisplayName,
                Email = registerDto.Email ,
                UserName = registerDto.UserName ,
                PhoneNumber = registerDto.PhoneNumber ,


            };
            var result = await userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(error => error.Description);
                throw new ValidationExceptions(errors);
            }
            return new UserResultDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await GenerateJWTTokenAsync(user),

            };
        }

       

        private async Task<string> GenerateJWTTokenAsync(AppUser user)
        {
            //Header 
            //Payload
            //Signature

            var jwtOPtions = options.Value;

            var authClaim = new List<Claim>()
            {
                new Claim(ClaimTypes.Name , user.UserName),
                new Claim(ClaimTypes.Email , user.Email)
            };

            var roles = await userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                authClaim.Add(new Claim(ClaimTypes.Role, role));
            }

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOPtions.SecretKey));


            var token = new JwtSecurityToken(
                issuer:jwtOPtions.Issuer,
                audience:jwtOPtions.Audience,
                claims: authClaim,
                expires: DateTime.UtcNow.AddDays(jwtOPtions.DyrationInDay),
                signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature)


            );


            //Token

            return new JwtSecurityTokenHandler().WriteToken(token);

        }



        public async Task<bool> CheckEmailExistAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task<UserResultDto> GetCurrentUserAsync(string email)
        {

            var user = await userManager.FindByEmailAsync(email);
            if (user is null) throw new UserNotFoundException(email);
            return new UserResultDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await GenerateJWTTokenAsync(user),
            };



        }

        public async Task<AddressDto> GetCurrentUserAddressAsync(string email)
        {
           var user =  await userManager.Users.Include(U => U.Address).FirstOrDefaultAsync(U => U.Email == email);                               
            
            if(user is null) throw new UserNotFoundException(email);

            var result= mapper.Map<AddressDto>(user);

            return result;
        
        }

        public async Task<AddressDto> UpdateCurrentUserAddressAsync(AddressDto address, string email)
        {
            var user = await userManager.Users.Include(U => U.Address).FirstOrDefaultAsync(U => U.Email == email);

            if (user is null) throw new UserNotFoundException(email);

            if(user.Address is not null )
            {
                user.Address.FirstName = address.FirstName;
                user.Address.LastName = address.LastName;
                user.Address.Street = address.Street;
                user.Address.City = address.City;
                user.Address.Country = address.Country;
            }else
            {
              var AddressResult =    mapper.Map<Address>(address);    

                user.Address = AddressResult; 
            }

           await userManager.UpdateAsync(user);

            return address;
        }

    }
}
