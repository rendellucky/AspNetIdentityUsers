using HW10.Models.Forms;
using HW10.Models.Repositories;
using HW10.Models.Repository;
using HW10.Models.Services;
using HW10.Models;
using Microsoft.AspNetCore.Mvc;

namespace HW10.Controllers
{
	public class UserSkillController : Controller
	{
		private readonly SkillRepository _skillRepository;
		private readonly UserRepository _userRepository;
		private readonly UserSkillRepository _userSkillRepository;

		public UserSkillController(SkillRepository skillRepository, UserRepository userRepository, UserSkillRepository userSkillRepository)
		{
			_skillRepository = skillRepository;
			_userRepository = userRepository;
			_userSkillRepository = userSkillRepository;
		}

		[HttpGet]
		public async Task<IActionResult> Create(int id)
		{
			var user = await _userRepository.GetModel(id);
			var skills = (await _skillRepository.GetModels()).Except(user.UserSkills.Select(x => x.Skill)).ToList();
			ViewData["Skills"] = skills;
			if (skills.Count == 0)
			{
				TempData["Message"] = "You already have all skills";
				return RedirectToAction("Edit", "Home", new { id = id });
			}
			return View(new UserSkillForm());
		}

		[HttpPost]
		public async Task<IActionResult> Create(int id, [FromForm] UserSkillForm form)
		{
			var user = await _userRepository.GetModel(id);
			if (!ModelState.IsValid)
			{
				return View(form);
			}

			var info = new UserSkill();
			info.User = user;
			info.Skill = await _skillRepository.GetModel(form.SkillId);
			info.Level = form.Level;

			await _userSkillRepository.CreateModel(info);
			await _userSkillRepository.SaveAsync();
			return RedirectToAction("Edit", "Home", new { id = id });
		}
		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var userSkill = await _userSkillRepository.GetModel(id);
			var user = await _userRepository.GetModel(userSkill.User.Id);

			var skills = (await _skillRepository.GetModels()).Except(user.UserSkills.Select(x => x.Skill)).ToList();
			if (skills.Count == 0)
			{
				var currentSkill = await _skillRepository.GetModel(userSkill.Skill.Id);
				ViewData["Skills"] = new List<Skill> { currentSkill };
			}
			else
				ViewData["Skills"] = skills;

			return View(new UserSkillForm(userSkill));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [FromForm] UserSkillForm form)
		{
			var userSkill = await _userSkillRepository.GetModel(id);
			var user = await _userRepository.GetModel(userSkill.User.Id);


			if (!ModelState.IsValid)
			{
				ViewData["Skills"] = (await
					_skillRepository
				.GetModels())
				.Except(user.UserSkills.Select(x => x.Skill)).ToList();

				return View(form);
			}

			userSkill.Skill = await _skillRepository.GetModel(form.SkillId);
			userSkill.Level = form.Level;

			_userSkillRepository.UpdateModel(userSkill);
			await _userSkillRepository.SaveAsync();
			return RedirectToAction("Edit", "Home", new { id = user.Id });
		}
        [HttpGet]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var userSkill = await _userSkillRepository.GetModel(id);
            var user = await _userRepository.GetModel(userSkill.User.Id);
			ViewData["UserId"] = user.Id; 
            return View(await _userSkillRepository.GetModel(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userSkill = await _userSkillRepository.GetModel(id);
            var user = await _userRepository.GetModel(userSkill.User.Id);
            _userSkillRepository.DeleteModel(id);
            await _userSkillRepository.SaveAsync();
            return RedirectToAction("Edit", "Home", new { id = user.Id });
        }
    }
}