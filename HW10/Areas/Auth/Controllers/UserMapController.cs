using HW10.Models;
using HW10.Models.Forms;
using HW10.Models.Repositories;
using HW10.Models.Repository;
using HW10.Models.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;

namespace HW10.Controllers
{
	[ApiController]
	public class UserMapController : ControllerBase
	{
		private readonly ImageStorage _imageStorage;
		private readonly MapMarkerRepository _mapMarkerRepository;
		private readonly UserManager<UserIdentity> _userManager;

		public UserMapController(ImageStorage imageStorage, MapMarkerRepository mapMarkerRepository, UserManager<UserIdentity> userManager)
		{
			_imageStorage = imageStorage;
			_mapMarkerRepository = mapMarkerRepository;
			_userManager = userManager;
		}

		[HttpGet]
		[Route("api/map/userMarker/{userId}")]
		public async Task<MapMarker> Marker(string userId)
		{
			var markers = await _mapMarkerRepository.GetModels();
			var user = await _userManager.FindByIdAsync(userId);
			return markers.First(m => user.MapMarker?.Id == m.Id);
		}

		[HttpPost]
		[Route("api/map/userMarker")]
		public async Task<object> AddMarker([FromBody] MapMarker marker)
		{
			if (!ModelState.IsValid)
			{
				return new { Ok = false };
			}
			await _mapMarkerRepository.CreateModel(marker);
			await _mapMarkerRepository.SaveAsync();
			return new
			{
				Ok = true,
				Marker = marker
			};
		}

		/*[HttpPost]
		[Route("Auth/User/Edit/api/map/userMarker")]
		public async Task<object> EditMarker([FromBody] MapMarker marker)
		{
			if (!ModelState.IsValid)
			{
				return new { Ok = false };
			}
			await _mapMarkerRepository.CreateModel(marker);
			await _mapMarkerRepository.SaveAsync();
			return new
			{
				Ok = true,
				Marker = marker
			};
		}*/

		[HttpDelete]
		[Route("api/map/userMarker")]
		public async Task<object> DeleteMarker([FromBody] JsonDocument data)
		{
			// get marker
			// get user - marker
			// user marker = null
			// delete marker
			var marker = await _mapMarkerRepository.GetModel(data.RootElement.GetProperty("id").GetInt32());
			var user = await _userManager.Users.Include(u => u.MapMarker).FirstOrDefaultAsync(x => x.MapMarker.Id == marker.Id);
			if (user != null)
			{
				user.MapMarker = null;
				await _userManager.UpdateAsync(user);
			}
			_mapMarkerRepository.DeleteModel(data.RootElement.GetProperty("id").GetInt32());
			await _mapMarkerRepository.SaveAsync();
			return new
			{
				Ok = true
			};
		}

		/*[HttpDelete]
		[Route("Auth/User/Edit/api/map/userMarker")]
		public async Task<object> EditMarker([FromBody] JsonDocument data)
		{
			var marker = await _mapMarkerRepository.GetModel(data.RootElement.GetProperty("id").GetInt32());
			var user = await _userManager.Users.Include(u => u.MapMarker).FirstOrDefaultAsync(x => x.MapMarker.Id == marker.Id);
			if (user != null)
			{
				user.MapMarker = null;
				await _userManager.UpdateAsync(user);
			}
			_mapMarkerRepository.DeleteModel(data.RootElement.GetProperty("id").GetInt32());
			await _mapMarkerRepository.SaveAsync();
			return new
			{
				Ok = true
			};
		}*/
	}
}