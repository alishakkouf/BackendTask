using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using BackendTask.Data.Models;
using BackendTask.Shared;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog.Events;
using BackendTask.Common;
using BackendTask.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BackendTask.Data.Models.ToDoTask;
using BackendTask.Data.Models.Identity;

namespace BackendTask.Data
{
    public class BackendTaskDbContext : IdentityDbContext<UserAccount, UserRole, long>
    {
        internal DbSet<CustomePermission> CustomePermissions { get; set; }
        internal DbSet<ToDoTask> Tasks { get; set; }
        internal DbSet<Category> Category { get; set; }

        private readonly ICurrentUserService _currentUserService;

        public BackendTaskDbContext(DbContextOptions<BackendTaskDbContext> options,
            ICurrentUserService currentUserService)
            : base(options)
        {
            _currentUserService = currentUserService;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries();

            foreach (var entry in entries)
            {
                if (entry.Entity is IAuditedEntity auditedEntity)
                {
                    var userId = _currentUserService.GetUserId();

                    if (entry.State == EntityState.Added)
                    {
                        auditedEntity.CreatedAt = DateTime.UtcNow;
                        auditedEntity.CreatedBy = userId;
                    }

                    if (entry.State == EntityState.Modified)
                    {
                        auditedEntity.ModifiedAt = DateTime.UtcNow;
                        auditedEntity.ModifiedBy = userId;
                    }
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}

