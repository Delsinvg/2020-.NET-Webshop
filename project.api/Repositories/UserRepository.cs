using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using project.api.Entities;
using project.api.Exceptions;
using project.api.Helper;
using project.models.RefreshTokens;
using project.models.Users;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace project.api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ProjectContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppSettings _appSettings;
        private readonly ClaimsPrincipal _user;

        public UserRepository(ProjectContext context, UserManager<User> userManager, RoleManager<Role> roleManager, SignInManager<User> signInManager, IHttpContextAccessor httpContextAccessor, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _appSettings = appSettings.Value;
            _user = _httpContextAccessor.HttpContext.User;
        }

        public async Task DeleteUser(string id)
        {
            User user = await _userManager.FindByIdAsync(id);

            IdentityResult result = await _userManager.DeleteAsync(user);
        }

        public async Task<GetUserModel> GetUser(string id)
        {
            if (_user.Claims.Where(x => x.Type.Contains("role")).Count() == 1 &&
                _user.IsInRole("Customer") &&
                _user.Identity.Name != id.ToString())
            {
                throw new ForbiddenException("Forbidden to get this user details.", this.GetType().Name, "GetUser", "403");
            }

            GetUserModel user = await _context.Users
                .Include(x => x.UserRoles)
                .Select(x => new GetUserModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    Address = $"{x.Address.Country} ({x.Address.CountryCode}) - {x.Address.City} ({x.Address.PostalCode}) - {x.Address.Street}",
                    Roles = x.UserRoles.Select(x => x.Role.Name).ToList()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            return user;
        }

        public async Task<List<GetUserModel>> GetUsers()
        {
            List<GetUserModel> users = await _context.Users
                .Include(x => x.UserRoles)
                .Select(x => new GetUserModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    Address = $"{x.Address.Country} ({x.Address.CountryCode}) - {x.Address.City} ({x.Address.PostalCode}) - {x.Address.Street}",
                    Roles = x.UserRoles.Select(x => x.Role.Name).ToList()
                })
                .AsNoTracking()
                .ToListAsync();

            return users;
        }

        public async Task PatchUser(Guid id, PatchUserModel patchUserModel)
        {
            if (_user.Claims.Where(x => x.Type.Contains("role")).Count() == 1 &&
                _user.IsInRole("Customer") &&
                _user.Identity.Name != id.ToString())
            {
                throw new ForbiddenException("Geen toelating om deze gebruiker zijn wachtwoord te wijzigen", this.GetType().Name, "PatchUser", "403");
            }

            User user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new EntityException("Gebruiker niet gevonden", this.GetType().Name, "PatchUser", "404");
            }

            IdentityResult result = await _userManager.ChangePasswordAsync(user, patchUserModel.CurrentPassword, patchUserModel.NewPassword);

            if (!result.Succeeded)
            {
                throw new IdentityException(result.Errors.First().Description, this.GetType().Name, "PatchUser", "400");
            }
        }

        public async Task<GetUserModel> PostUser(PostUserModel postUserModel)
        {

            EntityEntry<Address> address = await _context.Addresses.AddAsync(new Address
            {
                CountryCode = postUserModel.CountryCode,
                City = postUserModel.City,
                PostalCode = postUserModel.PostalCode,
                Country = postUserModel.Country,
                Street = postUserModel.Street
            });

            await _context.SaveChangesAsync();

            User user = new User
            {
                UserName = postUserModel.Email,
                FirstName = postUserModel.FirstName,
                LastName = postUserModel.LastName,
                Email = postUserModel.Email,
                AddressId = address.Entity.Id
            };

            IdentityResult result = await _userManager.CreateAsync(user, postUserModel.Password);

            if (!result.Succeeded)
            {
                string description = result.Errors.First().Description;

                if (description.Contains("is already taken"))
                {
                    description = "Dit e-mailadres is reeds geregistreerd";
                }

                throw new IdentityException(description, this.GetType().Name, "PostUser", "400");
            }

            try
            {
                if (postUserModel.Roles == null)
                {
                    await _userManager.AddToRoleAsync(user, "Customer");
                }
                else
                {
                    await _userManager.AddToRolesAsync(user, postUserModel.Roles);
                }
            }
            catch (Exception e)
            {
                await _userManager.DeleteAsync(user);
                throw new IdentityException(e.Message, this.GetType().Name, "PostUser", "400");
            }

            return await GetUser(user.Id.ToString());
        }

        public async Task PutUser(string id, PutUserModel putUserModel)
        {
            User user = await _userManager.FindByIdAsync(id);

            await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));

            user.FirstName = putUserModel.FirstName;
            user.LastName = putUserModel.LastName;
            user.Email = putUserModel.Email;
            user.AddressId = putUserModel.AddressId;

            await _userManager.AddToRolesAsync(user, putUserModel.Roles);

            IdentityResult result = await _userManager.UpdateAsync(user);
        }

        public async Task<List<GetRefreshTokenModel>> GetUserRefreshTokens(Guid id)
        {
            List<GetRefreshTokenModel> refreshTokens = await _context.RefreshTokens
                .Where(x => x.UserId == id)
                .Select(x => new GetRefreshTokenModel
                {
                    Id = x.Id,
                    Token = x.Token,
                    Expires = x.Expires,
                    IsExpired = x.IsExpired,
                    Created = x.Created,
                    CreatedByIp = x.CreatedByIp,
                    Revoked = x.Revoked,
                    RevokedByIp = x.RevokedByIp,
                    ReplacedByToken = x.ReplacedByToken,
                    IsActive = x.IsActive
                })
                .AsNoTracking()
                .ToListAsync();

            if (refreshTokens.Count == 0)
            {
                throw new CollectionException("No refresh tokens found.", this.GetType().Name, "GetUserRefreshTokens", "404");
            }

            return refreshTokens;
        }


        public async Task<string> SendResetToken(EmailModel emailModel)
        {

            string code = null;
                User user = await _userManager.FindByNameAsync(emailModel.Email);
                if (user != null)
                {
                    code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var callbackUrl = "https://localhost:44397/Authentication/ResetPassword?UserId=" + user.Id + "&code=" + code;
                    await sendEmail(callbackUrl, emailModel.Email);
                }
            return code;
           
        }

        public async Task sendEmail(string callback, string email)
        {
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("lsd2asp.net@gmail.com", "6@mJB5yuSJ5CvussGgY@");
            var body = "Please reset your password by clicking here: " + callback;
            await client.SendMailAsync("janis.odisee@gmail.com", email, "Reset Password", body);
        }

        // JWT Methods
        // ===========

        public async Task<PostAuthenticateResponseModel> Authenticate(PostAuthenticateRequestModel postAuthenticateRequestModel, string ipAddress)
        {
            User user = await _userManager.Users
                .Include(x => x.RefreshTokens)
                .FirstOrDefaultAsync(x => x.UserName == postAuthenticateRequestModel.UserName);

            if (user == null)
            {
                throw new UserNameException("Invalid username.", this.GetType().Name, "Authenticate", "401");
            }

            // Verify password when user was found by UserName
            SignInResult signInResult = await _signInManager.CheckPasswordSignInAsync(user, postAuthenticateRequestModel.Password, lockoutOnFailure: false);

            if (!signInResult.Succeeded)
            {
                throw new PasswordException("Invalid password.", this.GetType().Name, "Authenticate", "401");
            }

            // Authentication was successful so generate JWT and refresh tokens
            string jwtToken = await GenerateJwtToken(user);
            RefreshToken refreshToken = GenerateRefreshToken(ipAddress);

            // save refresh token
            user.RefreshTokens.Add(refreshToken);

            await _userManager.UpdateAsync(user);

            return new PostAuthenticateResponseModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                JwtToken = jwtToken,
                RefreshToken = refreshToken.Token,
                Roles = await _userManager.GetRolesAsync(user)
            };
        }

        public async Task<PostAuthenticateResponseModel> RefreshToken(string token, string ipAddress)
        {
            User user = await _userManager.Users
                .Include(x => x.RefreshTokens)
                .FirstOrDefaultAsync(x => x.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
            {
                throw new TokenException("No user found with this token.", this.GetType().Name, "RefreshToken", "401");
            }

            RefreshToken refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            // Refresh token is no longer active
            if (!refreshToken.IsActive)
            {
                throw new RefreshTokenException("Refresh token is no longer active.", this.GetType().Name, "RefreshToken", "401");
            };

            // Replace old refresh token with a new one
            RefreshToken newRefreshToken = GenerateRefreshToken(ipAddress);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;

            // Generate new jwt
            string jwtToken = await GenerateJwtToken(user);

            user.RefreshTokens.Add(newRefreshToken);

            await _userManager.UpdateAsync(user);

            return new PostAuthenticateResponseModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                JwtToken = jwtToken,
                RefreshToken = newRefreshToken.Token,
                Roles = await _userManager.GetRolesAsync(user)
            };
        }

        public async Task RevokeToken(string token, string ipAddress)
        {
            User user = await _userManager.Users
                .Include(x => x.RefreshTokens)
                .FirstOrDefaultAsync(x => x.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
            {
                throw new TokenException("No user found with this token.", this.GetType().Name, "RefreshToken", "401");
            }

            RefreshToken refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            // Refresh token is no longer active
            if (!refreshToken.IsActive)
            {
                throw new RefreshTokenException("Refresh token is no longer active.", this.GetType().Name, "RefreshToken", "401");
            };

            // Revoke token and save
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;

            await _userManager.UpdateAsync(user);
        }

        // JWT helper methods
        // ==================

        private async Task<string> GenerateJwtToken(User user)
        {
            var roleNames = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.Id.ToString()));
            claims.Add(new Claim("FirstName", user.FirstName));
            claims.Add(new Claim("LastName", user.LastName));
            claims.Add(new Claim("UserName", user.UserName));

            foreach (string roleName in roleNames)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleName));
            }

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims.ToArray()),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefreshToken GenerateRefreshToken(string ipAddress)
        {
            using RNGCryptoServiceProvider rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            byte[] randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomBytes),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }

        public async Task<string> ValidatePasswordReset(ResetPasswordModel resetPasswordModel)
        {
            IdentityResult result = null;
                User user = await _userManager.FindByIdAsync(resetPasswordModel.UserId.ToString());
                if (user != null)
                {
                    result = await _userManager.ResetPasswordAsync(user, resetPasswordModel.Code, resetPasswordModel.Password);
                }
                return result.ToString();
        }
    }
}

