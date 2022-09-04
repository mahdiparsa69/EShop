using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using EShop.Api.Models.RequstModels;
using EShop.Domain.Common;
using EShop.Domain.Filters;
using EShop.Domain.Interfaces;
using EShop.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet("{userId:guid}")]
        public async Task<ActionResult<UserViewModel>> GetAsync([FromRoute] Guid userId)
        {
            if (userId == default)
                return BadRequest();

            var user = await _userRepository.GetWithoutIncludeAsync(userId, HttpContext.RequestAborted);

            if (user == null)
                return NotFound();

            var userViewModel = _mapper.Map<User, UserViewModel>(user);

            return Ok(userViewModel);
        }

        [HttpGet]
        public async Task<ActionResult<List<UserViewModel>>> GetListAsync([FromQuery] string? username, [FromQuery] int offset = 0, [FromQuery] int count = 10)
        {

            var users = await _userRepository.GetListAsync(new UserFilter
            {
                Username = username,
                Offset = offset,
                Count = count
            }, HttpContext.RequestAborted);

            if (users?.Items == null || !users.Items.Any())
                return default;

            Response.Headers.Add("nextUrl", $"?offset={offset + count}&count={count}");

            var result = _mapper.Map<List<User>, List<UserViewModel>>(users.Items);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] UserCreateModel userCreateModel)
        {
            if (userCreateModel == null)
                return BadRequest();

            var user = _mapper.Map<UserCreateModel, User>(userCreateModel);

            user.Password = userCreateModel.Password.GetSha256();

            await _userRepository.AddAsync(user, HttpContext.RequestAborted);

            var userViewModel = _mapper.Map<User, UserViewModel>(user);

            return Ok(userViewModel);
        }

        [HttpDelete]
        public async Task<IActionResult> Remove([FromBody] LoginRequest userRequestModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = _mapper.Map<LoginRequest, User>(userRequestModel);

            if (user == null)
                return NotFound();

            await _userRepository.Remove(user, HttpContext.RequestAborted);

            return Ok(user);
        }

        [HttpDelete("{userId:guid}")]
        public async Task<IActionResult> Remove([FromRoute] Guid userId)
        {
            if (userId == null)
                return BadRequest();

            await _userRepository.Remove(userId, HttpContext.RequestAborted);

            return Ok($"User with ID : {userId} was deleted");
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] LoginRequest userRequestModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = _mapper.Map<LoginRequest, User>(userRequestModel);

            await _userRepository.Update(user, HttpContext.RequestAborted);

            return Ok(user);
        }
    }
}
