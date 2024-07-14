using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project.BLL.Interfaces;
using Project.DAL.Contexts;
using Project.DAL.Models;

namespace Project.BLL.Repositories
{
    public class DepartmentRepository :GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(MVCDbContext dbContext):base(dbContext)
        {
            
        }
    }
}
