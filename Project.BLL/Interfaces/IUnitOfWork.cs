using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Interfaces
{
	public interface IUnitOfWork
	{
        public IEmployeeRepository _employeeRepository { get; set; }
        public IDepartmentRepository _departmentRepository { get; set; }

        public Task<int> SaveChangeAsync();
    }
}
