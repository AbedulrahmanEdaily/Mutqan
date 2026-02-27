using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Repository.Interface
{
    public interface IGenericRepository<T>
    {
        Task<List<T>> GetAllAsync();
        Task<T> CreateAsync(T request);
        Task DeleteAsync(T request);
        Task UpdateAsync(T request);
    }
}
