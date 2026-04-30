using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.User;
using BookStoreApp.API.Static;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStoreApp.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase //cip...31
  {
    private readonly ILogger<AuthController> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<ApiUser> _userManager;
    private readonly IConfiguration _configuration;

    public AuthController(ILogger<AuthController> logger, IMapper mapper, UserManager<ApiUser> userManager, IConfiguration configuration)
    {
      this._logger = logger;
      this._mapper = mapper;
      this._userManager = userManager;
      this._configuration = configuration;
    }

    [HttpPost]
    [Route("register")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> Register(UserDto userDto)
    {
      /* not needed because .net will validate the model automatically via [Required] attribute
      if (userDto == null)
      {
          _logger.LogWarning("User registration attempted with null data.");
          return BadRequest("Invalid user data.");
      }*/
      // Registration logic here
      _logger.LogInformation($"{nameof(Register)} request: {userDto.Email}");
      try
      {
        var user = _mapper.Map<ApiUser>(userDto); // Assuming ApiUser is the user entity
        user.UserName = userDto.Email; // Set the UserName to the email

        var result = await _userManager.CreateAsync(user, userDto.Password);

        if (!result.Succeeded)
        {
          foreach (var error in result.Errors)
          {
            ModelState.AddModelError(error.Code, error.Description);
          }
          return BadRequest(ModelState);
        }

        //assign the role to the user
        await _userManager.AddToRoleAsync(user, userDto.Role);

        //return Ok(new { Message = "User registered successfully." });
        //return Accepted(new { Message = "User registered successfully." });
        _logger.LogInformation($"Successful {nameof(Login).ToLower()}: {userDto.Email}");
        return Accepted();
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, $"An error occurred in {nameof(Register)}.");
        return Problem($"An error occurred in {nameof(Register)}.", statusCode: 500);
      }
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status202Accepted)]
    public async Task<ActionResult<AuthResponse>> Login(LoginUserDto userDto)
    {
      _logger.LogInformation($"{nameof(Login)} request: {userDto.Email}");
      try
      {
        var user = await _userManager.FindByEmailAsync(userDto.Email);
        var validPassword = await _userManager.CheckPasswordAsync(user, userDto.Password);

        if (user == null || !validPassword)
        {
          return Unauthorized("Invalid email or password."); //Unauthorized = 401
        }

        // For simplicity, returning a success message
        //return Ok(new { Message = "Login successful." });
        //return Accepted(); // Return 202 Accepted for successful login

        string tokenString = await GenerateTokenAsync(user); //cip...33

        var response = new AuthResponse
        {
          UserId = user.Id,
          Token = tokenString,
          Email = userDto.Email
        };

        _logger.LogInformation($"Successful {nameof(Login).ToLower()}: {userDto.Email}");
        return Accepted(response); // Return 202 Accepted for successful login
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, $"An error occurred in {nameof(Login)}.");
        return Problem($"An error occurred in {nameof(Login)}.", statusCode: 500);
      }
    }

    private async Task<string> GenerateTokenAsync(ApiUser user) //cip...33
    {
      var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
      var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

      var roles = await _userManager.GetRolesAsync(user);
      var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();

      var userClaims = await _userManager.GetClaimsAsync(user);

      var claims = new List<Claim>
      {
        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim(CustomClaimTypes.Uid, user.Id)
      }
      .Union(roleClaims)
      .Union(userClaims);

      var token = new JwtSecurityToken(
        issuer: _configuration["JwtSettings:Issuer"],
        audience: _configuration["JwtSettings:Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddHours(Convert.ToInt32(_configuration["JwtSettings:Duration"])),
        signingCredentials: credentials
      );

      return new JwtSecurityTokenHandler().WriteToken(token); //convert token to a string
    }
  }
}
