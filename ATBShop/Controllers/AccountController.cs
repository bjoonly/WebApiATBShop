using ATBShop.Exceptions;
using ATBShop.Helpers;
using ATBShop.Interfaces;
using ATBShop.Models;
using AutoMapper;
using DAL.Data;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Imaging;

namespace ATBShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppEFContext _context;
        public AccountController(UserManager<AppUser> userManager,
            IJwtTokenService jwtTokenService, IMapper mapper, AppEFContext context)
        {
            _userManager = userManager;
            _mapper = mapper;
            _jwtTokenService = jwtTokenService;
            _context = context;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            var img = ImageWorker.FromBase64StringToImage(model.Photo);
            string randomFilename = Path.GetRandomFileName() + ".jpg";
            var dir = Path.Combine(Directory.GetCurrentDirectory(), "uploads", randomFilename);
            img.Save(dir, ImageFormat.Jpeg);
            var user = _mapper.Map<AppUser>(model);
            user.Photo = randomFilename;
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(new { errors = result.Errors });

            return Ok(new { token = _jwtTokenService.CreateTokenAsync(user).Result });
        }
        [HttpGet]
        [Authorize]
        [Route("users")]
        public async Task<IActionResult> Users()
        {
            var list = _context.Users.Select(x => _mapper.Map<UserItemViewModel>(x)).ToList();

            return Ok(list);
        }

        /// <summary>
        /// Login on site
        /// </summary>
        /// <param name="model">User email and password </param>
        /// <returns>Authorization token</returns>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {

                if (await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    return Ok(new { token = _jwtTokenService.CreateTokenAsync(user).Result });
                }
            }
            return BadRequest(new { error = "User doesn't exist" });
        }

        [HttpGet]
        [Route("get-user/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
                throw new AppException($"User with id {id} doesn't exist.");

            return Ok(_mapper.Map<UserItemViewModel>(user));
        }

        [HttpPut]
        [Route("edit-user/{id}")]
        public async Task<IActionResult> EditUser(int id, EditUserViewModel model)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
                throw new AppException($"User with id {id} doesn't exist.");

            if (model.Photo != null)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), model.Photo.Replace("images", "uploads").Remove(0, 1));

                if (!System.IO.File.Exists(filePath))
                {
                    filePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", user.Photo);
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);

                    var img = ImageWorker.FromBase64StringToImage(model.Photo);
                    string randomFilename = Path.GetRandomFileName() + ".jpg";
                    var dir = Path.Combine(Directory.GetCurrentDirectory(), "uploads", randomFilename);
                    img.Save(dir, ImageFormat.Jpeg);

                    user.Photo = randomFilename;
                }
            }

            user.Email = model.Email;
            user.FirstName = model.FirstName;
            user.SecondName = model.SecondName;
            user.PhoneNumber = model.Phone;

            await _userManager.UpdateAsync(user);

            return Ok(_mapper.Map<UserItemViewModel>(user));
        }
    }
}
