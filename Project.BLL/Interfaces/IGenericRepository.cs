using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.DAL.Models;

namespace Project.BLL.Interfaces
{
    public interface IGenericRepository<T> 
    {
        Task AddAsync(T employee);
        void Delete(T employee);
        void Update(T employee);
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
    }
}
