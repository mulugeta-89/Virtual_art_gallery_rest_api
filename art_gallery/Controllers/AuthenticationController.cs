﻿using art_gallery.Dtos;
using art_gallery.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using art_gallery.Services;
using System.Security.Claims;
using System.Text;

namespace art_gallery.Controllers
{
    [ApiController]
    [Route("/api/authenticate")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public readonly AuthenticationService _authenticationService;
        public AuthenticationController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, AuthenticationService authenticationService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _authenticationService = authenticationService;
        }


        [HttpPost]
        [Route("roles/add")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
        {
            var appRole = new ApplicationRole { Name = request.Role };
            var createRole = await _roleManager.CreateAsync(appRole);

            return Ok(new { message = "role created succesfully" });
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await RegisterAsync(request);

            return result.Success ? Ok(result) : BadRequest(result.Message);
        }
        private async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var userExists = await _userManager.FindByEmailAsync(request.Email);
                if (userExists != null) return new RegisterResponse { Message = "User already exists", Success = false };

                //if we get here, no user with this email..

                userExists = new ApplicationUser
                {
                    FullName = request.FullName,
                    IsArtist = request.IsArtist,
                    Email = request.Email,

                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    UserName = request.Email,

                };
                var createUserResult = await _userManager.CreateAsync(userExists, request.Password);
                if (!createUserResult.Succeeded) return new RegisterResponse { Message = $"Create user failed {createUserResult?.Errors?.First()?.Description}", Success = false };
                //user is created...
                //then add user to a role...
                var role = _authenticationService.DetermineUserRole(userExists.IsArtist);
                var addUserToRoleResult = await _userManager.AddToRoleAsync(userExists, role);
                if (!addUserToRoleResult.Succeeded) return new RegisterResponse { Message = $"Create user succeeded but could not add user to role {addUserToRoleResult?.Errors?.First()?.Description}", Success = false };

                //all is still well..
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
        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }


        [HttpPost]
        [Route("login")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(LoginResponse))]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await LoginAsync(request);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }
        private async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            try
            {

                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user is null) return new LoginResponse { Message = "Invalid email/password", Success = false };

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

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authentication"));
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
    }
}
