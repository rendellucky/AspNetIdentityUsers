using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HW10.Models
{
	public class SiteDbContext : IdentityDbContext<UserIdentity, IdentityRole<int>, int>
	{
		public SiteDbContext(DbContextOptions<SiteDbContext> options) : base(options) { }
		public virtual DbSet<User> UserInfos { get; set; }
		public virtual DbSet<Image> Images { get; set; }
		public virtual DbSet<Group> Groups { get; set; }
		public virtual DbSet<UserSkill> UserSkills { get; set; }
		public virtual DbSet<Skill> Skills { get; set; }
		public virtual DbSet<MapMarker> MapMarkers { get; set; }
	}
}