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
                .HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PredictionResult>()
                .HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PredictionResult>()
        .HasOne<HealthProfile>()
        .WithOne()
        .HasForeignKey<PredictionResult>(r => r.HealthProfileId)
        .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Notification>()
        .HasOne<ApplicationUser>()
        .WithMany()
        .HasForeignKey(n => n.UserId)
        .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ChatMessage>()
        .HasOne<ApplicationUser>()
        .WithMany()
        .HasForeignKey(m => m.UserId)
        .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserBadge>()
                    .HasKey(ub => new { ub.UserId, ub.BadgeId }); // Composite Key

            builder.Entity<UserBadge>()
                .HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(ub => ub.UserId);

            builder.Entity<UserBadge>()
                .HasOne<Badge>()
                .WithMany()
                .HasForeignKey(ub => ub.BadgeId);

            // (Decimal Precision)

            builder.Entity<HealthProfile>(entity =>
            {
                entity.Property(e => e.Bmi).HasColumnType("decimal(5,2)");
                entity.Property(e => e.InsulinLevel).HasColumnType("decimal(5,2)");
                entity.Property(e => e.HbA1cLevel).HasColumnType("decimal(5,2)");
                entity.Property(e => e.SugarIntakeGramsPerDay).HasColumnType("decimal(5,2)");
            });
        }
    }
}
