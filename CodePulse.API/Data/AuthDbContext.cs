using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "bfb8abed-0224-42f6-9977-953ef008c116";
            var writerRoleId = "3e35f33e-61fe-464e-835b-f628967e3b6c]";
            
            //Create Reader and Writer Role
            var roles = new List<IdentityRole> {
                new IdentityRole{Id=readerRoleId, Name = "Reader", NormalizedName = "READER", ConcurrencyStamp = readerRoleId},
                new IdentityRole{Id = writerRoleId, Name = "Writer", NormalizedName = "WRITER", ConcurrencyStamp = writerRoleId}
            };

            //Seed the roles
            builder.Entity<IdentityRole>().HasData(roles);

            //Create an Admin user
            var adminUserId = "8fe8776a-37ab-47e8-a1b2-1a8befdb0f91";
            var adminEmail = "admin@codepulse.com";
            
            var admin = new IdentityUser
            {
                Id = adminUserId,
                UserName = adminEmail,
                NormalizedUserName = adminEmail.ToUpper(),
                Email = adminEmail,
                NormalizedEmail = adminEmail.ToUpper()
            };

            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Password1!");

            builder.Entity<IdentityUser>().HasData(admin);
            
            //Give Roles to Admin
            var adminRoles = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    UserId = adminUserId,
                    RoleId = readerRoleId
                },
                new()
                {
                    UserId = adminUserId,
                    RoleId = writerRoleId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);

        }
    }
}
