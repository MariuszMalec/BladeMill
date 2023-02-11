using BladeMill.BLL.Configuration;
using BladeMill.BLL.DAL;
using BladeMill.BLL.DatatAcess;
using Microsoft.EntityFrameworkCore;

namespace BladeMill.BLL.DatatBaseAcess
{
    public class ApplicationDbContext : DbContext//: IdentityDbContext<ApplicationUser, ApplicationRoles, int>
    {
        //public override DbSet<ApplicationUser> Users { get; set; }

        public DbSet<UserDto> Uzytkownicy { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Transfer>().Property(x => x.Amount).HasPrecision(19, 4);
            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Seed();
            //modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
