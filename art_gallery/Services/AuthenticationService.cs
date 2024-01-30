using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using art_gallery.Dtos;
using art_gallery.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace art_gallery.Services
{
    public class AuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AuthenticationService(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var userExists = await _userManager.FindByEmailAsync(request.Email);
                if (userExists != null)
                    return new RegisterResponse
                    {
                        Message = "User already exists",
                        Success = false
                    };

                

                userExists = new ApplicationUser
                {
                    FullName = request.FullName,
                    IsArtist = request.IsArtist,
                    Email = request.Email,

                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    UserName = request.Username,

                };
                var createUserResult = await _userManager.CreateAsync(userExists, request.Password);
                if (!createUserResult.Succeeded)
                    return new RegisterResponse
                    {
                        Message =
                            $"Create user failed {createUserResult?.Errors?.First()?.Description}",
                        Success = false
                    };
                
                var role = DetermineUserRole(userExists.IsArtist);
                var addUserToRoleResult = await _userManager.AddToRoleAsync(userExists, role);
                if (!addUserToRoleResult.Succeeded)
                    return new RegisterResponse
                    {
                        Message =
                            $"Create user succeeded but could not add user to role {addUserToRoleResult?.Errors?.First()?.Description}",
                        Success = false
                    };

                
                return new RegisterResponse
                {
                    Success = true,
                    Message = "User registered successfully"
                };
            }
            catch (Exception ex)
            {
                return new RegisterResponse { Message = ex.Message, Success = false };
            }
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user is null)
                    return new LoginResponse
                    {
                        Message = "Invalid email/password",
                        Success = false
                    };

                //all is well if ew reach here
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                };
                var roles = await _userManager.GetRolesAsync(user);
                var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x));
                claims.AddRange(roleClaims);

                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes("this is my custom Secret key for authentication")
                );
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expires = DateTime.Now.AddMinutes(30);

                var token = new JwtSecurityToken(
                    issuer: "https://localhost:5001",
                    audience: "https://localhost:5001",
                    claims: claims,
                    expires: expires,
                    signingCredentials: creds
                );

                return new LoginResponse
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    Message = "Login Successful",
                    Email = user?.Email,
                    Username = user?.UserName,
                    FullName = user?.FullName,
                    Success = true,
                    UserId = user?.Id.ToString()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new LoginResponse { Success = false, Message = ex.Message };
            }
        }

        public string DetermineUserRole(bool isArtist)
        {
            return isArtist ? "ARTIST" : "USER";
        }
    }
}
