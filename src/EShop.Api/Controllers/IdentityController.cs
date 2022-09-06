using AutoMapper;
using EShop.Api.Models.RequstModels;
using EShop.Api.Models.ViewModels;
using EShop.Domain.Common;
using EShop.Domain.Enums;
using EShop.Domain.Interfaces;
using EShop.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public IdentityController(IUserRepository userRepository, IMapper mapper, ITokenService tokenService, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _tokenService = tokenService;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {

            var user = await _userRepository.GetUserByUsernameAsync(request.UserName, HttpContext.RequestAborted);

            if (user == null)
                return NotFound();

            var hashPassword = request.Password.GetSha256();

            if (user.Password != hashPassword)
                return BadRequest("Username or Password is not valid");

            var eShopAccessTokenPayload = new EShopAccessTokenPayload()
            {
                userId = user.Id,
                ExpireTokenTime = DateTime.Now.AddMinutes(30)
            };

            var generatedToken = _tokenService.BuildToken(eShopAccessTokenPayload, JwtHashAlgorithm.HS256);

            LoginResponse response = new LoginResponse()
            {
                AccessToken = generatedToken
            };

            return Ok(response);
        }
    }
}
