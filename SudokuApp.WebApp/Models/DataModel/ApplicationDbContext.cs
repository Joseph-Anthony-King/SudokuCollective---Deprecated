using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SudokuApp.Models;

namespace SudokuApp.WebApp.Models.DataModel {

    public class ApplicationDbContext : DbContext {
        
        private IConfiguration configuration;

        public ApplicationDbContext(
                DbContextOptions<ApplicationDbContext> options,
                IConfiguration iConfigService
            ) : base(options) {

            configuration = iConfigService;
        }

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Difficulty> Difficulties { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
            => optionsBuilder.UseNpgsql(configuration.GetValue<string>("ConnectionStrings:DatabaseConnection"));

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.ForNpgsqlUseIdentityColumns();

            modelBuilder.Entity<Permission>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Difficulty>()
                .HasKey(p => p.Id);
        }
    }
}