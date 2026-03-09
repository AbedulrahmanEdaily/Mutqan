using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Mutqan.DAL.Data;
using Mutqan.DAL.Models;
using Mutqan.DAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Repository.Class
{
    public class AttachmentRepository : IAttachmentRepository
    {
        private readonly AppDbContext _context;

        public AttachmentRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(Attachment attachment)
        {
            await _context.AddAsync(attachment);
            await _context.SaveChangesAsync();
        }

        public async Task<Attachment?> FindByIdAsync(Guid attachmentId)
        {
            return await _context.Attachments
                .Include(a => a.Task)
                .Include(a => a.UploadedBy)
                .FirstOrDefaultAsync(a => a.Id == attachmentId && !a.IsDeleted);
        }

        public async Task<List<Attachment>> GetAllAsync(Guid taskId)
        {
            return await _context.Attachments
                .Include(a => a.Task)
                .Include(a => a.UploadedBy)
                .Where(a => a.TaskId == taskId && !a.IsDeleted)
                .ToListAsync();
        }

        public async Task RemoveAsync(Attachment attachment)
        {
            attachment.IsDeleted = true;
            _context.Attachments.Update(attachment);
            await _context.SaveChangesAsync();
        }
    }
}
