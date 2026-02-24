using Mutqan.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Repository.Interface
{
    public interface IRefreshTokenRepository : IScopedRepository
    {
        Task UpdateAsync(RefreshToken refreshToken);
        Task AddAsync(RefreshToken refreshToken);
    }
}
