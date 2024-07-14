using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project.BLL.Interfaces;
using Project.DAL.Contexts;
using Project.DAL.Models;

namespace Project.BLL.Repositories
{
    public class EmployeeRepository :GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(MVCDbContext dbContext) : base(dbContext)
        {
            
        }

        public IQueryable<Employee> GetEmployeeByAddress(string Address)
        => _dbContext.Employees.Where(E => E.Address.ToLower().Contains(Address.ToLower()));

        public IQueryable<Employee> GetEmployeeByName(string Name)
        => _dbContext.Employees.Where(E => E.Name.ToLower().Contains(Name.ToLower()));

    }
}
