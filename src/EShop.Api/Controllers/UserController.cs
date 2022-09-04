using AutoMapper;
using EShop.Api.Models.RequstModels;
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

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromBody] UserRequestModel userRequest)
        {
            if (string.IsNullOrWhiteSpace(userRequest.UserName) || string.IsNullOrWhiteSpace(userRequest.Password))
                return BadRequest("User specefication can not be null");

            var user = _mapper.Map<UserRequestModel, User>(userRequest);

            var userFromDb = await _userRepository.GetUser(user);

            var userViewModel = _mapper.Map<User, UserViewModel>(userFromDb);

            return Ok(userViewModel);
        }

        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid userId)
        {
            if (userId == default || userId == null)
                return BadRequest();

            var user = await _userRepository.GetWithoutIncludeAsync(userId, HttpContext.RequestAborted);

            if (user == null)
                return NotFound();

            var userViewModel = _mapper.Map<User, UserViewModel>(user);

            return Ok(userViewModel);
        }

        [HttpGet("getusers")]
        public async Task<IActionResult> GetListAsync([FromQuery] string? username, [FromQuery] int offset = 0, [FromQuery] int count = 10)
        {
            UserFilter filter = new UserFilter()
            {
                Username = username,
                Offset = offset,
                Count = count
            };

            var users = await _userRepository.GetListAsync(filter, HttpContext.RequestAborted);

            if (users.Items == null)
                return default;

            var result = _mapper.Map<List<User>, List<UserViewModel>>(users.Items);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] UserCreateModel userCreateModel)
        {
            if (userCreateModel == null)
                return BadRequest();

            var user = _mapper.Map<UserCreateModel, User>(userCreateModel);

            await _userRepository.AddAsync(user, HttpContext.RequestAborted);

            var userViewModel = _mapper.Map<User, UserViewModel>(user);

            return Ok(userViewModel);
        }

        [HttpDelete]
        public async Task<IActionResult> Remove([FromBody] UserRequestModel userRequestModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = _mapper.Map<UserRequestModel, User>(userRequestModel);

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
        public async Task<IActionResult> Update([FromBody] UserRequestModel userRequestModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = _mapper.Map<UserRequestModel, User>(userRequestModel);

            _userRepository.Update(user, HttpContext.RequestAborted);

            return Ok(user);
        }
    }
}
