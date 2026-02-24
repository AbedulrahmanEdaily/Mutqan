using Mutqan.DAL.Data;
using Mutqan.DAL.Models;
using Mutqan.DAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Repository.Class
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _context;

        public RefreshTokenRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(RefreshToken refreshToken)
        {
            await _context.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RefreshToken refreshToken)
        {
            _context.Update(refreshToken);
            await _context.SaveChangesAsync();
        }
    }
}
