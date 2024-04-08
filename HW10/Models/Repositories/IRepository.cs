using HW10.Models.Forms;
using HW10.Models.Services;
using Microsoft.AspNetCore.Mvc;

namespace HW10.Models.Repository
{
	public interface IRepository<T> where T : class
	{
		public Task<T> GetModel(int id);
		public Task<List<T>> GetModels();
		public Task CreateModel(T model);
		public void UpdateModel(T model);
		public void DeleteModel(int id);
		public Task SaveAsync();
	}
}
