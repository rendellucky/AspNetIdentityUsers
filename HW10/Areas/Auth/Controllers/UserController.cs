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
	public class UserController : Controller
	{
		private readonly UserManager<UserIdentity> _userManager;
		private readonly RoleManager<IdentityRole<int>> _roleManager;
		private readonly ImageStorage _imageStorage;
		private readonly MapMarkerRepository _mapMarkerRepository;

		public UserController(UserManager<UserIdentity> userManager, RoleManager<IdentityRole<int>> roleManager, ImageStorage imageStorage, MapMarkerRepository mapMarkerRepository)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_imageStorage = imageStorage;
			_mapMarkerRepository = mapMarkerRepository;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			return View(_userManager.Users.Include(x => x.Image).ToList());
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			var roles = await _roleManager.Roles.ToListAsync();


			var form = new CreateUserWithRolesForm
			{
				Roles = roles.Select(
					x => new UserRoleForm
					{
						Active = false,
						Name = x.Name,
					}).ToList()
			};

			return View(form);
		}
		[HttpPost]
		public async Task<IActionResult> Create([FromForm] CreateUserWithRolesForm form)
		{
			if (!ModelState.IsValid)
			{
				return View(form);
			}

			if (form.Password != form.ConfirmPassword)
			{
				ModelState.AddModelError(nameof(form.ConfirmPassword), "Passwords must match");
				return View(form);
			}

			var user = await _userManager.FindByEmailAsync(form.Login);
			if (user != null)
			{
				ModelState.AddModelError(nameof(form.Login), "Such user already exists");
				return View(form);
			}

			user = new UserIdentity
			{
				Image = await _imageStorage.SaveUploadedFileAsync(form.Image),
				Email = form.Login,
				UserName = form.Login,
				EmailConfirmed = true,
			};
			if (form.MapMarkerId != null)
			{
				user.MapMarker = await _mapMarkerRepository.GetModel(form.MapMarkerId);
			}
			var result = await _userManager.CreateAsync(user, form.Password);

			if (!result.Succeeded)
			{
				ModelState.AddModelError(nameof(form.Login), string.Join(";", result.Errors.ToList().Select(x => x.Description)));
				return View(form);
			}

			foreach (var userRole in form.Roles.Where(x => x.Active))
			{
				await _userManager.AddToRoleAsync(user, userRole.Name);
			}

			return RedirectToAction("Index", "User", new { area = "Auth" });
		}

		[HttpGet]
		public async Task<IActionResult> ResetPassword(int id)
		{
			ViewData["User"] = await _userManager.Users.FirstAsync(x => x.Id == id);
			return View(new ResetPasswordForm());
		}
		[HttpPost]
		public async Task<IActionResult> ResetPassword(int id, [FromForm] ResetPasswordForm form)
		{
			if (!ModelState.IsValid)
			{
				return View(form);
			}

			if (form.Password != form.ConfirmPassword)
			{
				ModelState.AddModelError(nameof(form.ConfirmPassword), "Passwords must match");
				return View(form);
			}

			var user = await _userManager.Users.FirstAsync(x => x.Id == id);
			ViewData["User"] = user;

			if (user.UserName == User.Identity.Name)
			{
				return Forbid();
			}

			var token = await _userManager.GeneratePasswordResetTokenAsync(user);

			var result = await _userManager.ResetPasswordAsync(user, token, form.Password);

			if (!result.Succeeded)
			{
				ModelState.AddModelError(nameof(form.Password), string.Join(";", result.Errors.ToList().Select(x => x.Description)));
				return View(form);
			}

			return RedirectToAction("Index", "User", new { area = "Auth" });
		}


		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var user = await _userManager.Users.FirstAsync(x => x.Id == id);

			var userRoles = await _userManager.GetRolesAsync(user);
			var roles = await _roleManager.Roles.ToListAsync();

			var updatedForm = new UserUpdateForm
			{
				Roles = roles.Select(
					x => new UserRoleForm
					{
						Active = userRoles.Contains(x.Name),
						Name = x.Name,
					}).ToList(),

			};
			if (user.MapMarker != null)
				updatedForm.MapMarkerId = user.MapMarker.Id;

			ViewData["User"] = user;

			return View(updatedForm);
		}
		[HttpPost]
		public async Task<IActionResult> Edit(int id, [FromForm] UserUpdateForm form)
		{
			var model = await _userManager.Users.FirstAsync(x => x.Id == id);

			if (form.MapMarkerId != 0)
			{
				model.MapMarker = await _mapMarkerRepository.GetModel(form.MapMarkerId);
			}

			if (form.Roles != null)
			{
				foreach (var userRole in form.Roles)
				{
					if (userRole.Active)
					{
						if (!await _userManager.IsInRoleAsync(model, userRole.Name))
						{
							await _userManager.AddToRoleAsync(model, userRole.Name);
						}
					}
					else
					{
						if (await _userManager.IsInRoleAsync(model, userRole.Name))
						{
							await _userManager.RemoveFromRoleAsync(model, userRole.Name);
						}
					}
				}
			}

			await _userManager.UpdateAsync(model);

			return RedirectToAction("Index", "User", new { area = "Auth" });
		}

		[HttpGet]
		public async Task<IActionResult> EditAvatar(int id)
		{
			var user = await _userManager.Users.FirstAsync(x => x.Id == id);

			ViewData["User"] = user;

			return View(new UserUpdateAvatarForm());
		}
		[HttpPost]
		public async Task<IActionResult> EditAvatar(int id, [FromForm] UserUpdateAvatarForm form)
		{
			var model = await _userManager.Users.FirstAsync(x => x.Id == id);

			if (form.Image != null)
			{
				if (model.Image != null)
				{
					_imageStorage.RemoveImage(model.Image);
				}
				model.Image = await _imageStorage.SaveUploadedFileAsync(form.Image);
			}

			await _userManager.UpdateAsync(model);

			return RedirectToAction("Index", "User", new { area = "Auth" });
		}
	}
}