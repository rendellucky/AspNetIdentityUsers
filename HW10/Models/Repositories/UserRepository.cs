using HW10.Models.Forms;
using HW10.Models.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace HW10.Models.Repository
{
	public class UserRepository : IRepository<User>
	{
		private SiteDbContext _context { get; set; }

		private ImageStorage _imageStorage { get; set; }

		public UserRepository(SiteDbContext context, ImageStorage imageStorage)
		{
			this._context = context;
			this._imageStorage = imageStorage;
		}

		public async Task CreateModel(User model)
		{
			await _context.AddAsync(model);
		}

		public async void DeleteModel(int id)
		{
			var user = await GetModel(id);
			if (user.Image != null)
			{
				_imageStorage.RemoveImage(user.Image);
			}
			_context.UserInfos.Remove(user);
		}

		public async Task<User> GetModel(int id)
		{
			return await _context
				.UserInfos
				.Include(x => x.Group)
				.Include(x => x.Image)
				.Include(x => x.Images)
				.Include(x=>x.UserSkills)
				.ThenInclude(x=>x.Skill)
				.FirstAsync(x => x.Id == id);
		}

		public async Task<List<User>> GetModels()
		{
			return await _context
				.UserInfos
				.Include(x => x.Group)
				.Include(x => x.Image)
				.Include(x => x.Images)
				.Include(x=>x.UserSkills)
				.ThenInclude(x => x.Skill)
				.ToListAsync();
		}

		public void UpdateModel(User model)
		{
			_context.Entry(model).State = EntityState.Modified;
		}

		public async Task SaveAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}
