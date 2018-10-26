using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Microsoft.Extensions.Logging.Debug;
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
        public async Task<IActionResult> Authenticate([FromBody]TableUserDto userDto)
        {
            var user = _userService.Authenticate(userDto.Email, userDto.Password);
            

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
            //Spara token i user databasen
            _userService.StoreToken(tokenString, user.Userid);
            
            return Ok(new
            {
                x_auth_token = tokenString
            });

        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] TableUserDto userDto)
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
                TableUser id = _userService.Update(token, newUserInfo);
                return Ok(new
                {
                    userId = id
                });
            }
            catch (ApplicationException ex)
            {

                return BadRequest(new {message = ex.Message});
            }
        }

    }
}