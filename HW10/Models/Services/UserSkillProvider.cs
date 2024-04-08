using HW10.Models.Helpers;
using Microsoft.EntityFrameworkCore;

namespace HW10.Models.Services
{
    public class UserSkillProvider
    {
        private readonly SiteDbContext _context;
        public UserSkillProvider(SiteDbContext context)
        {
            _context = context;
        }
        public List<UserSkillCount> GetUserSkillData()
        {
            //return _context.UserSkills.Include(x => x.Skill).Include(x=>x.User).GroupBy(x => x.Skill.Id).Select(g => new UserSkillCount { Id = g.Key, Count = g.Select(x=>x.User.Id).Count() }).ToList();
            return _context.UserInfos
                .Include(x => x.UserSkills)
                    .ThenInclude(x => x.Skill)
                .SelectMany(x => x.UserSkills)
                .GroupBy(x => x.Skill.Id)
                .Select(g => new UserSkillCount { Id = g.Key, Count = g.Select(x => x.User.Id).Count() })
                .ToList();
        }
		public async Task DeleteUserSkills(int userId)
		{
			var userSkills = await _context.UserSkills.Include(x=>x.User).Where(x => x.User.Id == userId).ToListAsync();
			_context.UserSkills.RemoveRange(userSkills);
		}
	}
}
