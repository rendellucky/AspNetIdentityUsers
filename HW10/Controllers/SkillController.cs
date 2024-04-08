using HW10.Models;
using HW10.Models.Forms;
using HW10.Models.Repositories;
using HW10.Models.Repository;
using HW10.Models.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HW10.Controllers
{
	[Authorize(Roles = "Admin")]
	public class SkillController : Controller
	{
		private readonly UserSkillRepository _userSkillRepository;
		private readonly SkillRepository _skillRepository;
		private readonly UserSkillProvider _userSkillProvider;
		private readonly ImageStorage _imageStorage;

		public SkillController(UserSkillProvider userSkillProvider, ImageStorage imageStorage, UserSkillRepository userSkillRepository, SkillRepository skillRepository)
		{
			_userSkillRepository = userSkillRepository;
			_userSkillProvider = userSkillProvider;
			_imageStorage = imageStorage;
			_skillRepository = skillRepository;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			ViewData["UserSkillData"] = _userSkillProvider.GetUserSkillData();
			var models = await _skillRepository.GetModels();
			return View(models);
		}

		[HttpGet]

		public async Task<IActionResult> Create()
		{
			return View(new SkillForm());
		}

		[HttpPost]

		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([FromForm] SkillForm form)
		{
			if (!ModelState.IsValid)
			{
				return View(form);
			}
			var info = new Skill();
			info.Name = form.Name;
			if (form.Image != null)
			{
				info.Image = await _imageStorage.SaveUploadedFileAsync(form.Image);
			}
			var userSkills = await _userSkillRepository.GetModels();
			foreach (var userSkill in userSkills)
			{
				if (userSkill.Skill.Name == form.Name)
				{
					ViewData["Message"] = $"{userSkill.Skill.Name} already exists";
					return View();
				}
			}

			await _skillRepository.CreateModel(info);
			await _skillRepository.SaveAsync();
			return RedirectToAction("Index");
		}
		[HttpGet]

		public async Task<IActionResult> Edit(int id)
		{
			var skill = await _skillRepository.GetModel(id);
			return View(new SkillForm(skill));
		}

		[HttpPost]

		public async Task<IActionResult> Edit(int id, [FromForm] SkillForm form)
		{
			if (!ModelState.IsValid)
			{
				return View(form);
			}

			var info = await _skillRepository.GetModel(id);
			info.Name = form.Name;
			if (form.Image != null)
			{
				if (info.Image != null)
				{
					_imageStorage.RemoveImage(info.Image);
				}
				info.Image = await _imageStorage.SaveUploadedFileAsync(form.Image);
			}

			_skillRepository.UpdateModel(info);
			await _skillRepository.SaveAsync();
			return RedirectToAction("Index");
		}
		[HttpGet]

		public async Task<IActionResult> DeleteConfirm(int id)
		{
			var user = await _skillRepository.GetModel(id);
			return View(user);
		}

		[HttpPost]

		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(int id)
		{
			_skillRepository.DeleteModel(id);
			await _userSkillRepository.SaveAsync();
			await _skillRepository.SaveAsync();
			return RedirectToAction("Index");
		}
	}
}