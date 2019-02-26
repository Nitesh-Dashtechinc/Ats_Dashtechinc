using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Ats.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(): base("AtsConnstring", throwIfV1Schema: false)
        {
            //Enable Migration
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Migrations.Configuration>());
            Database.SetInitializer<ApplicationDbContext>(null);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<Department> Department { get; set; }
        public DbSet<Designation> Designation { get; set; }
        public DbSet<InterEducBackground> InterEducBackground { get; set; }
        public DbSet<InterReference> InterReference { get; set; }
        public DbSet<InterPersonalInfo> InterPersonalInfo { get; set; }
        public DbSet<InterPreEmpDetail> InterPreEmpDetail { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<InterLanguage> InterLanguages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}