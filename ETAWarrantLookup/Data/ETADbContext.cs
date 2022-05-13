using ETAWarrantLookup.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using System;

namespace ETAWarrantLookup.Data
{
    public class Subscription
    {
        public string SubscriptionId { get; set; }
        public string ApplicationUserId { get; set; }
        public string PaymentId { get; set; }
        public string PaymentToken { get; set; }
        public DateTime PaymentDate { get; set; }
    }



    public class ETADbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ETADbContext(DbContextOptions<ETADbContext> options)
    : base(options)
        {

        }

        public DbSet<Subscription> Subscriptions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //seed admin account and claim
            // any guid
            const string ADMIN_ID = "a18be9c0-aa65-4af8-bd17-00bd9344e575";
            
            // any guid, but nothing is against to use the same one
            const string ROLE_ID = ADMIN_ID;

            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = ROLE_ID,
                Name = "admin",
                NormalizedName = "admin"
            });

            var hasher = new PasswordHasher<ApplicationUser>();
            modelBuilder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                Id = ADMIN_ID,
                UserName = "admin",
                NormalizedUserName = "admin",
                Email = "some-admin-email@nonce.fake",
                NormalizedEmail = "some-admin-email@nonce.fake",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "SOME_ADMIN_PLAIN_PASSWORD"),
                SecurityStamp = string.Empty
            });

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = ROLE_ID,
                UserId = ADMIN_ID
            });
        }
    }
}

