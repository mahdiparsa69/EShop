using AutoMapper;
using EShop.Api.Models.RequstModels;
using EShop.Domain.Common;
using EShop.Domain.Enums;
using EShop.Domain.Interfaces;
using EShop.Domain.Models;
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
        public async Task<IActionResult> Login([FromBody] UserRequestModel userRequestModel)
        {
            if (string.IsNullOrWhiteSpace(userRequestModel.UserName) || string.IsNullOrWhiteSpace(userRequestModel.Password))
                return BadRequest();

            var userModel = _mapper.Map<UserRequestModel, User>(userRequestModel);

            var user = await _userRepository.GetUser(userModel);

            if (user?.Username == null) return NotFound();

            var jwtPayload = new JWTPayload();

            jwtPayload.userId = user.Id;

            var generatedToken = _tokenService.BuildToken(jwtPayload, _configuration["Jwt:Key"].ToString(), JwtHashAlgorithm.HS256);

            return Ok(generatedToken);
        }
    }
}
