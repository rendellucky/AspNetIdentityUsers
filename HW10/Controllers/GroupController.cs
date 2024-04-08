using HW10.Models;
using HW10.Models.Helpers;
using HW10.Models.Repositories;
using HW10.Models.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HW10.Controllers
{
	[Authorize]
	public class GroupController : Controller
	{
		private readonly GroupRepository _groupRepository;
		private readonly GroupProvider _groupProvider;

		public GroupController(GroupRepository groupRepository, GroupProvider groupProvider)
		{
			_groupRepository = groupRepository;
			_groupProvider = groupProvider;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			ViewData["GroupUserData"] = _groupProvider.GetGroupUserData();
			return View(await _groupRepository.GetModels());
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			return View(new Group());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([FromForm] Group form)
		{
			if (!ModelState.IsValid)
			{
				return View(form);
			}

			var groups = await _groupRepository.GetModels();
			foreach(var group in groups)
			{
				if(group.Title==form.Title)
				{
					ViewData["Message"] = $"{group.Title} already exists";
					return View();
				}
			}

			await _groupRepository.CreateModel(form);
			await _groupRepository.SaveAsync();
			return RedirectToAction("Index");
		}
		[HttpGet]
		public async Task<IActionResult> DeleteConfirm(int id)
		{
			return View(await _groupRepository.GetModel(id));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(int id)
		{
			_groupRepository.DeleteModel(id);
			await _groupRepository.SaveAsync();
			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var group = await _groupRepository.GetModel(id);
			return View(group);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(int id, [FromForm] Group form)
		{
			if (!ModelState.IsValid)
			{
				return View(form);
			}
			var info = await _groupRepository.GetModel(id);
			var groups = await _groupRepository.GetModels();
			foreach (var group in groups)
			{
				if (group.Title == form.Title)
				{
					ViewData["Message"] = $"{group.Title} already exists";
					return View(form);
				}
			}
			info.Title = form.Title;
			_groupRepository.UpdateModel(info);
			await _groupRepository.SaveAsync();
			return RedirectToAction("Index");
		}
	}
}
