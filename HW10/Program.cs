using HW10.Models;
using HW10.Models.Middlewares;
using HW10.Models.Repositories;
using HW10.Models.Repository;
using HW10.Models.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HW10
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();
			builder.Services.AddScoped<ImageStorage>();
			builder.Services.AddScoped<GroupProvider>();
			builder.Services.AddScoped<UserSkillProvider>();
			builder.Services.AddScoped<GroupRepository>();
			builder.Services.AddScoped<UserRepository>();
			builder.Services.AddScoped<UserSkillRepository>();
			builder.Services.AddScoped<SkillRepository>();
			builder.Services.AddScoped<MapMarkerRepository>();
			builder.Services.AddDbContext<SiteDbContext>(options =>
			{
				options.UseSqlite("filename=users.db");
			});

			builder.Services.AddIdentity<UserIdentity, IdentityRole<int>>(options =>
			{
				options.SignIn.RequireConfirmedPhoneNumber = false;
				options.SignIn.RequireConfirmedEmail = false;
				options.SignIn.RequireConfirmedAccount = false;

				options.Password.RequiredLength = 3;
				options.Password.RequiredUniqueChars = 0;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireDigit = false;
				options.Password.RequireLowercase = false;
				options.Password.RequireUppercase = false;
			})
				.AddRoles<IdentityRole<int>>()
				.AddEntityFrameworkStores<SiteDbContext>()
				.AddDefaultTokenProviders();

			builder.Services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = new PathString("/Auth/Home/Login");
				options.LogoutPath = new PathString("/Auth/Home/Logout");
				options.AccessDeniedPath = new PathString("/Auth/Home/AccessDenied");
			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();
			app.UseMiddleware<CurrentUserMiddleware>();

			try
			{
				var roleManager = app.Services.CreateScope().ServiceProvider.GetService<RoleManager<IdentityRole<int>>>();
				if (!roleManager.RoleExistsAsync("Admin").Result)
				{
					roleManager.CreateAsync(new IdentityRole<int>() { Name = "Admin" }).Wait();
				}
				if (!roleManager.RoleExistsAsync("User").Result)
				{
					roleManager.CreateAsync(new IdentityRole<int>() { Name = "User" }).Wait();
				}
			}
			catch (Exception) { }

			app.MapControllerRoute(
				name: "areas",
				pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
				);

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}