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
using System.Text.Json;

namespace HW10.Controllers
{
	[ApiController]
	public class MapController : ControllerBase
	{
		private readonly ImageStorage _imageStorage;
		private readonly MapMarkerRepository _mapMarkerRepository;
		private readonly UserManager<UserIdentity> _userManager;

		public MapController(ImageStorage imageStorage, MapMarkerRepository mapMarkerRepository, UserManager<UserIdentity> userManager)
		{
			_imageStorage = imageStorage;
			_mapMarkerRepository = mapMarkerRepository;
			_userManager = userManager;
		}

		[HttpGet]
		[Route("api/map/markers")]
		public async Task<ICollection<MapMarker>> List()
		{
			return await _mapMarkerRepository.GetModels();
		}

	}
}