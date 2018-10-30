using System;
using System.Collections.Generic;
<<<<<<< HEAD
using System.Diagnostics;
=======
>>>>>>> c97239a217eaee684f79b15c3d37c53fcd0fbc44
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
<<<<<<< HEAD
using Microsoft.Extensions.Logging.Debug;
=======
>>>>>>> c97239a217eaee684f79b15c3d37c53fcd0fbc44
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using UserAPI.Dto;
using UserAPI.Helpers;
using UserAPI.Models;
using UserAPI.Services;

namespace UserAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UsersController(IUserService userService, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("login")]
<<<<<<< HEAD
        public async Task<IActionResult> Authenticate([FromBody]TableUserDto userDto)
        {
            var user = _userService.Authenticate(userDto.Email, userDto.Password);
            
=======
        public IActionResult Authenticate([FromBody]TableUserDto userDto)
        {
            var user = _userService.Authenticate(userDto.Email, userDto.Password);
>>>>>>> c97239a217eaee684f79b15c3d37c53fcd0fbc44

            if (user == null)
            {
                return BadRequest(new { message = "Username or password incorrect" });
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("UserSystemSecret");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Userid.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
<<<<<<< HEAD
            //Spara token i user databasen
            _userService.StoreToken(tokenString, user.Userid);
            
=======

>>>>>>> c97239a217eaee684f79b15c3d37c53fcd0fbc44
            return Ok(new
            {
                x_auth_token = tokenString
            });

        }

<<<<<<< HEAD
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] TableUserDto userDto)
=======
        [HttpPost("register")]
        public IActionResult Register([FromBody] TableUserDto userDto)
>>>>>>> c97239a217eaee684f79b15c3d37c53fcd0fbc44
        {
            var user = _mapper.Map<TableUser>(userDto);

            try
            {
                _userService.Create(user, userDto.Password);
                return Ok();
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });

            }
        }
<<<<<<< HEAD
        
        //Hämtar användarinfo baserat på token
        [HttpGet("user")]
        public async Task<IActionResult> GetUser([FromHeader] String authtoken)
        {
            try
            {
                TableUser user = _userService.GetUserInfo(authtoken);
                return Ok(new
                {
                    message = user.Userid
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = authtoken});
            }
            
        }
        
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromHeader] String token, [FromBody] String newUserInfo)
        {
            try
            {
                _userService.Update(token, newUserInfo);
                return Ok();
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }
=======
>>>>>>> c97239a217eaee684f79b15c3d37c53fcd0fbc44

    }
}