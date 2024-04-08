using HW10.Models.Helpers;
using Microsoft.EntityFrameworkCore;

namespace HW10.Models.Services
{
	public class GroupProvider
	{
		private readonly SiteDbContext _context;
		public GroupProvider(SiteDbContext context) 
		{
			_context = context;
		}
		public List<GroupUserCount> GetGroupUserData()
		{
			return _context.UserInfos.Include(x => x.Group).GroupBy(x => x.Group.Id).Select(g => new GroupUserCount { Id = g.Key, Count = g.Count() }).ToList();
		}
	}
}
