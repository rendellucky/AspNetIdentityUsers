using HW10.Models.Repository;
using Microsoft.EntityFrameworkCore;

namespace HW10.Models.Repositories
{
	public class SkillRepository : IRepository<Skill>
	{
		private readonly SiteDbContext _context;

		public SkillRepository(SiteDbContext context)
		{
			_context = context;
		}
		public async Task CreateModel(Skill model)
		{
			await _context.AddAsync(model);
		}

		public async void DeleteModel(int id)
		{
			_context.Remove(await GetModel(id));
		}

		public async Task<Skill> GetModel(int id)
		{
			var skill = await _context.Skills.Include(x=>x.Image)
				.FirstAsync(x => x.Id == id);
			
			return skill;
		}

		public async Task<List<Skill>> GetModels()
		{
			var skills = await _context.Skills.Include(x=>x.Image)
				.ToListAsync();
			
			return skills;
		}

		public async Task SaveAsync()
		{
			await _context.SaveChangesAsync();
		}

		public void UpdateModel(Skill model)
		{
			_context.Entry(model).State = EntityState.Modified;
		}
	}
}
