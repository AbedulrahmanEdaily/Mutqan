using Mutqan.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Repository.Interface
{
    public interface IAttachmentRepository:IScopedRepository
    {
        public Task CreateAsync(Attachment attachment);
        public Task RemoveAsync(Attachment attachment);
        public Task<Attachment?> FindByIdAsync(Guid attachmentId);
        public Task<List<Attachment>> GetAllAsync(Guid taskId);
    }
}
