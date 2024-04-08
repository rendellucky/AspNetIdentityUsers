using HW10.Models.Helpers;
using HW10.Models.Repository;
using Microsoft.EntityFrameworkCore;

namespace HW10.Models.Repositories
{
	public class GroupRepository : IRepository<Group>
	{
		private SiteDbContext _context;

		public GroupRepository(SiteDbContext context)
		{
			_context = context;
		}
		public async Task CreateModel(Group model)
		{
			await _context.AddAsync(model);
		}

		public void DeleteModel(int id)
		{
			var group = _context.Groups.First(g=> g.Id == id);
			_context.Groups.Remove(group);
		}

		public async Task<Group> GetModel(int id)
		{
			return await _context.Groups.FirstAsync(g=>g.Id == id);
		}

		public async Task<List<Group>> GetModels()
		{
			return await _context.Groups.ToListAsync();
		}

		public async Task SaveAsync()
		{
			await _context.SaveChangesAsync();
		}

		public void UpdateModel(Group model)
		{
			_context.Entry(model).State = EntityState.Modified;
		}
	}
}
