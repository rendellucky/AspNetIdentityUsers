using HW10.Models.Helpers;
using HW10.Models.Repository;
using Microsoft.EntityFrameworkCore;

namespace HW10.Models.Repositories
{
	public class MapMarkerRepository : IRepository<MapMarker>
	{
		private SiteDbContext _context;

		public MapMarkerRepository(SiteDbContext context)
		{
			_context = context;
		}
		public async Task CreateModel(MapMarker model)
		{
			await _context.AddAsync(model);
		}

		public void DeleteModel(int id)
		{
			var mapMarker = _context.MapMarkers.First(m=> m.Id == id);
			_context.MapMarkers.Remove(mapMarker);
		}

		public async Task<MapMarker> GetModel(int id)
		{
			return await _context.MapMarkers.FirstAsync(m=>m.Id == id);
		}

		public async Task<List<MapMarker>> GetModels()
		{
			return await _context.MapMarkers.ToListAsync();
		}

		public async Task SaveAsync()
		{
			await _context.SaveChangesAsync();
		}

		public void UpdateModel(MapMarker model)
		{
			_context.Entry(model).State = EntityState.Modified;
		}
	}
}
