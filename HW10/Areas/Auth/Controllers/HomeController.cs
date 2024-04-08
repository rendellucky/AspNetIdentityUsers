using HW10.Areas.Auth.Models.Forms;
using HW10.Models;
using HW10.Models.Forms;
using HW10.Models.Repositories;
using HW10.Models.Repository;
using HW10.Models.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

namespace HW10.Areas.Auth.Controllers
{
    [Area("Auth")]
    public class HomeController : Controller
    {
        private readonly UserManager<UserIdentity> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

		public HomeController(UserManager<UserIdentity> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> LoginModal()
        {
            return PartialView(new LoginForm());
        }

		[HttpPost]
        public async Task<IActionResult> LoginModal([FromForm] LoginForm form, string? returnUrl)
        {
			if (!ModelState.IsValid)
			{
				return PartialView(form);
			}

			var user = await _userManager.FindByEmailAsync(form.Login);
			if (user == null)
			{
				ModelState.AddModelError(nameof(form.Login), "Such user doesn't exist");
				return PartialView(form);
			}

            if(!await _userManager.CheckPasswordAsync(user, form.Password))
            {
				ModelState.AddModelError(nameof(form.Password), "Wrong password");
				return PartialView(form);
			}

            await SignIn(user);

			if (returnUrl != null)
			{
				return Redirect(returnUrl);
			}

			return RedirectToAction("Index", "Home", new { area = "" });
        }

		[HttpGet]
		public async Task<IActionResult> ProfileModal()
		{
            var user = await _userManager.FindByEmailAsync(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value);
            ViewData["CurrentUser"] = user;
			return PartialView();
		}

		[HttpGet]
        public async Task<IActionResult> RegisterModal()
        {
            return PartialView(new RegisterForm());
        }
        [HttpPost]
        public async Task<IActionResult> RegisterModal([FromForm] RegisterForm form, string? returnUrl)
        {
			if (!ModelState.IsValid)
            {
                return PartialView(form);
            }

            if (form.Password != form.ConfirmPassword)
            {
                ModelState.AddModelError(nameof(form.ConfirmPassword), "Passwords must match");
                return PartialView(form);
            }

            var user = await _userManager.FindByEmailAsync(form.Login);
            if (user != null)
            {
                ModelState.AddModelError(nameof(form.Login), "Such user already exists");
                return PartialView(form);
            }

            user = new UserIdentity
            {
                Email = form.Login,
                UserName = form.Login,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, form.Password);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(nameof(form.Login), string.Join(";", result.Errors.ToList().Select(x => x.Description)));
                return PartialView(form);
            }

            var roleUser = await _roleManager.Roles.FirstAsync(x => x.Name == "User");
            await _userManager.AddToRoleAsync(user, roleUser.Name);

            await SignIn(user);

            if(returnUrl != null)
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home", new { area = "" });
        }

        private async Task SignIn(UserIdentity user)
        {
			var identity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);
			identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
			identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
			identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach(var role in userRoles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

			await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, new ClaimsPrincipal(identity));
		}

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            return RedirectToAction("Index", "Home", new { area = "" });
        }
        [HttpGet]
        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }
    }
}