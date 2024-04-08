using HW10.Models.Repository;
using Microsoft.EntityFrameworkCore;

namespace HW10.Models.Repositories
{
    public class UserSkillRepository : IRepository<UserSkill>
    {
        private readonly SiteDbContext _context;

        public UserSkillRepository(SiteDbContext context)
        {
            _context = context;
        }

        public async Task CreateModel(UserSkill model)
        {
            await _context.AddAsync(model);
        }
        public async void DeleteModel(int id)
        {
            var model = await GetModel(id);
            _context.UserSkills.Remove(model);
        }
        public async Task<UserSkill> GetModel(int id)
        {
            return await _context
                .UserSkills
                .Include(x => x.Skill)
                .ThenInclude(x => x.Image)
                .Include(x => x.User)
                .FirstAsync(x => x.Id == id);
        }

        public async Task<List<UserSkill>> GetModels()
        {
            return await _context
                .UserSkills
                .Include(x => x.Skill)
                    .ThenInclude(x => x.Image)
                .Include(x => x.User)
                .ToListAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void UpdateModel(UserSkill model)
        {
            _context.Entry(model).State = EntityState.Modified;
        }
    }
}
