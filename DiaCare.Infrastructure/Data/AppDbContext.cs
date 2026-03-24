using DiaCare.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiaCare.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<HealthProfile> HealthProfiles { get; set; }
        public DbSet<PredictionResult> PredictionResults { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Badge> Badges { get; set; }
        public DbSet<UserBadge> UserBadges { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            
            builder.Entity<HealthProfile>()
                .HasOne(p => p.User)
                .WithMany(u => u.HealthProfiles)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

       
            builder.Entity<PredictionResult>()
                .HasOne(r => r.User)
                .WithMany(u => u.PredictionResults)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PredictionResult>()
                .HasOne(r => r.HealthProfile)
                .WithMany(p => p.PredictionResults) 
                .HasForeignKey(r => r.HealthProfileId)
                .OnDelete(DeleteBehavior.NoAction);

            
            builder.Entity<ChatMessage>()
                .HasOne(m => m.User)
                .WithMany(u => u.ChatMessages)
                .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Notification>()
    .HasOne(n => n.User)
    .WithMany(u => u.Notifications)
    .HasForeignKey(n => n.UserId)
    .OnDelete(DeleteBehavior.Cascade);

            // Composite Key 
            builder.Entity<UserBadge>()
                .HasKey(ub => new { ub.UserId, ub.BadgeId });

            builder.Entity<UserBadge>()
    .HasOne(ub => ub.User)
    .WithMany(u => u.UserBadges)
    .HasForeignKey(ub => ub.UserId);

            builder.Entity<UserBadge>()
                .HasOne(ub => ub.Badge)
                .WithMany(b => b.UserBadges)
                .HasForeignKey(ub => ub.BadgeId);
        }
    }
}
