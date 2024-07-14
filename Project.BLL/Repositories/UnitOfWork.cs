using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.BLL.Interfaces;
using Project.DAL.Contexts;

namespace Project.BLL.Repositories
{
	public class UnitOfWork : IUnitOfWork, IDisposable
	{
		private readonly MVCDbContext _dbContext;

		public IEmployeeRepository _employeeRepository { get; set; }
		public IDepartmentRepository _departmentRepository { get; set; }
        public UnitOfWork(MVCDbContext dbContext)
        {
            _employeeRepository = new EmployeeRepository(dbContext);
            _departmentRepository = new DepartmentRepository(dbContext);
			_dbContext = dbContext;
		}

		public async Task<int> SaveChangeAsync()
		=> await _dbContext.SaveChangesAsync();

		public void Dispose()
		{
			_dbContext.Dispose();
		}
	}
}
