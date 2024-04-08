using HW10.Models;
using HW10.Models.Forms;
using HW10.Models.Repositories;
using HW10.Models.Repository;
using HW10.Models.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;

namespace HW10.Controllers
{
	public class HomeController : Controller
	{
		private readonly ImageStorage _imageStorage;
		private readonly UserSkillProvider _userSkillProvider;
		private readonly UserRepository _userRepository;
		private readonly GroupRepository _groupRepository;
		private readonly UserSkillRepository _userSkillRepository;
		private readonly SkillRepository _skillRepository;

		public HomeController(ImageStorage imageStorage, UserRepository userRepository, GroupRepository groupRepository, UserSkillRepository userSkillRepository, SkillRepository skillRepository, UserSkillProvider userSkillProvider)
		{
			_imageStorage = imageStorage;
			_userRepository = userRepository;
			_groupRepository = groupRepository;
			_userSkillRepository = userSkillRepository;
			_skillRepository = skillRepository;
			_userSkillProvider = userSkillProvider;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> Show(int id)
		{
			var user = await _userRepository.GetModel(id);
			return View(user);
		}

		public async Task<IActionResult> AboutMe()
		{
			ViewData["Groups"] = await _groupRepository.GetModels();
			ViewData["Skills"] = await _skillRepository.GetModels();
			ViewData["UserSkills"] = await _userSkillRepository.GetModels();
			var users = await _userRepository.GetModels();
			return View(users);
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			ViewData["Groups"] = await _groupRepository.GetModels();
			ViewData["Skills"] = await _skillRepository.GetModels();
			return View(new UserForm());
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromForm] UserForm form)
		{
			if (!ModelState.IsValid)
			{
				ViewData["Groups"] = await _groupRepository.GetModels();
				return View(form);
			}

			var info = new User();
			var random = new Random();
			info.Id = random.Next(1, 1000);
			info.FirstName = form.FirstName;
			info.LastName = form.LastName;
			info.Description = form.Description;
			info.IsActive = form.IsActive;
			if (form.Birthday != null)
			{
				info.Birthday = (DateTime)form.Birthday;
			}
			info.Group = await _groupRepository.GetModel(form.GroupId);
			var skills = await _userSkillRepository.GetModels();

			if (form.Image != null)
			{
				info.Image = await _imageStorage.SaveUploadedFileAsync(form.Image);
			}

			if (form.Images != null)
			{
				foreach (var image in form.Images)
				{
					info.Images.Add(await _imageStorage.SaveUploadedFileAsync(image));
				}
			}

			await _userRepository.CreateModel(info);
			await _userRepository.SaveAsync();
			return RedirectToAction("AboutMe");
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> Edit(int id)
		{
			var user = await _userRepository.GetModel(id);
			ViewData["Groups"] = await _groupRepository.GetModels();
			ViewData["UserInfo"] = user;
			ViewData["Skills"] = await _skillRepository.GetModels();
			ViewData["UserSkills"] = (await _userSkillRepository.GetModels()).Where(x => x.User?.Id == id).ToList();
			return View(new UserForm(user));
		}

		[HttpPost]
		public async Task<IActionResult> Edit(int id, [FromForm] UserForm form)
		{
			if (!ModelState.IsValid)
			{
				return View(form);
			}

			var info = await _userRepository.GetModel(id);
			info.FirstName = form.FirstName;
			info.LastName = form.LastName;
			info.Description = form.Description;
			info.IsActive = form.IsActive;
			if (form.Birthday != null)
			{
				info.Birthday = (DateTime)form.Birthday;
			}
			info.Group = await _groupRepository.GetModel(form.GroupId);
			if (form.Image != null)
			{
				if (info.Image != null)
				{
					_imageStorage.RemoveImage(info.Image);
				}
				info.Image = await _imageStorage.SaveUploadedFileAsync(form.Image);
			}

			if (form.Images != null)
			{
				foreach (var image in form.Images)
				{
					info.Images.Add(await _imageStorage.SaveUploadedFileAsync(image));
				}
			}

			_userRepository.UpdateModel(info);
			await _userRepository.SaveAsync();
			return RedirectToAction("AboutMe");
		}

		[HttpGet]
		public async Task<IActionResult> DeleteConfirm(int id)
		{
			var user = await _userRepository.GetModel(id);
			return View(user);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(int id)
		{
			await _userSkillProvider.DeleteUserSkills(id);
			_userRepository.DeleteModel(id);
			await _userRepository.SaveAsync();
			return RedirectToAction("AboutMe");
		}

		[HttpGet]
		public async Task<IActionResult> DeleteImage(int id)
		{
			var user = await _userRepository.GetModel(id);
			if (user.Image != null)
			{
				_imageStorage.RemoveImage(user.Image);
				user.Image = null;
				await _userRepository.SaveAsync();
			}
			return RedirectToAction("Edit", new { id = id });
		}

		[HttpPost]
		public async Task<IActionResult> DeleteImageFromCarousel(int id, [FromForm] string ImageSrc)
		{
			var user = await _userRepository.GetModel(id);
			if (ImageSrc != null)
			{
				if (user.Images != null)
				{
					var img = user.Images.First(x => x.Src == ImageSrc);
					if (user.Images != null)
					{
						_imageStorage.RemoveImage(img);
						user.Images.Remove(img);
						await _userRepository.SaveAsync();
					}
				}
			}
			return RedirectToAction("Edit", new { id = id });
		}

		[HttpPost]
		public async Task<IActionResult> Search([FromForm] string SearchValue)
		{
			var models = await _userRepository.GetModels();
			var userInfos = models
				.Where(x => x.FirstName.ToLower().Contains(SearchValue.ToLower())
				|| x.LastName.ToLower().Contains(SearchValue.ToLower())
				|| x.Description.ToLower().Contains(SearchValue.ToLower())
				|| x.Birthday.ToString().ToLower().Contains(SearchValue.ToLower())
				|| x.UserSkills.Any(s => s.Skill != null && s.Skill.Name.ToLower().Contains(SearchValue.ToLower()))).ToList();
			ViewData["SearchValue"] = SearchValue;
			ViewData["Groups"] = await _groupRepository.GetModels();
			ViewData["Skills"] = await _skillRepository.GetModels();
			ViewData["UserSkills"] = await _userSkillRepository.GetModels();
			return View("AboutMe", userInfos);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}