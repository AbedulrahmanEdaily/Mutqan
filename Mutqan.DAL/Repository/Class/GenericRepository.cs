using Microsoft.EntityFrameworkCore;
using Mutqan.DAL.Repository.Interface;
using Mutqan.DAL.Data;
using Mutqan.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Repository.Class
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseModel
    {

        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<T> CreateAsync(T request)
        {
            await _context.Set<T>().AddAsync(request);
            await _context.SaveChangesAsync();
            return request;
        }

        public async Task DeleteAsync(T request)
        {
            request.IsDeleted = true;
            _context.Set<T>().Update(request);
            await _context.SaveChangesAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task UpdateAsync(T request)
        {
            _context.Set<T>().Update(request);
            await _context.SaveChangesAsync();
        }
    }
}
